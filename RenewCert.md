# Automated certificate renewals

All of the network routes for non-production environments use the OCP4 platform wildcard certificate. No configuration is required.

For production, a certificate from Entrust is used. This certificate must be renewed every year; while the actual installation process is automated, the issuing/renewal of the updated certifcate must go through a manual approval process.

Certbot is used to (partially) automate this process (see below).


## Certbot

Common Services maintains [Certbot](https://github.com/BCDevOps/certbot), an OpenShift-specific wrapper around the [EFF's Certbot](https://certbot.eff.org) tool, which is most often used with [Let's Encrypt](https://letsencrypt.org) but can also be used with other ACME servers (e.g. Entrust). 

When configured correctly, it'll automatically install certificates when they're available, or request one when the currently-installed one is about to expire.


### Cronjob

Once Certbot is set up, a cronjob runs to check if a new certificate is available. If not, it does nothing, but if the existing certificate expires in less than a month, it'll submit a [ACME certificate request](https://en.wikipedia.org/wiki/Automatic_Certificate_Management_Environment) to Entrust for approval.

The product owner (or whatever email Certbot is configured with) will then receive an automated email from Entrust acknowledging this request. However, it does not automatically approve the request (see next section).

By default, the cronjob runs late afternoon every Sunday and Wednesday (`0 0 * * 1,4`, i.e. Monday and Thursday midnight UTC).


### Manual approval process

All Entrust certificate requests must be externally approved. 

Within the NRM, this is handled by submitting a request to the [Service Desk](https://apps.nrs.gov.bc.ca/int/jira/servicedesk/customer/portal/1) with the following info:

* Domain for the certificate
* Entrust Tracking ID
  * This can be found inside the autoamted email from Entrust
* iStore Coding
* Expense Authority

Once approved, Entrust will send another automated email informing the product owner. The approval is valid for 2 weeks, during which Certbot will install the updated certificate the next time it runs.

If the certificate is not installed within that timeframe, you'll have to start over with the request process; i.e. Certbot (automatically) submits an ACME certificate request, Entrust will send an email with the Tracking ID, and another ticket will need to be submitted with the Service Desk.


### Setup

*For the latest setup instructions, please see the [Common Services Certbot repo](https://github.com/BCDevOps/certbot/blob/master/docker/README.md).*

1. Set an environment variable for the name of the OpenShift namespace for convenience:
    ```sh
    export NAMESPACE=xxxxxx-prod
    ```
2. Tag the appropriate network route with `certbot-managed=true`. This can be done inside the OpenShift web console under **Networking** > **Routes** > `your-route-here` > **Labels** > **Edit**. To verify:
    ```sh
    oc get route -n $NAMESPACE -l certbot-managed=true -o=jsonpath='{range .items[*]}{.metadata.name}{"\n"}{end}'
    ```
3. Set up the Certbot cronjob. Edit the values for `CERTBOT_EMAIL` and `CERTBOT_SERVER` as appropriate:
    ```sh
    export CERTBOT_EMAIL=Product.Owner@gov.bc.ca
    export CERTBOT_SERVER=https://www.entrust.net/acme/api/v1/directory/XX-XXXX-XXXX
    oc process -n $NAMESPACE -f "https://raw.githubusercontent.com/BCDevOps/certbot/master/openshift/certbot.dc.yaml" -p CERTBOT_EMAIL=$CERTBOT_EMAIL -p CERTBOT_SERVER=$CERTBOT_SERVER -p CERTBOT_DEBUG=true -p CERTBOT_SUBSET=true|oc apply -n $NAMESPACE -f -
    ```
    For `CERTBOT_EMAIL`, it's recommended that you use a shared inbox, in order to avoid having to constantly update the email (or forwarding the Entrust emails) whenever the project changes hands.

    The value for `CERTBOT_SERVER` can be found on [Confluence](https://apps.nrs.gov.bc.ca/int/confluence/display/SHOWCASE/Certbot+SSL+Renewal+Process?). If it's out of date, ask around on Rocket.Chat or the internal Stack Overflow.


### Troubleshooting

#### Manually running the cronjob

If you don't want to wait for the next scheduled run, the cronjob can also be triggered manually:

```sh
oc create job -n $NAMESPACE "certbot-manual-$(date +%s)" --from=cronjob/certbot
```

(replace `$NAMESPACE` with the corresponding OpenShift namespace; e.g. `xxxxxx-prod`)

Clean up afterwards by deleting the finished cronjob once you're done:

```sh
oc get job -n $NAMESPACE -o name |grep -F -e '-manual-'|xargs oc delete -n $NAMESPACE
```


#### Manually backing up certificates

Certbot stores its configuration within a PersistentVolumeClaim (PVC) called `certbot`, inside `/etc/letsencrypt`.

As certifcates/keys can't be downloaded from Entrust at a later point in time, it might be a good idea to back things up before wiping the PVC, if necessary:

1. Create a dummy pod that mounts `/etc/letsencrypt`. This one uses the `httpd` image, but you can use anything:

    ```sh
    export NAMESPACE=xxxxxx-prod
    echo '{"apiVersion":"v1","kind":"Pod","metadata":{"name":"dummy-pod","labels":{"app":"httpd"},"namespace":"'"$NAMESPACE"'"},"spec":{"containers":[{"name":"httpd","image":"image-registry.openshift-image-registry.svc:5000/openshift/httpd:latest","ports":[{"containerPort":8080}],"volumeMounts":[{"name":"certbot-config","mountPath":"/etc/letsencrypt"}]}],"volumes":[{"name":"certbot-config","persistentVolumeClaim":{"claimName":"certbot"}}]}}' | oc create -f -
    ```
2. Wait for the pod to spin up. You can check this under **Workloads** > **Pods**. 
3. Download the entire `/etc/letsencrypt` directory:
    ```sh
    oc rsync dummy-pod:/etc/letsencrypt/ /home/insert-username-here/
    ```
    *Note:* the target local directory is the last parameter. Modify as appropriate.

    The current certificate can be found inside `live/openshift-route-certs`. The previous one is in `archive/openshift-route-certs` (along with the current one, the certs inside `live/` are symlinks).

4. Delete the pod once you're done:
    ```sh
    oc delete pod dummy-pod
    ```


#### Wiping the PersistentVolumeClaim (PVC)

Generally, wiping and re-creating the PVC from scratch is sufficient to trigger a ACME certificate request the next time it runs.

Backup the existing certificates before proceeding!

To delete the PVC and create a new one:
```sh
export NAMESPACE=xxxxxx-prod
oc delete job -n $NAMESPACE -l app=certbot
oc delete pvc certbot
echo '{"apiVersion":"v1","kind":"PersistentVolumeClaim","metadata":{"annotations":{},"labels":{"app":"certbot","template":"certbot-template"},"name":"certbot","namespace":"'"$NAMESPACE"'"},"spec":{"accessModes":["ReadWriteMany"],"resources":{"requests":{"storage":"64Mi"}},"storageClassName":"netapp-file-standard"}}' | oc create -f -
```

*Note:* all Certbot jobs (even terminated ones) must be deleted before the PVC can be deleted. The `oc delete job` takes care of this.


#### Reinstaling Certbot

If wiping the PVC doesn't work, then the entire Certbot install can be removed, followed by a re-install.

Backup the existing certificates before proceeding!

To wipe the existing Certbot install:

```sh
export NAMESPACE=xxxxxx-prod
oc delete all -n $NAMESPACE -l build=certbot
oc delete cronjob,pvc,rolebinding,sa -n $NAMESPACE -l app=certbot
```

Then, follow the setup instructions as usual.

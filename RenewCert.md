# Renewing the Cert

The following are the current (as of Dec. 2018) steps for renewing the SSL Certs for the OpenShift instance of the app. A user with full admin access to the production project must perform the following steps.  It's likely these steps can be automated, but that has not been done.

## Background

Agri-nmp uses [Let's Encrypt](https://letsencrypt.org/) certificates and [certbot](https://certbot.eff.org) to manage the certificates.  The certificates are free, but must be renewed every 90 days.  The renewal process (once you know it) is relatively easy, but quite manual, with all the risk that entails.

## Install certbot locally

Installation instructions for [certbot](https://certbot.eff.org) can be found here - [https://certbot.eff.org/docs/install.html](https://certbot.eff.org/docs/install.html). 

If you are on Windows (and probably other platforms), it's probably easiest to use the docker instructions, but with different local mount points. For example, where they suggest using the command:

```
docker run -it --rm --name certbot \
            -v "/etc/letsencrypt:/etc/letsencrypt" \
            -v "/var/lib/letsencrypt:/var/lib/letsencrypt" \
            certbot/certbot certonly
```

change the before the colon `/etc/letsencrypt` and `/var/lib/letsencrypt` with references to local directories (e.g. `${PWD}/letsencrypt` and `${PWD}/libletsencrypt`).

If you are on a linux machine, you can just install `certbot` using a package manager (`sudo apt get certbot` on Ubuntu), or on anything, install it to run under python.

## Run certbot to generate the new certificates

On linux, run the following command to generate the certs, responding to the prompts and stopping at the `Press Enter to Continue` step.  **Don't press "Enter" (yet)**.

```
sudo certbot certonly --manual -d nmp.apps.nrs.gov.bc.ca
```

Based on the docs, a `--config_dir <dir>` option can be used to change where the cert files are generated (but that's not been tried).

On other OS's the command might be different - eg. on docker.

The prompts are pretty obvious, with the only tricky one being the need to enter an email address for notifications.

Above the `Press Enter to Continue` prompt, you will see a long string in two parts, with a "." separator.  Copy that string as it will be used on the next step.

## Updating the certbot pod - Proving Control

The copied string needs to be put into an OpenShift Config Map such that it proves that you have control over the URL being renewed.

- go into the OpenShift Agri-NMP Prod project
- go to Resources -> Config Maps
- Click into the `certbot` config map
  - The format of copied string from certbot should be the same as the value of the existing string - e.g same two parts, same length
- Click Actions -> Edit
- Set the Key to the first part of the copied string (up to the ".")
- Set the Value to the entire string
- Click Save

Redeploy the `certbot` deployment (Applications -> Deployments -> certbot -> Deploy)

To verify that the editing worked, go to the URL: https://nmp.apps.nrs.gov.bc.ca/.well-known/acme-challenge/VQq-yfgiqhq3YLFwSsOhhbKW7WNg6VBBe-71WVBML2I (replacing that last section with the first part of the copied string). You should get back in the browser the full copied string.

Once completed, return to the `certbot` command line and hit `Enter` per the instructions.  You should see a `Congratulations` note and the directory where the certs can be found.

## Install the Certs

Go to the directory where the certs were saved and display them such that you can copy the text.  As well, go in the OpenShift Agri-NMP Prod project to Applications -> Routes -> nmp-prod and then select Actions -> Edit.  Scroll down to where the values of the certificates are - there are three (`Certificate`, `Private Key` and `CA Certificate`).

- As a precaution - copy and paste the current values into a text editor in case you need to replace them.
- From the directory where certbot put the certificate files (e.g. /etc/letsencrypt/live/nmp.apps.nrs.gov.bc.ca)
  - Copy the text from file `cert.pem` to the `Certificate` text box
  - Copy the text from file `privkey1.pem` to the `Private Key` text box
  - Copy the text from the file `fullchain1.pem` to the `CA Certificate` text box (note the file contains two keys)
- Click save
- Verify that the app is accessible: [https://nmp.apps.nrs.gov.bc.ca/](https://nmp.apps.nrs.gov.bc.ca/)
- Check the "lock" icon -> "Certificate" that the date of the certificate has been update.

There is likely an OpenShift `oc` command that could be run to automatically copy of the files into the correct place, but we didn't dig into that.

Done!
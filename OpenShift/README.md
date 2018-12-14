# Automated/Assisted Build and Deployment

## Build
```
OpenShift/pipeline-cli build --config=openshift/config.groovy --pr=267
```

## Deployment
```
OpenShift/pipeline-cli deploy --config=openshift/config.groovy --pr=267 --env=dev

```

## Cleanup
```
# adjust '-n csnr-devops-lab-tools` to the correct namespace
oc -n csnr-devops-lab-tools delete dc,rc,svc,route,bc,is,secret,pvc,configmap -l app-name=nmp
```

# Manual Build/Deployment
## Prerequisites
  * Have a Pull-Request

  * Set Project
    ```
    oc project csnr-devops-lab-tools
    ```
## Build
```
oc process -f OpenShift/dotnet-20.bc.json -o json | oc apply -f -

oc process -f OpenShift/dotnet-20-node.bc.json -p NAME_SUFFIX=-build-267 -p VERSION=build-v267 -p SOURCE_GIT_URL=https://github.com/bcgov/agri-nmp.git -p SOURCE_GIT_REF=refs/pull/267/head -o json | oc apply -f -

oc process -f OpenShift/nmp.bc.json -p NAME_SUFFIX=-build-267 -p VERSION=build-v267 -p SOURCE_GIT_URL=https://github.com/bcgov/agri-nmp.git -p SOURCE_GIT_REF=refs/pull/267/head -o json | oc apply -f -
```

## Deployment
```
oc process -f OpenShift/nmp.bc.json -p NAME_SUFFIX=-dev-267 -p VERSION=dev-v267 -p 'HOST=' -o json | oc apply -f -
```

## Cleanup
Same as automated/assisted 'Cleanup' above
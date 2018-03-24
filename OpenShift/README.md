# How to manually load templates

```
oc process -f dotnet-20.bc.json -p 'NAME_SUFFIX=-pr-1' -l 'app=nmp-pr-1,app-name=nmp,env-name=pr-1' | oc apply -f - --dry-run
oc process -f dotnet-20-node.bc.json -p 'NAME_SUFFIX=-pr-1' -l 'app=nmp-pr-1,app-name=nmp,env-name=pr-1' | oc apply -f - --dry-run
oc process -f nmp.bc.json -p 'NAME_SUFFIX=-pr-1' -l 'app=nmp-pr-1,app-name=nmp,env-name=pr-1' | oc apply -f - --dry-run

```

# Deployment
```
oc process -f nmp.dc.json -p 'NAME_SUFFIX=-pr-1' -p 'ENV_NAME=pr-1' -l 'app=nmp-pr-1,app-name=nmp,env-name=pr-1' | oc create -f - --dry-run

oc tag pdf-pr-1:latest pdf-pr-1:pr-1
oc tag nmp-pr-1:latest nmp-pr-1:pr-1
```


```
oc delete dc -l app-name=nmp; oc delete svc -l app-name=nmp ; oc delete route -l app-name=nmp

oc delete all -l app-name=nmp
```
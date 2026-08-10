## Applying templates

### BuildConfigs

BuildConfigs should be applied in 624dc1-tools. Run `oc apply -f [file]` in the terminal. These yaml files create both the BuildConfig and the ImageStream they output to.

### Deployments

Deployments are applied to the dev, test, and prod environment projects. The yaml files create the Deployment, a PersistentVolumeClaim if it uses one, its Service, and, for hosted services, the Route. 

The nmp-web yaml is a template and can be created by running `oc process -f nmp-web-deployment.yaml`. It is configured for the dev environment. To apply it to a different environment, set the parameters with the flag `-p [name]=[value]` after the file name. The other Deployment files are not templates and use the same command as the BuildConfig files.
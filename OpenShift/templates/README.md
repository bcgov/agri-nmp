AGRI-NMP
======================

OpenShift Configuration and Deployment
----------------

The agri-nmp-tools (Tools) project contains the Build Configurations (bc) and Image Streams (is) that are referenced by the Deployment Configurations.

The following projects contain the Deployment Configurations (dc) for the various types of deployments:
- agri-nmp-dev (Development)
- agri-nmp-test (Test)
- agri-nmp-prod (Production)
 
In AGRI-NMP there are 2 components that are deployed 
- .NET Core API Server 
- .NET Core PDF microservice


Steps to configure the deployment:
----------------------------------

Ensure that you have access to the build and deployment templates.

These can be found in the openshift/templates folder of the project repository.

If you are setting up a local OpenShift cluster for development, fork the main project.  You'll be using your own fork.

Connect to the OpenShift server using the CLI; either your local instance or the production instance. 
If you have not connected to the production instance before, use the web interface as you will need to login though your GitHub account.  Once you login you'll be able to get the token you'll need to login to you project(s) through the CLI from here; Token Request Page.  The CLI will also give you a URL to go to if you attempt a login to the OpenShift server without a token.

The same basic procedure, minus the GitHub part, can be used to connect to your local instance.

Login to the server using the oc command from the page.
Switch to the Tools project by running:
`oc project agri-nmp-tools`

`oc process -f https://raw.githubusercontent.com/bcgov/agri-nmp/master/OpenShift/templates/build-template.json | oc create -f -`

This will produce several builds and image streams.

 You can now login to the web interface and observe the progress of the initial build.
Once the initial build is done, create builds with tags "dev", "test", and "prod" as required for deployment.  The deployment configurations will use these tags to determine which image to load.
You can edit a build to change the tag.

Once you have images tagged for dev, test or prod you are ready to deploy.
Open a command prompt and login as above to OpenShift
Change to the project for the type of deployment you are configuring.  For example, to configure a dev deployment, switch to agri-nmp-dev

In the command prompt, type
`oc project agri-nmp-dev`
By default projects do not have permission to access images from other projects.  You will need to grant that.
Run the following:
`oc policy add-role-to-user system:image-puller system:serviceaccount:<project_identifier>:default -n <project namespace where project_identifier needs access>`

EXAMPLE - to allow the production project access to the images, run:

`oc policy add-role-to-user system:image-puller system:serviceaccount:agri-nmp-prod:default -n agri-nmp-tools`

- Process and create the Environment Template
- `oc process -f deployment-template.json  -p APP_DEPLOYMENT_TAG=<DEPLOYMENT TAG> | oc create -f -`
	- Substitute latest (dev enviornment), test (test enviornment) or prod (prod enviornment) for the <DEPLOYMENT TAG>


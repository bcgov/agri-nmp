# BC Ministry of Agriculture Nutrition Management Program

## Contribution

Please report any [issues](https://github.com/bcgov/agri-nmp/issues).

[Pull requests](https://github.com/bcgov/agri-nmp/pulls) are always welcome.

If you would like to contribute, please see our [contributing](CONTRIBUTING.md) guidelines.

Please note that this project is released with a [Contributor Code of Conduct](CODE_OF_CONDUCT.md). By participating in this project you agree to abide by its terms.

## Development

This project uses .NET Core version 3.1 and PostgreSQL.

You will need Visual Studio, which supports both Mac and Windows.

### Running NMP locally

1. Download and install [Visual Studio](https://visualstudio.microsoft.com/) (not Visual Studio Code). When installing, make sure you also select .NET or ASP .NET when prompted (menu options may vary depending on OS).
2. Install PostgreSQL and create the local database. NMP will auto-populate it with static values on its first run:
    ```sh
    brew install postgresql
    brew services run postgresql
    psql postgres
    create user nmp with encrypted password 'nmp';
    create database nmp owner nmp;
    ```
3. Clone the repository.
4. Create a file called `secrets.json` in `src/SERVERAPI` containing the following:
    ```json
    {
        "Agri:ConnectionString": "Server=localhost;Database=nmp;Username=nmp;Password=nmp"
    }
    ```
3. Open `agri-nmp/app/server/Server.sln` in Visual Studio, and wait for NuGet to download all the dependencies.
4. Run SERVERAPI from inside Visual Studio. On Visual Studio for Mac,  this is the "play" icon in the top-left corner.


Once running, the application can be accessed at http://localhost:8080.

Don't forget to shut down the database after quitting:
```sh
brew services stop postgresql
```

#### Soil values

By default, if the database is empty, NMP will auto-populate it with static values.

To update the values in the local database, use `pg_dump` to grab data from PROD.

## CI/CD pipeline

As all three NMP projects share the same namespace on OpenShift, they share a similar deployment process.

Image builds are automatically triggered via GitHub webooks, whenever there is a push or PR merge to the main branch. A Tekton pipeline then performs image promotion, auto-deploying the build to DEV. 

Deploying to TEST and PROD are done by manually starting the `promote-test-nmp-web` and `promote-prod-nmp-web` pipelines respectively. This can be done in the OpenShift web conosle, under **Pipelines** > **Pipelines** in the tools namespace, and clicking "Start" for the respective pipeline.

To rollback PROD to the previous build, run the `undo-last-promote-prod-nmp-web` pipeline.

## Updates to code values without developer assistance

This project allows non-technical users (i.e. Product Owners) with business domain knowledge, to revise and update static code table values, message text, etc.  See the [instructions](app/Server/src/SERVERAPI/Data/README.md) for details.

## Static Code Analysis (obsolete)

Steps to conduct static code analysis:
1) Install the Visual Studio 2017 Community Edition plus standalone build tools, such that you are able to compile the source for the application.
2) Login to http://sonarqube-agri-nmp-tools.pathfinder.gov.bc.ca
3) Aquire a token by going to My Account -> Security Tab
4) Change to the folder containing the .SLN file (the Server directory)
5) Edit the sonar.bat file in that folder, changing the token to match the value above.
6) Run sonar.bat on a Windows computer to execute the scan and upload the stats.


### Adding a pull secret to the OpenShift project, and import Dotnet builder image

RedHat requires authentication to the image repository where the Dotnet images are stored.  Follow these steps to enable this:

1)  Sign on with a developer account to https://registry.redhat.io.  Developer accounts are free as of October 2019.

2) Go to the Service Accounts section of the website, which as of October 2019 was in the top right of the web page.

3) Add a service account if one does not exist.

4) Once you have a service account, click on it and select the OpenShift Secret tab.

5) Click on "view its contents" at the Step 1: Download secret section.  

6) Copy the contents of the secret 

7) Import the secret into OpenShift.  Note that you will likely need to edit the name of the secret to match naming conventions.

8) In a command line with an active connection to OpenShift, and the current project set to the Tools project, run the following commands:

    `oc secrets link default <SECRETNAME> --for=pull`  
    `oc secrets add serviceaccount/builder secrets/<SECRETNAME>`

    Where `<SECRETNAME>` is the name you specified in step 7 when you imported the secret.

9) You can now import images from the Redhat repository.  For example:

    `oc import-image dotnet/dotnet-31-rhel7 --from=registry.redhat.io/dotnet/dotnet-31-rhel7 --confirm` 

10) Adjust your builds to use this imported image

*Note:* For the Agri project, the pull secret is in the agri-nmp-tools namespace. It can't be shared with any other namespace. This is why you will see redis image stream being created in the agri-nmp-tools namespace and then this namespace being referenced in the deployment scripts which will run in other namespaces.


# REDIS Code (currently commented out)

As of Release 3 Redis containers were added, but due to some technical issues the application couldn't properly use them for session data. At this moment the code for Redis has been commented out in the application and in the pipelines. Product Owners agreed to run the app as a single container for now.

# NMP application pod - Single Container

Originally this application was running multiple containers and one of the loadbalancing change to the infrastructure broke this app. It had to do with the stickyness of sessions. As a best practice, application shouldn't rely on loadbalancer to provide sticky session function, rather it should manage its state externally. For this reason Redis was setup, but due to some outstanding issues, this was put on hold and currently the app is running as a single container.

Will need to put this in the NMP pod, to resurrect Redis:
```json
{
  "name": "REDIS_PASSWORD",
  "valueFrom": {
    "secretKeyRef": {
      "name": "${NAME}-redis-credentials${SUFFIX}",
      "key": "password"
    }
  }
}
```

# Postgres - Single Container
Due to the non-critical nature of this application, Postgres currently runs as a single container. In the future, it can made HA using Patroni (used by other projects in BC Gov. Openshift platform).

# License

    Copyright 2017 Province of British Columbia

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at 

       http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.

BC Ministry of Agriculture Nutrition Management Program
-----------------

Product Owner
--------
David Poon, David.Poon@gov.bc.ca


Contribution
------------

Please report any [issues](https://github.com/bcgov/agri-nmp/issues).

[Pull requests](https://github.com/bcgov/agri-nmp/pulls) are always welcome.

If you would like to contribute, please see our [contributing](CONTRIBUTING.md) guidelines.

Please note that this project is released with a [Contributor Code of Conduct](CODE_OF_CONDUCT.md). By participating in this project you agree to abide by its terms.

Development
-----------
This project uses .NET Core version 2, which is currently in Preview (3).

You will need to install Visual Studio 2017 Preview 3 in order to effectively develop the application from a Windows PC.

Note that .NET Core is cross platform, so you can also use a Mac or Linux computer equipped with the appropriate build tools.  

Updates to code values without developer assistance
---------------------------------------------
This project allows non-technical users (i.e. Product Owners) with business domain knowledge, to revise and update static code table values, message text, etc.  See the [instructions](app/Server/src/SERVERAPI/Data/README.md) for details.

Static Code Analysis
--------------------

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

Note: For the Agri project, the pull secret is in the agri-nmp-tools namespace. It can't be shared with any other namespace. This is why you will see redis image stream being created in the agri-nmp-tools namespace and then this namespace being referenced in the deployment scripts which will run in other namespaces.


License
-------

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

Maintenance
-----------

This repository is maintained by BC Ministry of Agriculture

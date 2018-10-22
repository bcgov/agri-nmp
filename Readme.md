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

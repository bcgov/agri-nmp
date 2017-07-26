@echo off
echo Running SonarQube Static Analysis

rem This script requires that the following prerequisites be done:
rem 1. Visual Studio 2017 build tools are installed
rem 2. The project be converted to use .NET Core 1.1.2 or newer (must use .csproj for the project file rather than project.json)
rem 3. The 1.0.4 .NET Core SDK is installed.  If you install a more recent SDK, update this script to reference that newer version.
rem 4. SonarQube MSBuild Scanner 3.0.1 or newer is installed
rem     Download the release from https://github.com/SonarSource/sonar-scanner-msbuild/releases and put it on the path.

rem Workaround for issue found 2017/07/25 in that MSBuild cannot find the sdk.
set MSBuildSDKsPath=C:\Program Files\dotnet\sdk\2.0.0-preview2-006497\Sdks


for /f %%i in ('git rev-parse HEAD') do set COMMIT=%%i

rem dotnet restore project.sonar.json
SonarQube.Scanner.MSBuild.exe /d:sonar.host.url=http://sonarqube-agri-nmp-tools.pathfinder.gov.bc.ca /n:"Nutrient Management Project" /v:%COMMIT%  /d:sonar.login=<TOKEN> begin /k:"agri-nmp"
"C:\Program Files (x86)\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin\MSBuild" /t:Rebuild 

SonarQube.Scanner.MSBuild.exe end




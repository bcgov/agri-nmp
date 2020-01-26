#  ASP.NET Core 2.0 API Server

Starter App 

## Run

Linux/OS X/windows:

```
dotnet run
```
 
# Configuration Data 


### Database via Entity Framework


#### On local Dev environment
To drop the database locally in development the below setting to true
```
"RefreshDatabase": true
```
in appsettings.json.

To just reload seeddata set the below setting to true
```
"LoadSeedData": true
```


#### On OpenShift environment
There is only the option to reload the Seed Data, which is meant to manage the release of Data Structure changes and new tables or columns, which is relfected with updated data.

To trigger a reload of the seed data in OpenShift environment set the enviornment variable

`LOAD_SEED_DATA`

To true, which will truncate all non versioned data prior to loading and add a new version of configuration data using 'StaticDataVersion_#.json' file. 
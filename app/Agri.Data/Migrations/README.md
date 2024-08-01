# NMP Migrations

## Running Migrations

Make sure the EntityFramework command line tool is installed:
    run ```dotnet tool install --global dotnet-ef --version 7.0.2```

**\* all commands listed below must be run from /app \***

to create a migration:
   run ```dotnet ef migrations add <NameOfMigration> -p ./Agri.data -s ./Server/src/SERVERAPI```

to run all unapplied migrations:
   run ```dotnet ef database update -p ./Agri.data -s ./Server/src/SERVERAPI```

to run up to a specific migration:
   run ```dotnet ef database update <NameOfMigration> -p ./Agri.data -s ./Server/src/SERVERAPI```
    *\*Note\** : To UNDO a migration, choose the last good migration to run to. This will NOT undo changes made to the database

to check what migrations have already been run:
   run ```dotnet ef migrations list --project ./Server/src/SERVERAPI/SERVERAPI.csproj```
   You can also directly look at the __EFMigrationsHistory table in the db to see a record of all applied migrations

to run these actions when the database is hosted in Openshift (dev,test,prod):

   1. Stop running your local database by running ```brew services stop postgresql```
   2. Start port-forwarding the remote database by running ```oc port-forward <name-of-remote-postgresql-pod> 5432:```
      *\*Note\** : Make sure you are logged into Openshift CLI and that your project is set to the namespace of the pod you are accessing
   3. Update the connection string in your secrets.json file using the database name, username, and password found in the Openshift secrets called nmp-postgresql-credentials inside the appropriate namespace

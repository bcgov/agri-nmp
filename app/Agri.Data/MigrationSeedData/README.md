#Migration Seed Data

Each json file in this folder is for a specific modification or update related to a user storey.  These files are used by AgriSeeder.Seed method to change data in the AgriConfiguration database.

The application of the migration seed data will be tracked in the MigrationSeedData table and the code usage in the seed method should add the application of the data.  The same code will ensure the data hasn't already been applied by checking this table first.

**Adding new files**
1. When adding a new json file give it a name with next numeric prefix
2. The json needs to match the structure of the Agri.Models.Data.MigrationSeedData without the date
3. It is important the **Id** field is the next incremental value, the **JsonFileName** is the name of the file and the **ReasonDescription** has either the User Story or the reason for the change
4. The **Data** field can be a single Model object or a collection of objects, but ensure the json is valid for either.
5. Ensure that the file **Build Action** property for the json file is set to be an "Embedded resource"

**Usage of New Files**

Below is an example for the AgriSeeder on how to use the new files, which the immediate example is for adding a new record, but you can use the Data how ever is required, insert, delete to replace with an update or even drop a record.  The key note is to ensure code is added to check AppliedMigrationSeedData for the application of the change and when applied a record is added to AppliedMigrationSeedData after a change.
    

            if (!_context.AppliedMigrationSeedData.Any(a => a.JsonFilename.Equals("1_UserPrompts", StringComparison.CurrentCultureIgnoreCase)))
            {
                var migrationSeedData = SeedDataLoader.GetMigrationSeedData<List<UserPrompt>>("1_UserPrompts");
                foreach (var newUserPrompt in migrationSeedData.Data)
                {
                    if (!_context.UserPrompts.Any(up => up.Id == newUserPrompt.Id))
                    {
                        _context.UserPrompts.Add(newUserPrompt);
                    }
                }
                _context.AppliedMigrationSeedData.Add(migrationSeedData);
            }

Also above shows the SeedDataLoader.GetMigrationSeedData requires the Type to be serialized from the **Data** field of the json.

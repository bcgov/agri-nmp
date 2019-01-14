using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using Agri.Models.Data;
using Newtonsoft.Json;

namespace Agri.Data
{
    public class SeedDataLoader
    {
        public static MigrationSeedData<T> GetMigrationSeedData<T>(string seedDataFileName) 
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                using (var stream = assembly.GetManifestResourceStream($"Agri.Data.MigrationSeedData.{seedDataFileName}.json"))
                using (var reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    var result = JsonConvert.DeserializeObject<MigrationSeedData<T>>(json);
                    result.AppliedDateTime = DateTime.Now;
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"SeedDataUpdates json file structure error!{Environment.NewLine}{ex.Message}");
            }
        }
    }
}

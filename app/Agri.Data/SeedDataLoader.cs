using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace Agri.Data
{
    public class SeedDataLoader
    {
        public static T GetStaticDataJson<T>(string seedDataFileName) 
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                using (var stream = assembly.GetManifestResourceStream($"Agri.Data.SeedData.{seedDataFileName}.json"))
                using (var reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<T>(json);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"SeedDataUpdates json file structure error!{Environment.NewLine}{ex.Message}");
            }
        }
    }
}

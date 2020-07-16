using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using Agri.Models.Data;
using Newtonsoft.Json;
using Agri.Models.Configuration;

namespace Agri.Data
{
    public class SeedDataLoader
    {
        public static T GetSeedJsonData<T>(string seedDataFileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(seedDataFileName))
            using (var reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();
                var result = JsonConvert.DeserializeObject<T>(json);
                return result;
            }
        }
    }
}
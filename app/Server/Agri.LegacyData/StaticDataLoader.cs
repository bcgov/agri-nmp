using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Agri.LegacyData
{
    public static class StaticDataLoader
    {
        public static JObject GetStaticDataJson()
        {
            var resourceStream = Assembly.Load("Agri.LegacyData").GetManifestResourceStream("Agri.LegacyData.Data.static.json");

            try
            {
                using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
                {
                    return JObject.Parse(reader.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Static Data json file structure error!");
            }
        }
    }
}

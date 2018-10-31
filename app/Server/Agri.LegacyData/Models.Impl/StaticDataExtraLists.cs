using System;
using System.Collections.Generic;
using Agri.Models.StaticData;
using Newtonsoft.Json.Linq;

namespace Agri.LegacyData.Models.Impl
{
    public class StaticDataExtraLists : StaticData
    {
        public List<AmmoniaRetention> GetAmmoniaRetentions()
        {
            JArray array = (JArray)rss["agri"]["nmp"]["ammoniaretentions"]["ammoniaretention"];
            var ammoniaRetentions = new List<AmmoniaRetention>(); 

            foreach (var r in array)
            {
                var ammoniaRetention = new AmmoniaRetention
                {
                    SeasonApplicationId = Convert.ToInt32(r["seasonapplicatonid"].ToString()),
                    DM = Convert.ToInt32(r["dm"].ToString()),
                    Value = r["value"].ToString() == "null"
                            ? (decimal?)null
                            : Convert.ToDecimal(r["value"].ToString())
                };
                ammoniaRetentions.Add(ammoniaRetention);
            }

            return ammoniaRetentions;
        }
    }
}

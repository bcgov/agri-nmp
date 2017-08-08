using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERVERAPI.Models
{
    public class StaticData
    {
        public class Regions
        {
            public List<Region> regions { get; set; }
        }
        public class Region
        {
            public int id { get; set; }
            public string name { get; set; }
            public string location { get; set; }
            public int p_regioncd { get; set; }
            public int k_regioncd { get; set; }
        }
        public class SelectListItem
        {
            public int Id { get; set; }
            public string Value { get; set; }
        }
        public class SelectCodeItem
        {
            public string Cd { get; set; }
            public string Value { get; set; }
        }

    }
}

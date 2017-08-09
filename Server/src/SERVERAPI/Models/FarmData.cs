using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SERVERAPI.Models
{
    public class FarmData
    {
        public string year { get; set; }
        public string farmName { get; set; }
        public int? farmRegion { get; set; }
        public bool? soilTests { get; set; }
        public bool? manure { get; set; }
        public List<YearData> years { get; set; }
    }
    public class YearData
    {
        public string year { get; set; }
        public List<Field> fields { get; set; }
    }
    public class Field
    {
        public string fieldName { get; set; }
        public decimal area { get; set; }
        public string comment { get; set; }
    }
}


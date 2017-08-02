using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SERVERAPI.Models
{
    public class FarmData
    {
        public string farmName { get; set; }
        public bool? soilTests { get; set; }
        public bool? manure { get; set; }
        public List<YearData> Years { get; set; }
    }
    public class YearData
    {
        public string Year { get; set; }
        public List<Field> Fields { get; set; }
    }
    public class Field
    {
        public string FieldName { get; set; }
        public int Area { get; set; }
        public string Comment { get; set; }
    }
}


using System.Collections.Generic;

namespace Agri.Models.Farm
{
    public class Field
    {
        public int id { get; set; }
        public string fieldName { get; set; }
        public decimal area { get; set; }
        public string comment { get; set; }
        public Nutrients nutrients { get; set; }
        public List<FieldCrop> crops {get; set; }
        public SoilTest soilTest { get; set; }   
        public string prevYearManureApplicationFrequency { get; set; }  
        public int? prevYearManureApplicationNitrogenCredit { get; set; } 
        public decimal? SoilTestNitrateOverrideNitrogenCredit { get; set; }
    }
}
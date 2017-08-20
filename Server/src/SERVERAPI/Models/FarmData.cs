using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SERVERAPI.Models
{
    public class FarmData
    {
        public FarmDetails farmDetails { get; set; }
        public List<YearData> years { get; set; }
    }
    public class FarmDetails
    {
        public string year { get; set; }
        public string farmName { get; set; }
        public int? farmRegion { get; set; }
        public bool? soilTests { get; set; }
        public bool? manure { get; set; }
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
        public Nutrients nutrients { get; set; }
    }
    public class Nutrients
    {
        public List<NutrientManure> nutrientManures { get; set; }
        public List<NutrientFertilizer> nutrientFertilizers { get; set; }
        public List<NutrientOther> nutrientOthers { get; set; }
    }
    public class NutrientManure
    {
        public int id { get; set; }
        public string manureId { get; set; }
        public string applicationId { get; set; }
        public string unitId { get; set; }
        public decimal rate { get; set; }
        public decimal nh4Retention { get; set; }
        public decimal nAvail { get; set; }
        public decimal yrN { get; set; }
        public decimal yrP2o5 { get; set; }
        public decimal yrK2o { get; set; }
        public decimal ltN { get; set; }
        public decimal ltP2o5 { get; set; }
        public decimal ltK2o { get; set; }
    }
    public class NutrientFertilizer
    {
        public string id { get; set; }
    }
    public class NutrientOther
    {
        public string id { get; set; }
    }
}


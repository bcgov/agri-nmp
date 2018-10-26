﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SERVERAPI.Models
{
    public class FarmData
    {
        public FarmDetails farmDetails { get; set; }
        public bool unsaved { get; set; }
        public List<YearData> years { get; set; }
    }
    public class FarmDetails
    {
        public string year { get; set; }
        public string farmName { get; set; }
        public int? farmRegion { get; set; }
        public bool? soilTests { get; set; }
        public string testingMethod { get; set; }
        public bool? manure { get; set; }
    }
    public class YearData
    {
        public string year { get; set; }
        public List<Field> fields { get; set; }
        public List<FarmManure> farmManures { get; set; }
        public List<GeneratedManure> GeneratedManures { get; set; }
    }
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
    public class Nutrients
    {
        public List<NutrientManure> nutrientManures { get; set; }
        public List<NutrientFertilizer> nutrientFertilizers { get; set; }
        public List<NutrientOther> nutrientOthers { get; set; }
    }
    public class NutrientManure
    {
        public int id { get; set; }
        public bool custom { get; set; }
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
        public int id { get; set; }
        public int fertilizerTypeId { get; set; }
        public int fertilizerId { get; set; }
        public int applUnitId { get; set; }
        public decimal applRate { get; set; }
        public DateTime? applDate { get; set; }
        public int applMethodId { get; set; }
        public decimal? customN { get; set; }
        public decimal? customP2o5 { get; set; }
        public decimal? customK2o { get; set; }
        public decimal fertN { get; set; }
        public decimal fertP2o5 { get; set; }
        public decimal fertK2o { get; set; }
        public decimal liquidDensity { get; set; }
        public int liquidDensityUnitId { get; set; }
    }
    public class NutrientOther
    {
        public int id { get; set; }
        public string description { get; set; }
        public decimal yrN { get; set; }
        public decimal yrP2o5 { get; set; }
        public decimal yrK { get; set; }
        public decimal ltN { get; set; }
        public decimal ltP2o5 { get; set; }
        public decimal ltK { get; set; }
    }
    public class FieldCrop
    {
        public int id { get; set; }
        public string cropId { get; set; }
        public string cropOther { get; set; }
        public decimal yield { get; set; } // tons/acre
        public decimal reqN { get; set; }
        public decimal stdN { get; set; }
        public decimal reqP2o5 { get; set; }
        public decimal reqK2o { get; set; }
        public decimal remN { get; set; }
        public decimal remP2o5 { get; set; }
        public decimal remK2o { get; set; }
        public decimal? crudeProtien { get; set; }
        public int prevCropId { get; set; }
        public bool? coverCropHarvested { get; set; }
        public int prevYearManureAppl_volCatCd { get; set; }
        public int? yieldHarvestUnit { get; set; }
        public decimal yieldByHarvestUnit { get; set; }
    }
    public class SoilTest
    {
        public DateTime sampleDate { get; set; }
        public decimal valNO3H { get; set; }
        public decimal ValP { get; set; }
        public decimal valK { get; set; }
        public decimal valPH { get; set; }
        public int ConvertedKelownaP { get; set; }
        public int ConvertedKelownaK { get; set; }
    }
    public class FarmManure
    {
        public int id { get; set; }
        public bool customized { get; set; }
        public int manureId { get; set; }
        public string name { get; set; }
        public string manure_class { get; set; }
        public string solid_liquid { get; set; }
        public string moisture { get; set; }
        public decimal nitrogen { get; set; }
        public int ammonia { get; set; }
        public decimal phosphorous { get; set; }
        public decimal potassium { get; set; }
        public int dmid { get; set; }
        public int nminerizationid { get; set; }
        public decimal? nitrate { get; set; }
    }

    public class GeneratedManure
    {
        public int id { get; set; }
        public int animalId { get; set; }
        public int animalSubTypeId { get; set; }
        public int averageAnimalNumber { get; set; }
        public StaticData.ManureMaterialType manureType { get; set; }
        public decimal washWaterGallons { get; set; }
    }
}


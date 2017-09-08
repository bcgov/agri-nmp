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
            public int soil_test_phospherous_region_cd { get; set; }
            public int soil_test_potassium_region_cd { get; set; }
            public int locationid { get; set; }
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

        public class Locations
        {
            public List<Location> locations { get; set; }
        }

        public class Location
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public class Manures
        {
            public List<Manure> manures { get; set; }
        }

        public class Manure
        {
            public int id { get; set; }
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
        }

        public class Season_Applications
        {
            public List<Season_Application> season_applications { get; set; }
        }

        public class Season_Application
        {
            public int id { get; set; }
            public string name { get; set; }
            public string season { get; set; }
            public string application_method { get; set; }
            public string moisture { get; set; }
            public decimal dm_lt1 { get; set; }
            public decimal dm_1_5 { get; set; }
            public decimal dm_5_10 { get; set; }
            public decimal dm_gt10 { get; set; }            
            public string poultry_solid { get; set; }
            public string compost { get; set; }
        }

        public class Units
        {
            public List<Unit> units { get; set; }
        }

        public class Unit
        {
            public int id { get; set; }
            public string name { get; set; }
            public string nutrient_content_units { get; set; }
            public decimal conversion_lbton { get; set; }
            public string nutrient_rate_units { get; set; }
            public string cost_units { get; set; }
            public decimal cost_applications { get; set; }
            public string dollar_unit_area { get; set; }
            public string value_material_units { get; set; }
            public decimal value_N { get; set; }
            public decimal value_P2O5 { get; set; }
            public decimal value_K2O { get; set; }
            public string solid_liquid { get; set; }
        }

        public class CropTypes
        {
            public List<CropType> cropTypes { get; set; }
        }

        public class CropType
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public class Crops
        {
            public List<Crop> crops { get; set; }
        }

        public class Crop
        {
            public int id { get; set; }
            public string cropname { get; set; }
            public int croptypeid { get; set; }
            public int yieldcd { get; set; }
            public decimal? cropremovalfactor { get; set; }
            public decimal? value_P2O5 { get; set; }
            public decimal? value_KO5 { get; set; }
            public decimal n_recommcd { get; set; }
            public decimal? n_recomm_lbperac { get; set; }
            public decimal? n_high_lbperac { get; set; }
            public int prevcropcd { get; set; }
        }

        public class Yield
        {
            public int id { get; set; }
            public string yielddesc { get; set; }
        }

      public class Crop_STP_RegionCd
        {
            public int cropid { get; set; }
            public int regionid { get; set; }
            public int regioncd { get; set; }
        }

        public class Crop_STK_RegionCd
        {
            public int cropid { get; set; }
            public int regionid { get; set; }
            public int regioncd { get; set; }
        }

        public class DM
        {
            public int ID { get; set; }
            public string name { get; set; }            
        }

        public class AmmoniaRetention
        {
            public int seasonapplicatonid { get; set; }
            public int dm { get; set; }
            public decimal value { get; set; }
        }

        public class NMineralization
        {
            public int id { get; set; }
            public string name { get; set; }
            public int locationid { get; set; }
            public decimal firstyearvalue { get; set; }
            public decimal longtermvalue { get; set; }
        }

        public class SoilTestMethods
        {
            public List<SoilTestMethod> methods { get; set; }
        }
        public class SoilTestMethod
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public class PrevCropTypes
        {
            public List<PrevCropType> prevCropTypes { get; set; }
        }
        public class PrevCropType
        {
            public int id { get; set; }
            public int cropType { get; set; }
            public string name { get; set; }
            public int nCreditMetric { get; set; }
            public int nCreditImperial { get; set; }
        }
    }
}

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
            public string location { get; set; }
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
            public List<Unit> unit { get; set; }
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
    }
}

namespace Agri.Models
{
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

        // conversion factor to the units displayed in the section, Manure and Fertilizer Required, of the Complete Report.
        // does not consider the area 
        public decimal farm_reqd_nutrients_std_units_conversion { get; set; }
        public decimal farm_reqd_nutrients_std_units_area_conversion { get; set; }
        public string solid_liquid { get; set; }
    }
}
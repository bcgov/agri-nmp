namespace Agri.Models
{
    public class FertilizerUnit
    {
        public int id { get; set; }
        public string name { get; set; }
        public string dry_liquid { get; set; }

        public decimal conv_to_impgalperac { get; set; }

        // conversion factor to the units displayed in the section, Manure and Fertilizer Required, of the Complete Report.
        // does not consider the area 
        public decimal farm_reqd_nutrients_std_units_conversion { get; set; }
        public decimal farm_reqd_nutrients_std_units_area_conversion { get; set; }
    }
}
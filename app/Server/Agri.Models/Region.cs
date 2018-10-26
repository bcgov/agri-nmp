namespace Agri.Models
{
    public class Region
    {
        public int id { get; set; }
        public string name { get; set; }
        public int soil_test_phospherous_region_cd { get; set; }
        public int soil_test_potassium_region_cd { get; set; }
        public int locationid { get; set; }
        public int sortNum { get; set; }
    }
}
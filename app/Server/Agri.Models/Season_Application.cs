namespace Agri.Models
{
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
        public int sortNum { get; set; }
        public string manure_type { get; set; }
    }
}
namespace Agri.Models
{
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
        public int sortNum { get; set; }
        public decimal cubic_Yard_Conversion { get; set; }
        public decimal nitrate { get; set; }
    }
}
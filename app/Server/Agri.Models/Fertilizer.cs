namespace Agri.Models
{
    public class Fertilizer
    {
        public int id { get; set; }
        public string name { get; set; }
        public string dry_liquid { get; set; }
        public decimal nitrogen { get; set; }
        public decimal phosphorous { get; set; }
        public decimal potassium { get; set; }
        public int sortNum { get; set; }
    }
}
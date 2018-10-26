namespace Agri.Models
{
    public class SeasonApplication
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Season { get; set; }
        public string ApplicationMethod { get; set; }
        public string Moisture { get; set; }
        public decimal DM_lt1 { get; set; }
        public decimal DM_1_5 { get; set; }
        public decimal DM_5_10 { get; set; }
        public decimal DM_gt10 { get; set; }
        public string PoultrySolid { get; set; }
        public string Compost { get; set; }
        public int SortNum { get; set; }
        public string ManureType { get; set; }
    }
}
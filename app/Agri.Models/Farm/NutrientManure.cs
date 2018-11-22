namespace Agri.Models.Farm
{
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
}
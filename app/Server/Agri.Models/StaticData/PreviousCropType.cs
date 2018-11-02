namespace Agri.Models.StaticData
{
    public class PreviousCropType
    {
        public int Id { get; set; }
        public int PreviousCropCode { get; set; }
        public string Name { get; set; }
        public int NitrogenCreditMetric { get; set; }  //Not being used
        public int NitrogenCreditImperial { get; set; }
    }
}
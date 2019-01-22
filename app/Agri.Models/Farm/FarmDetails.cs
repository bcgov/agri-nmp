namespace Agri.Models.Farm
{
    public class FarmDetails
    {
        public string year { get; set; }
        public string farmName { get; set; }
        public int? farmRegion { get; set; }
        public int? farmSubRegion { get; set; }
        public bool? soilTests { get; set; }
        public string testingMethod { get; set; }
        public bool? manure { get; set; }
    }
}
namespace Agri.Models.Configuration
{
    public class SubRegion
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AnnualPrecipitation { get; set; }
        public int AnnualPrecipitationOctToMar { get; set; }
        public int RegionId { get; set; }
        public Region Region { get; set; }
    }
}
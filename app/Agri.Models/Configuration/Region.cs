using System.Collections.Generic;

namespace Agri.Models.Configuration
{
    public class Region
    {
        public Region()
        {
            SubRegions = new List<SubRegion>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int SoilTestPhosphorousRegionCd { get; set; }
        public int SoilTestPotassiumRegionCd { get; set; }
        public int LocationId { get; set; }
        public int SortNumber { get; set; }
        public List<SubRegion> SubRegions { get; set; }

    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class Location
    {
        public Location()
        {
            CropYields = new List<CropYield>();
            NitrogenMineralizations = new List<NitrogenMineralization>();
            Regions = new List<Region>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public List<CropYield> CropYields { get; set; }
        public List<NitrogenMineralization> NitrogenMineralizations { get; set; }
        public List<Region> Regions { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace Agri.Models.StaticData
{
    public class Location
    {
        public Location()
        {
            CropYields = new List<CropYield>();
            Regions = new List<Region>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public List<CropYield> CropYields { get; set; }
        public List<Region> Regions { get; set; }
    }
}
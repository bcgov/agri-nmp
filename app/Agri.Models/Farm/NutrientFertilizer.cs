using System;

namespace Agri.Models.Farm
{
    public class NutrientFertilizer
    {
        public int id { get; set; }
        public int fertilizerTypeId { get; set; }
        public int fertilizerId { get; set; }
        public int applUnitId { get; set; }
        public decimal applRate { get; set; }
        public DateTime? applDate { get; set; }
        public int applMethodId { get; set; }
        public decimal? customN { get; set; }
        public decimal? customP2o5 { get; set; }
        public decimal? customK2o { get; set; }
        public decimal fertN { get; set; }
        public decimal fertP2o5 { get; set; }
        public decimal fertK2o { get; set; }
        public decimal liquidDensity { get; set; }
        public int liquidDensityUnitId { get; set; }
        public int eventsPerSeason { get; set; }
        public bool isFertigation { get; set; }
    }
}
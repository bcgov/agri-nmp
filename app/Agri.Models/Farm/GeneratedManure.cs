using Agri.Models.Configuration;
using System;

namespace Agri.Models.Farm
{
    public class GeneratedManure : ManagedManure
    {
        public int animalId { get; set; }
        public string animalName { get; set; }
        public int animalSubTypeId { get; set; }
        public string animalSubTypeName { get; set; }
        public int averageAnimalNumber { get; set; }
        public string manureTypeName { get; set; }
        public string annualAmount { get; set; }
        public string washWaterGallonsToString => string.Format("{0:#,##0}", washWaterGallons);
        public decimal washWaterGallons => Math.Round(Convert.ToDecimal(washWater) * Convert.ToInt32(averageAnimalNumber) * 365);
        public decimal washWater { get; set; }
        public decimal milkProduction { get; set; }
        public decimal? solidPerGalPerAnimalPerDay { get; set; }
        public override string ManureId => $"Generated{Id ?? 0}";
        public override string ManagedManureName => animalSubTypeName;
    }
}
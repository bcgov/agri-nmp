using Agri.Models.Configuration;
using System;
using Agri.Models;

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
        public decimal annualAmountDecimal => annualAmount != null ? Convert.ToDecimal(annualAmount.Split(' ')[0]) : 0M;
        public string washWaterGallonsToString => string.Format("{0:#,##0}", washWaterGallons);
        public decimal washWaterGallons
        {
            get
            {
                var washWaterGallons = 0m;
                if (washWaterUnits == WashWaterUnits.USGallonsPerDayPerAnimal)
                {
                    washWaterGallons= Math.Round(Convert.ToDecimal(washWater) * Convert.ToInt32(averageAnimalNumber) * 365);
                }
                else if (washWaterUnits == WashWaterUnits.USGallonsPerDay)
                {
                    washWaterGallons = Math.Round(Convert.ToDecimal(washWater) * 365);
                }

                return washWaterGallons;
            }
        }
        public decimal washWater { get; set; }
        public WashWaterUnits washWaterUnits { get; set; }
        public decimal milkProduction { get; set; }
        public decimal? solidPerGalPerAnimalPerDay { get; set; }
        public override string ManureId => $"Generated{Id ?? 0}";
        public override string ManagedManureName => animalSubTypeName;
    }
}
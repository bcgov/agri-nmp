using Agri.Models.Configuration;
using System;
using Agri.Models;

namespace Agri.Models.Farm
{
    public class GeneratedManure : ManagedManure
    {
        public int AnimalId { get; set; }
        public string AnimalName { get; set; }
        public int AnimalSubTypeId { get; set; }
        public string AnimalSubTypeName { get; set; }
        public int AverageAnimalNumber { get; set; }
        public string ManureTypeName { get; set; }
        public string AnnualAmount { get; set; }
        public decimal AnnualAmountDecimal => AnnualAmount != null ? Convert.ToDecimal(AnnualAmount.Split(' ')[0]) : 0M;
        public string WashWaterGallonsToString => string.Format("{0:#,##0}", WashWaterGallons);

        public decimal WashWaterGallons
        {
            get
            {
                var washWaterGallons = 0m;
                if (WashWaterUnits == WashWaterUnits.USGallonsPerDayPerAnimal)
                {
                    washWaterGallons = Math.Round(Convert.ToDecimal(WashWater) * Convert.ToInt32(AverageAnimalNumber) * 365);
                }
                else if (WashWaterUnits == WashWaterUnits.USGallonsPerDay)
                {
                    washWaterGallons = Math.Round(Convert.ToDecimal(WashWater) * 365);
                }

                return washWaterGallons;
            }
        }

        public decimal WashWater { get; set; }
        public WashWaterUnits WashWaterUnits { get; set; }
        public decimal MilkProduction { get; set; }
        public decimal? SolidPerGalPerAnimalPerDay { get; set; }
        public override string ManureId => $"Generated{Id ?? 0}";
        public override string ManagedManureName => AnimalSubTypeName;
        public int BreedId;
        public string BreedName;
        public int GrazingDaysPerYear { get; set; }
        public bool ShowBreedAndGrazingDaysPerYear { get; set; }
    }
}
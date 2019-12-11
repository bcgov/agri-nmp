using Agri.Data;

namespace Agri.CalculateService
{
    public interface ICalculateAnimalRequirement
    {
        decimal? MilkProduction { get; set; }
        decimal? WashWater { get; set; }

        decimal? GetBreedManureFactorByBreedId(int _breedId);

        decimal? GetDefaultMilkProductionBySubTypeId(int _subTypeId);

        decimal? GetWashWaterBySubTypeId(int _subTypeId);
    }

    public class CalculateAnimalRequirement : ICalculateAnimalRequirement
    {
        private readonly IAgriConfigurationRepository _sd;

        public CalculateAnimalRequirement(IAgriConfigurationRepository sd)
        {
            _sd = sd;
        }

        public decimal? WashWater { get; set; }
        public decimal? MilkProduction { get; set; }

        // default for Wash Water
        public decimal? GetWashWaterBySubTypeId(int _subTypeId)
        {
            decimal? defaultWashWater = _sd.GetIncludeWashWater(_subTypeId);

            return defaultWashWater;
        }

        // default for Milk Production
        public decimal? GetDefaultMilkProductionBySubTypeId(int _subTypeId)
        {
            decimal? defaultMilkProduction = _sd.GetMilkProduction(_subTypeId);

            return defaultMilkProduction;
        }

        // breed manure factor for an animal by breed
        public decimal? GetBreedManureFactorByBreedId(int _breedId)
        {
            decimal? breedManureFactor = _sd.GetBreedManureFactorByBreedId(_breedId);

            return breedManureFactor;
        }
    }
}
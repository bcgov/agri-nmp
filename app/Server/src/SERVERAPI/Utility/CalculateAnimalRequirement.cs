using Agri.Interfaces;
using SERVERAPI.Models.Impl;


namespace SERVERAPI.Utility
{
    public class CalculateAnimalRequirement
    {
        private UserData _ud;
        private IAgriConfigurationRepository _sd;

        public CalculateAnimalRequirement(UserData ud, IAgriConfigurationRepository sd)
        {
            _ud = ud;
            _sd = sd;
        }
        public decimal? washWater { get; set; }
        public decimal? milkProduction { get; set; }

        // default for Wash Water
        public decimal? GetWashWaterBySubTypeId(int _subTypeId)
        {
            decimal? defaultWashWater = null;
            defaultWashWater = _sd.GetIncludeWashWater(_subTypeId);

            return defaultWashWater;
        }

        // default for Milk Production
        public decimal? GetDefaultMilkProductionBySubTypeId(int _subTypeId)
        {
            decimal? defaultMilkProduction = null;
            defaultMilkProduction = _sd.GetMilkProduction(_subTypeId);

            return defaultMilkProduction;
        }

        // breed manure factor for an animal by breed
        public decimal? GetBreedManureFactorByBreedId(int _breedId)
        {
            decimal? breedManureFactor = null;
            breedManureFactor = _sd.GetBreedManureFactorByBreedId(_breedId);

            return breedManureFactor;
        }

    }

}
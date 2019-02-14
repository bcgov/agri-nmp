using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class StaticDataVersion
    {
        public StaticDataVersion()
        {
            AmmoniaRetentions = new List<AmmoniaRetention>();
            Animals = new List<Animal>();
            AnimalSubTypes = new List<AnimalSubType>();
            BCSampleDateForNitrateCredits = new List<BCSampleDateForNitrateCredit>();
            Breeds = new List<Breed>();
            ConversionFactors = new List<ConversionFactor>();
            Crops = new List<Crop>();
            CropSoilTestPhosphorousRegions = new List<CropSoilTestPhosphorousRegion>();
            CropSoilTestPotassiumRegions = new List<CropSoilTestPotassiumRegion>();
            CropTypes = new List<CropType>();
            CropYields = new List<CropYield>();
            DefaultSoilTests = new List<DefaultSoilTest>();
            DensityUnits = new List<DensityUnit>();
            DryMatters = new List<DryMatter>();
            Fertilizers = new List<Fertilizer>();
            FertilizerMethods = new List<FertilizerMethod>();
            HarvestUnits = new List<HarvestUnit>();
            LiquidFertilizerDensities = new List<LiquidFertilizerDensity>();
            LiquidMaterialApplicationUsGallonsPerAcreRateConversions = new List<LiquidMaterialApplicationUSGallonsPerAcreRateConversion>();
            LiquidMaterialsConversionFactors = new List<LiquidMaterialsConversionFactor>();
            LiquidSolidSeparationDefaults = new List<LiquidSolidSeparationDefault>();
            ManureImportedDefaults = new List<ManureImportedDefault>();
            Manures = new List<Manure>();
            Messages = new List<Message>();
            NitrateCreditSampleDates = new List<NitrateCreditSampleDate>();
            NitrogenMineralizations = new List<NitrogenMineralization>();
            NitrogenRecommendations = new List<NitrogenRecommendation>();
            PhosphorusSoilTestRanges = new List<PhosphorusSoilTestRange>();
            PotassiumSoilTestRanges = new List<PotassiumSoilTestRange>();
            PreviousCropTypes = new List<PreviousCropType>();
            PrevManureApplicationYears = new List<PreviousManureApplicationYear>();
            PrevYearManureApplicationNitrogenDefaults = new List<PreviousYearManureApplicationNitrogenDefault>();
            Regions = new List<Region>();
            RptCompletedFertilizerRequiredStdUnits = new List<RptCompletedFertilizerRequiredStdUnit>();
            RptCompletedManureRequiredStdUnits = new List<RptCompletedManureRequiredStdUnit>();
            SeasonApplications = new List<SeasonApplication>();
            SoilTestMethods = new List<SoilTestMethod>();
            SoilTestPhosphorusRanges = new List<SoilTestPhosphorusRange>();
            SoilTestPhosphorousKelownaRanges = new List<SoilTestPhosphorousKelownaRange>();
            SoilTestPhosphorousRecommendations = new List<SoilTestPhosphorousRecommendation>();
            SoilTestPotassiumRanges = new List<SoilTestPotassiumRange>();
            SoilTestPotassiumKelownaRanges = new List<SoilTestPotassiumKelownaRange>();
            SoilTestPotassiumRecommendations = new List<SoilTestPotassiumRecommendation>();
            SolidMaterialApplicationTonPerAcreRateConversions = new List<SolidMaterialApplicationTonPerAcreRateConversion>();
            SolidMaterialsConversionFactors = new List<SolidMaterialsConversionFactor>();
            Units = new List<Unit>();
            SubRegions = new List<SubRegion>();
            Yields = new List<Yield>();
        }

        [Key]
        public int Id { get; set; }
        public string Version { get; set; }
        public DateTime CreatedDateTime { get; set; }


        public List<AmmoniaRetention> AmmoniaRetentions { get; set; }
        public List<Animal> Animals { get; set; }
        public List<AnimalSubType> AnimalSubTypes { get; set; }
        public List<BCSampleDateForNitrateCredit> BCSampleDateForNitrateCredits { get; set; }
        public List<Breed> Breeds { get; set; }
        public List<ConversionFactor> ConversionFactors { get; set; }
        public List<Crop> Crops { get; set; }
        public List<CropSoilTestPhosphorousRegion> CropSoilTestPhosphorousRegions { get; set; }
        public List<CropSoilTestPotassiumRegion> CropSoilTestPotassiumRegions { get; set; }
        public List<CropType> CropTypes { get; set; }
        public List<CropYield> CropYields { get; set; }
        public List<DefaultSoilTest> DefaultSoilTests { get; set; }
        public List<DensityUnit> DensityUnits { get; set; }
        public List<DryMatter> DryMatters { get; set; }
        public List<Fertilizer> Fertilizers { get; set; }
        public List<FertilizerMethod> FertilizerMethods { get; set; }
        public List<HarvestUnit> HarvestUnits { get; set; }
        public List<LiquidFertilizerDensity> LiquidFertilizerDensities { get; set; }
        public List<LiquidMaterialApplicationUSGallonsPerAcreRateConversion> LiquidMaterialApplicationUsGallonsPerAcreRateConversions { get; set; }
        public List<LiquidMaterialsConversionFactor> LiquidMaterialsConversionFactors { get; set; }
        public List<LiquidSolidSeparationDefault> LiquidSolidSeparationDefaults { get; set; }
        public List<ManureImportedDefault> ManureImportedDefaults { get; set; }
        public List<Manure> Manures { get; set; }
        public List<Message> Messages { get; set; }
        public List<NitrateCreditSampleDate> NitrateCreditSampleDates { get; set; }
        public List<NitrogenMineralization> NitrogenMineralizations { get; set; }
        public List<NitrogenRecommendation> NitrogenRecommendations { get; set; }
        public List<PhosphorusSoilTestRange> PhosphorusSoilTestRanges { get; set; }
        public List<PotassiumSoilTestRange> PotassiumSoilTestRanges { get; set; }
        public List<PreviousCropType> PreviousCropTypes { get; set; }
        public List<PreviousManureApplicationYear> PrevManureApplicationYears { get; set; }
        public List<PreviousYearManureApplicationNitrogenDefault> PrevYearManureApplicationNitrogenDefaults { get; set; }
        public List<Region> Regions { get; set; }
        public List<RptCompletedFertilizerRequiredStdUnit> RptCompletedFertilizerRequiredStdUnits { get; set; }
        public List<RptCompletedManureRequiredStdUnit> RptCompletedManureRequiredStdUnits { get; set; }
        public List<SeasonApplication> SeasonApplications { get; set; }
        public List<SoilTestMethod> SoilTestMethods { get; set; }
        public List<SoilTestPhosphorusRange> SoilTestPhosphorusRanges { get; set; }
        public List<SoilTestPhosphorousKelownaRange> SoilTestPhosphorousKelownaRanges { get; set; }
        public List<SoilTestPhosphorousRecommendation> SoilTestPhosphorousRecommendations { get; set; }
        public List<SoilTestPotassiumRange> SoilTestPotassiumRanges { get; set; }
        public List<SoilTestPotassiumKelownaRange> SoilTestPotassiumKelownaRanges { get; set; }
        public List<SoilTestPotassiumRecommendation> SoilTestPotassiumRecommendations { get; set; }
        public List<SolidMaterialApplicationTonPerAcreRateConversion> SolidMaterialApplicationTonPerAcreRateConversions { get; set; }
        public List<SolidMaterialsConversionFactor> SolidMaterialsConversionFactors { get; set; }
        public List<Unit> Units { get; set; }
        public List<SubRegion> SubRegions { get; set; }
        public List<Yield> Yields { get; set; }
    }
}

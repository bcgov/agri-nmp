using System;
using System.Collections.Generic;
using Agri.Models;
using Agri.Models.Calculate;
using Agri.Models.Configuration;
using Agri.Models.Security;

namespace Agri.Interfaces
{
    public interface IAgriConfigurationRepository
    {
        List<AmmoniaRetention> GetAmmoniaRetentions();
        List<CropYield> GetCropYields();
        List<Location> GetLocations();
        List<CropSoilTestPotassiumRegion> GetCropSoilTestPotassiumRegions();
        List<CropSoilTestPhosphorousRegion> GetCropSoilTestPhosphorousRegions();
        List<UserPrompt> GetUserPrompts();
        List<ExternalLink> GetExternalLinks();
        List<SoilTestPhosphorusRange> GetSoilTestPhosphorusRanges();
        List<SoilTestPotassiumRange> GetSoilTestPotassiumRanges();
        List<Message> GetMessages();
        List<SeasonApplication> GetSeasonApplications();
        List<Yield> GetYields();
        List<NitrogenRecommendation> GetNitrogenRecommendations();
        RptCompletedManureRequiredStdUnit GetRptCompletedManureRequiredStdUnit();
        StaticDataVersion GetCurrentStaticDataVersion();
        RptCompletedFertilizerRequiredStdUnit GetRptCompletedFertilizerRequiredStdUnit();
        BCSampleDateForNitrateCredit GetBCSampleDateForNitrateCredit();
        List<SoilTestPotassiumKelownaRange> GetSoilTestPotassiumKelownaRanges();
        List<SoilTestPhosphorousKelownaRange> GetSoilTestPhosphorousKelownaRanges();
        List<SoilTestPotassiumRecommendation> GetSoilTestPotassiumRecommendations();
        List<SoilTestPhosphorousRecommendation> GetSoilTestPhosphorousRecommendations();
        List<HarvestUnit> GetHarvestUnits();
        List<LiquidFertilizerDensity> GetLiquidFertilizerDensities();
        List<NitrogenMineralization> GetNitrogeMineralizations();

        List<KeyValuePair<string, string>> GetSoilConverterDetails();

        List<DryMatter> GetDryMatters();
        List<Region> GetRegions();
        List<SelectListItem> GetRegionsDll();
        SubRegion GetSubRegion(int? subRegionId);
        List<SubRegion> GetSubRegions();
        List<SelectListItem> GetSubRegionsDll(int? regionId);
        Manure GetManure(string manId);
        Manure GetManureByName(string manureName);
        List<Manure> GetManures();
        List<SelectListItem> GetManuresDll();
        SeasonApplication GetApplication(string applId);
        List<SeasonApplication> GetApplications();
        List<SelectListItem> GetApplicationsDll(string manureType);
        Unit GetUnit(string unitId);
        List<Unit> GetUnits();
        List<SelectListItem> GetUnitsDll(string unitType);
        List<FertilizerUnit> GetFertilizerUnits();
        List<SelectListItem> GetFertilizerUnitsDll(string unitType);
        FertilizerUnit GetFertilizerUnit(int Id);
        List<DensityUnit> GetDensityUnits();
        List<SelectListItem> GetDensityUnitsDll();
        DensityUnit GetDensityUnit(int Id);
        List<CropType> GetCropTypes();
        CropType GetCropType(int id);
        List<SelectListItem> GetCropTypesDll();
        List<Crop> GetCrops();
        List<SelectListItem> GetCropsDll(int cropType);
        List<Crop> GetCrops(int cropType);
        List<Crop> GetCropsByManureApplicationHistory(int manureAppHistory);
        Crop GetCrop(int cropId);
        int GetCropPrevYearManureApplVolCatCd(int cropId);
        Yield GetYieldById(int yieldId);
        CropSoilTestPhosphorousRegion GetCropSTPRegionCd(int cropid, int soilTestPotassiumRegionCode);
        CropSoilTestPotassiumRegion GetCropSTKRegionCd(int cropid, int soilTestPotassiumRegionCode);
        DryMatter GetDryMatter(int ID);
        AmmoniaRetention GetAmmoniaRetention(int seasonApplicatonId, int dm);
        NitrogenMineralization GetNMineralization(int id, int locationid);
        string GetSoilTestMethod(string id);
        List<SoilTestMethod> GetSoilTestMethods();
        List<SelectListItem> GetSoilTestMethodsDll();
        Region GetRegion(int id);
        PreviousCropType GetPrevCropType(int id);
        List<PreviousCropType> GetPreviousCropTypes();
        List<SelectListItem> GetPrevCropTypesDll(string prevCropCd);
        CropYield GetCropYield(int cropid, int locationid);

        SoilTestPhosphorousRecommendation GetSTPRecommend(int stp_kelowna_rangeid,
            int soil_test_phosphorous_region_cd, int phosphorous_crop_group_region_cd);

        SoilTestPhosphorousKelownaRange GetSTPKelownaRangeByPpm(int ppm);

        SoilTestPotassiumRecommendation GetSTKRecommend(int stk_kelowna_rangeid,
            int soil_test_potassium_region_cd, int potassium_crop_group_region_cd);

        SoilTestPotassiumKelownaRange GetSTKKelownaRangeByPpm(int ppm);
        ConversionFactor GetConversionFactor();
        BalanceMessages GetMessageByChemicalBalance(string balanceType, long balance, bool legume);
        string GetMessageByChemicalBalance(string balanceType, long balance, bool legume, decimal soilTest);

        BalanceMessages GetMessageByChemicalBalance(string balanceType, long balance1, long balance2,
            string assignedChemical);

        FertilizerType GetFertilizerType(string id);
        List<FertilizerType> GetFertilizerTypes();
        List<SelectListItem> GetFertilizerTypesDll();
        Fertilizer GetFertilizer(string id);
        List<Fertilizer> GetFertilizers();
        List<SelectListItem> GetFertilizersDll(string fertilizerType);
        SoilTestMethod GetSoilTestMethodByMethod(string _soilTest);
        SoilTestMethod GetSoilTestMethodById(string id);
        LiquidFertilizerDensity GetLiquidFertilizerDensity(int fertilizerId, int densityId);
        DefaultSoilTest GetDefaultSoilTest();
        string GetDefaultSoilTestMethod();
        List<PotassiumSoilTestRange> GetPotassiumSoilTestRanges();
        List<PhosphorusSoilTestRange> GetPhosphorusSoilTestRanges();
        string GetPotassiumSoilTestRating(decimal value);
        string GetPhosphorusSoilTestRating(decimal value);
        FertilizerMethod GetFertilizerMethod(string id);
        List<FertilizerMethod> GetFertilizerMethods();
        List<SelectListItem> GetFertilizerMethodsDll();
        string GetSoilTestWarning();
        string GetExternalLink(string name);
        string GetUserPrompt(string name);
        StaticDataVersion GetLatestVersionDataTree();
        string GetStaticDataVersion();
        List<PreviousManureApplicationYear> GetPrevManureApplicationInPrevYears();
        PreviousManureApplicationYear GetPrevManureApplicationInPrevYearsByManureAppHistory(
            int manureAppHistory);
        List<PreviousYearManureApplicationNitrogenDefault> GetPrevYearManureNitrogenCreditDefaults();
        bool WasManureAddedInPreviousYear(string userSelectedPrevYearsManureAdded);
        int GetInteriorId();
        bool IsRegionInteriorBC(int? region);
        bool IsNitrateCreditApplicable(int? region, DateTime sampleDate, int yearOfAnalysis);
        decimal GetSoilTestNitratePPMToPoundPerAcreConversionFactor();
        List<Browser> GetAllowableBrowsers();
        bool IsManureClassCompostType(string manure_class);
        bool IsManureClassCompostClassType(string manure_class);
        bool IsManureClassOtherType(string manure_class);
        string GetManureRptStdUnit(string solidLiquid);
        string GetFertilizerRptStdUnit(string dryLiquid);
        bool IsCustomFertilizer(int fertilizerTypeID);
        bool IsFertilizerTypeDry(int fertilizerTypeID);
        bool IsFertilizerTypeLiquid(int fertilizerTypeID);
        FertilizerType GetFertilizerType(int fertilizerTypeID);
        bool IsCropGrainsAndOilseeds(int cropType);
        List<SelectListItem> GetCropHarvestUnitsDll();
        string GetHarvestYieldUnitName(string yieldUnit);
        string GetHarvestYieldDefaultUnitName();
        bool IsCropHarvestYieldDefaultUnit(int selectedCropYieldUnit);
        int GetHarvestYieldDefaultUnit();
        int GetHarvestYieldDefaultDisplayUnit();
        decimal ConvertYieldFromBushelToTonsPerAcre(int cropid, decimal yield);
        List<NutrientIcon> GetNutrientIcons();
        NutrientIcon GetNutrientIcon(string name);
        List<Animal> GetAnimals();
        Animal GetAnimal(int id);
        List<SelectListItem> GetAnimalTypesDll();
        List<AnimalSubType> GetAnimalSubTypes(int animalId);
        List<AnimalSubType> GetAnimalSubTypes();
        List<SelectListItem> GetSubtypesDll(int animalType);
        decimal GetIncludeWashWater(int Id);
        decimal GetMilkProduction(int Id);
        AnimalsUsingWashWater GetAnimalsUsingWashWater();
        bool DoesAnimalUseWashWater(int animalSubTypeId);
        AnimalSubType GetAnimalSubType(int id);
        List<MainMenu> GetMainMenus();
        MainMenu GetMainMenu(CoreSiteActions action);
        List<SelectListItem> GetMainMenusDll();
        List<SubMenu> GetSubMenus();
        List<SelectListItem> GetSubmenusDll();
        List<SubMenu> GetSubMenus(int mainMenuId);
        [Obsolete]
        List<StaticDataValidationMessages> ValidateRelationship(string childNode, string childfield,
            string parentNode, string parentfield);
        ManureImportedDefault GetManureImportedDefault();
        List<SolidMaterialsConversionFactor> GetSolidMaterialsConversionFactors();
        List<LiquidMaterialsConversionFactor> GetLiquidMaterialsConversionFactors();
        List<SolidMaterialApplicationTonPerAcreRateConversion> GetSolidMaterialApplicationTonPerAcreRateConversions();
        List<LiquidMaterialApplicationUSGallonsPerAcreRateConversion> GetLiquidMaterialApplicationUSGallonsPerAcreRateConversion();
        List<Breed> GetBreeds();
        List<SelectListItem> GetBreedsDll(int animalType);
        decimal GetBreedManureFactorByBreedId(int breedId);
        List<SelectListItem> GetBreed(int breedId);
        LiquidSolidSeparationDefault GetLiquidSolidSeparationDefaults();
        int ArchiveConfigurations(ManageVersionUser manageVersionUser);
        bool AuthenticateManagerVersionUser(string username, string password);
        ManageVersionUser GetManagerVersionUser(string username);
    }
}
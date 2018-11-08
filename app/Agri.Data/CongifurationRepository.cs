using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agri.Models.Calculate;
using Agri.Models.Configuration;
using Version = Agri.Models.Configuration.Version;

namespace Agri.Data
{
    public class CongifurationRepository : IConfigurationRepository
    {
        public List<Region> GetRegions()
        {
            throw new NotImplementedException();
        }

        public List<SelectListItem> GetRegionsDll()
        {
            throw new NotImplementedException();
        }

        public Manure GetManure(string manId)
        {
            throw new NotImplementedException();
        }

        public Manure GetManureByName(string manureName)
        {
            throw new NotImplementedException();
        }

        public List<Manure> GetManures()
        {
            throw new NotImplementedException();
        }

        public List<SelectListItem> GetManuresDll()
        {
            throw new NotImplementedException();
        }

        public SeasonApplication GetApplication(string applId)
        {
            throw new NotImplementedException();
        }

        public List<SeasonApplication> GetApplications()
        {
            throw new NotImplementedException();
        }

        public List<SelectListItem> GetApplicationsDll(string manureType)
        {
            throw new NotImplementedException();
        }

        public Unit GetUnit(string unitId)
        {
            throw new NotImplementedException();
        }

        public List<Unit> GetUnits()
        {
            throw new NotImplementedException();
        }

        public List<SelectListItem> GetUnitsDll(string unitType)
        {
            throw new NotImplementedException();
        }

        public List<FertilizerUnit> GetFertilizerUnits()
        {
            throw new NotImplementedException();
        }

        public List<SelectListItem> GetFertilizerUnitsDll(string unitType)
        {
            throw new NotImplementedException();
        }

        public FertilizerUnit GetFertilizerUnit(int Id)
        {
            throw new NotImplementedException();
        }

        public List<DensityUnit> GetDensityUnits()
        {
            throw new NotImplementedException();
        }

        public List<SelectListItem> GetDensityUnitsDll()
        {
            throw new NotImplementedException();
        }

        public DensityUnit GetDensityUnit(int Id)
        {
            throw new NotImplementedException();
        }

        public List<CropType> GetCropTypes()
        {
            throw new NotImplementedException();
        }

        public CropType GetCropType(int id)
        {
            throw new NotImplementedException();
        }

        public List<SelectListItem> GetCropTypesDll()
        {
            throw new NotImplementedException();
        }

        public List<Crop> GetCrops()
        {
            throw new NotImplementedException();
        }

        public List<SelectListItem> GetCropsDll(int cropType)
        {
            throw new NotImplementedException();
        }

        public List<Crop> GetCrops(int cropType)
        {
            throw new NotImplementedException();
        }

        public Crop GetCrop(int cropId)
        {
            throw new NotImplementedException();
        }

        public int GetCropPrevYearManureApplVolCatCd(int cropId)
        {
            throw new NotImplementedException();
        }

        public List<Yield> GetYield(int yieldId)
        {
            throw new NotImplementedException();
        }

        public CropSoilTestPhosphorousRegion GetCropSTPRegionCd(int cropid, int soil_test_phosphorous_region_cd)
        {
            throw new NotImplementedException();
        }

        public CropSoilTestPotassiumRegion GetCropSTKRegionCd(int cropid, int soil_test_potassium_region_cd)
        {
            throw new NotImplementedException();
        }

        public DryMatter GetDryMatter(int ID)
        {
            throw new NotImplementedException();
        }

        public AmmoniaRetention GetAmmoniaRetention(int seasonApplicatonId, int dm)
        {
            throw new NotImplementedException();
        }

        public NitrogenMineralization GetNMineralization(int id, int locationid)
        {
            throw new NotImplementedException();
        }

        public string GetSoilTestMethod(string id)
        {
            throw new NotImplementedException();
        }

        public List<SoilTestMethod> GetSoilTestMethods()
        {
            throw new NotImplementedException();
        }

        public List<SelectListItem> GetSoilTestMethodsDll()
        {
            throw new NotImplementedException();
        }

        public Region GetRegion(int id)
        {
            throw new NotImplementedException();
        }

        public PreviousCropType GetPrevCropType(int id)
        {
            throw new NotImplementedException();
        }

        public List<PreviousCropType> GetPreviousCropTypes()
        {
            throw new NotImplementedException();
        }

        public List<SelectListItem> GetPrevCropTypesDll(string prevCropCd)
        {
            throw new NotImplementedException();
        }

        public CropYield GetCropYield(int cropid, int locationid)
        {
            throw new NotImplementedException();
        }

        public SoilTestPhosphorousRecommendation GetSTPRecommend(int stp_kelowna_rangeid, int soil_test_phosphorous_region_cd,
            int phosphorous_crop_group_region_cd)
        {
            throw new NotImplementedException();
        }

        public SoilTestPhosphorousKelownaRange GetSTPKelownaRangeByPpm(int ppm)
        {
            throw new NotImplementedException();
        }

        public SoilTestPotassiumRecommendation GetSTKRecommend(int stk_kelowna_rangeid, int soil_test_potassium_region_cd,
            int potassium_crop_group_region_cd)
        {
            throw new NotImplementedException();
        }

        public SoilTestPotassiumKelownaRange GetSTKKelownaRangeByPpm(int ppm)
        {
            throw new NotImplementedException();
        }

        public ConversionFactor GetConversionFactor()
        {
            throw new NotImplementedException();
        }

        public BalanceMessages GetMessageByChemicalBalance(string balanceType, long balance, bool legume)
        {
            throw new NotImplementedException();
        }

        public string GetMessageByChemicalBalance(string balanceType, long balance, bool legume, decimal soilTest)
        {
            throw new NotImplementedException();
        }

        public BalanceMessages GetMessageByChemicalBalance(string balanceType, long balance1, long balance2, string assignedChemical)
        {
            throw new NotImplementedException();
        }

        public FertilizerType GetFertilizerType(string id)
        {
            throw new NotImplementedException();
        }

        public List<FertilizerType> GetFertilizerTypes()
        {
            throw new NotImplementedException();
        }

        public List<SelectListItem> GetFertilizerTypesDll()
        {
            throw new NotImplementedException();
        }

        public Fertilizer GetFertilizer(string id)
        {
            throw new NotImplementedException();
        }

        public List<Fertilizer> GetFertilizers()
        {
            throw new NotImplementedException();
        }

        public List<SelectListItem> GetFertilizersDll(string fertilizerType)
        {
            throw new NotImplementedException();
        }

        public SoilTestMethod GetSoilTestMethodByMethod(string _soilTest)
        {
            throw new NotImplementedException();
        }

        public SoilTestMethod GetSoilTestMethodById(string id)
        {
            throw new NotImplementedException();
        }

        public LiquidFertilizerDensity GetLiquidFertilizerDensity(int fertilizerId, int densityId)
        {
            throw new NotImplementedException();
        }

        public DefaultSoilTest GetDefaultSoilTest()
        {
            throw new NotImplementedException();
        }

        public string GetDefaultSoilTestMethod()
        {
            throw new NotImplementedException();
        }

        public string SoilTestRating(string chem, decimal value)
        {
            throw new NotImplementedException();
        }

        public FertilizerMethod GetFertilizerMethod(string id)
        {
            throw new NotImplementedException();
        }

        public List<FertilizerMethod> GetFertilizerMethods()
        {
            throw new NotImplementedException();
        }

        public List<SelectListItem> GetFertilizerMethodsDll()
        {
            throw new NotImplementedException();
        }

        public string GetSoilTestWarning()
        {
            throw new NotImplementedException();
        }

        public string GetExternalLink(string name)
        {
            throw new NotImplementedException();
        }

        public string GetUserPrompt(string name)
        {
            throw new NotImplementedException();
        }

        public Version GetVersionData()
        {
            throw new NotImplementedException();
        }

        public string GetStaticDataVersion()
        {
            throw new NotImplementedException();
        }

        public List<PreviousManureApplicationYear> GetPrevManureApplicationInPrevYears()
        {
            throw new NotImplementedException();
        }

        public List<PreviousYearManureApplicationNitrogenDefault> GetPrevYearManureNitrogenCreditDefaults()
        {
            throw new NotImplementedException();
        }

        public bool wasManureAddedInPreviousYear(string userSelectedPrevYearsManureAdded)
        {
            throw new NotImplementedException();
        }

        public int GetInteriorId()
        {
            throw new NotImplementedException();
        }

        public bool IsRegionInteriorBC(int? region)
        {
            throw new NotImplementedException();
        }

        public bool IsNitrateCreditApplicable(int? region, DateTime sampleDate, int yearOfAnalysis)
        {
            throw new NotImplementedException();
        }

        public decimal GetSoilTestNitratePPMToPoundPerAcreConversionFactor()
        {
            throw new NotImplementedException();
        }

        public List<Browser> GetAllowableBrowsers()
        {
            throw new NotImplementedException();
        }

        public bool IsManureClassCompostType(string manure_class)
        {
            throw new NotImplementedException();
        }

        public bool IsManureClassCompostClassType(string manure_class)
        {
            throw new NotImplementedException();
        }

        public bool IsManureClassOtherType(string manure_class)
        {
            throw new NotImplementedException();
        }

        public string GetManureRptStdUnit(string solidLiquid)
        {
            throw new NotImplementedException();
        }

        public string GetFertilizerRptStdUnit(string dryLiquid)
        {
            throw new NotImplementedException();
        }

        public bool IsCustomFertilizer(int fertilizerTypeID)
        {
            throw new NotImplementedException();
        }

        public bool IsFertilizerTypeDry(int fertilizerTypeID)
        {
            throw new NotImplementedException();
        }

        public bool IsFertilizerTypeLiquid(int fertilizerTypeID)
        {
            throw new NotImplementedException();
        }

        public FertilizerType GetFertilizerType(int fertilizerTypeID)
        {
            throw new NotImplementedException();
        }

        public bool IsCropGrainsAndOilseeds(int cropType)
        {
            throw new NotImplementedException();
        }

        public List<SelectListItem> GetCropHarvestUnitsDll()
        {
            throw new NotImplementedException();
        }

        public string GetHarvestYieldUnitName(string yieldUnit)
        {
            throw new NotImplementedException();
        }

        public string GetHarvestYieldDefaultUnitName()
        {
            throw new NotImplementedException();
        }

        public bool IsCropHarvestYieldDefaultUnit(int selectedCropYieldUnit)
        {
            throw new NotImplementedException();
        }

        public int GetHarvestYieldDefaultUnit()
        {
            throw new NotImplementedException();
        }

        public int GetHarvestYieldDefaultDisplayUnit()
        {
            throw new NotImplementedException();
        }

        public decimal ConvertYieldFromBushelToTonsPerAcre(int cropid, decimal yield)
        {
            throw new NotImplementedException();
        }

        public List<NutrientIcon> GetNutrientIcons()
        {
            throw new NotImplementedException();
        }

        public NutrientIcon GetNutrientIcon(string name)
        {
            throw new NotImplementedException();
        }

        public List<Animal> GetAnimals()
        {
            throw new NotImplementedException();
        }

        public Animal GetAnimal(int id)
        {
            throw new NotImplementedException();
        }

        public List<SelectListItem> GetAnimalTypesDll()
        {
            throw new NotImplementedException();
        }

        public List<AnimalSubType> GetAnimalSubTypes(int animalId)
        {
            throw new NotImplementedException();
        }

        public List<AnimalSubType> GetAnimalSubTypes()
        {
            throw new NotImplementedException();
        }

        public List<SelectListItem> GetSubtypesDll(int animalType)
        {
            throw new NotImplementedException();
        }

        public List<SelectListItem> GetManureMaterialTypesDll()
        {
            throw new NotImplementedException();
        }
    }
}

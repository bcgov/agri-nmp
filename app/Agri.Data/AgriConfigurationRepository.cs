using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agri.Interfaces;
using Agri.Models.Calculate;
using Agri.Models.Configuration;
using Microsoft.EntityFrameworkCore;
using Version = Agri.Models.Configuration.Version;

namespace Agri.Data
{
    public class AgriConfigurationRepository : IAgriConfigurationRepository
    {
        private AgriConfigurationContext _context;

        private const string MANURE_CLASS_COMPOST = "Compost";
        private const string MANURE_CLASS_COMPOST_BOOK = "Compost_Book";
        private const string MANURE_CLASS_OTHER = "Other";
        private const int CROPTYPE_GRAINS_OILSEEDS_ID = 4;
        private const int CROP_YIELD_DEFAULT_CALCULATION_UNIT = 1;
        private const int CROP_YIELD_DEFAULT_DISPLAY_UNIT = 2;

        public AgriConfigurationRepository(AgriConfigurationContext context)
        {
            _context = context;
        }

        public decimal ConvertYieldFromBushelToTonsPerAcre(int cropid, decimal yield)
        {
            var crop = GetCrop(cropid);
            if (crop.HarvestBushelsPerTon.HasValue)
                return (yield / Convert.ToDecimal(crop.HarvestBushelsPerTon));
            return -1;
        }

        public List<Browser> GetAllowableBrowsers()
        {
            return _context.Browsers.ToList();
        }

        public AmmoniaRetention GetAmmoniaRetention(int seasonApplicatonId, int dm)
        {
            return _context.AmmoniaRetentions.SingleOrDefault(ar =>
                ar.SeasonApplicationId == seasonApplicatonId && ar.DryMatter == dm);
        }

        public List<AmmoniaRetention> GetAmmoniaRetentions()
        {
            return _context.AmmoniaRetentions.ToList();
        }

        public Animal GetAnimal(int id)
        {
            return _context.Animals
                .Where(a => a.Id == id)
                .Include(a => a.AnimalSubTypes)
                    .SingleOrDefault();
        }

        public List<Animal> GetAnimals()
        {
            return _context.Animals
                .Include(a => a.AnimalSubTypes)
                    .ToList();
        }

        public AnimalSubType GetAnimalSubType(int id)
        {
            throw new NotImplementedException();
        }

        public List<AnimalSubType> GetAnimalSubTypes(int animalId)
        {
            return GetAnimal(animalId)
                .AnimalSubTypes
                    .ToList();
        }

        public List<AnimalSubType> GetAnimalSubTypes()
        {
            return GetAnimals().SelectMany(a => a.AnimalSubTypes).ToList();
        }

        public List<SelectListItem> GetAnimalTypesDll()
        {
            return GetAnimalSubTypes().Select(st => new SelectListItem {Id = st.Id, Value = st.Name}).ToList();
        }

        public SeasonApplication GetApplication(string applId)
        {
            return GetApplications().SingleOrDefault(a => a.Id == Convert.ToInt32(applId));
        }

        public List<SeasonApplication> GetApplications()
        {
            return _context.SeasonApplications.ToList();
        }

        public List<SelectListItem> GetApplicationsDll(string manureType)
        {
            var appls = GetApplications();

            appls = appls.OrderBy(n => n.SortNum).ThenBy(n => n.Name).ToList();

            List<SelectListItem> applsOptions = new List<SelectListItem>();

            foreach (var r in appls)
            {
                if (r.ManureType.Contains(manureType))
                {
                    var li = new SelectListItem()
                        { Id = r.Id, Value = r.Name };
                    applsOptions.Add(li);
                }
            }

            return applsOptions;
        }

        public BCSampleDateForNitrateCredit GetBCSampleDateForNitrateCredit()
        {
            return _context.BCSampleDateForNitrateCredit.FirstOrDefault();
        }

        public ConversionFactor GetConversionFactor()
        {
            return _context.ConversionFactors.FirstOrDefault();
        }

        public Crop GetCrop(int cropId)
        {
            return _context.Crops
                .Where(c => c.Id == cropId)
                .Include(c => GetCropYields())
                .Include(c => c.CropSoilTestPhosphorousRegions)
                .Include(c => c.CropSoilTestPotassiumRegions)
                    .SingleOrDefault();
        }

        public List<SelectListItem> GetCropHarvestUnitsDll()
        {
            var harvestUnits = _context.HarvestUnits.ToList();

            var harvestUnitsOptions = new List<SelectListItem>();
            foreach (var r in harvestUnits)
            {
                var li = new SelectListItem{ Id = r.Id, Value = r.Name };
                harvestUnitsOptions.Add(li);
            }

            return harvestUnitsOptions;
        }

        public int GetCropPrevYearManureApplVolCatCd(int cropId)
        {
            throw new NotImplementedException();
        }

        public List<Crop> GetCrops()
        {
            return _context.Crops.ToList();
        }

        public List<Crop> GetCrops(int cropType)
        {
            return GetCrops().Where(c => c.CropTypeId == cropType).ToList();
        }

        public List<SelectListItem> GetCropsDll(int cropType)
        {
            var crops = GetCrops();

            crops = crops.OrderBy(n => n.SortNumber).ThenBy(n => n.CropName).ToList();

            List<SelectListItem> cropsOptions = new List<SelectListItem>();

            foreach (var r in crops)
            {
                if (r.CropTypeId == cropType)
                {
                    var li = new SelectListItem()
                        { Id = r.Id, Value = r.CropName };
                    cropsOptions.Add(li);
                }
            }

            return cropsOptions;
        }

        public List<CropSoilTestPhosphorousRegion> GetCropSoilTestPhosphorousRegions()
        {
            return _context.CropSoilTestPhosphorousRegions.ToList();
        }

        public List<CropSoilTestPotassiumRegion> GetCropSoilTestPotassiumRegions()
        {
            return _context.CropSoilTestPotassiumRegions.ToList();
        }

        public CropSoilTestPotassiumRegion GetCropSTKRegionCd(int cropId, int soilTestPotassiumRegionCode)
        {
            return GetCropSoilTestPotassiumRegions()
                .Where(p => p.CropId == cropId && p.SoilTestPotassiumRegionCode == soilTestPotassiumRegionCode)
                .SingleOrDefault();
        }

        public CropSoilTestPhosphorousRegion GetCropSTPRegionCd(int cropId, int soilTestPotassiumRegionCode)
        {
            return GetCropSoilTestPhosphorousRegions()
                .Where(p => p.CropId == cropId && p.SoilTestPhosphorousRegionCode == soilTestPotassiumRegionCode)
                .SingleOrDefault();
        }

        public CropType GetCropType(int id)
        {
            return GetCropTypes().SingleOrDefault(c => c.Id == id);
        }

        public List<CropType> GetCropTypes()
        {
            return _context.CropTypes.ToList();
        }

        public List<SelectListItem> GetCropTypesDll()
        {
            var types = GetCropTypes();

            List<SelectListItem> typesOptions = new List<SelectListItem>();

            foreach (var r in types)
            {
                var li = new SelectListItem()
                    { Id = r.Id, Value = r.Name };
                typesOptions.Add(li);
            }

            return typesOptions;
        }

        public CropYield GetCropYield(int cropId, int locationId)
        {
            return GetCropYields().SingleOrDefault(cy => cy.CropId == cropId && cy.LocationId == locationId);
        }

        public List<CropYield> GetCropYields()
        {
            return _context.CropYields.ToList();
        }

        public DefaultSoilTest GetDefaultSoilTest()
        {
            throw new NotImplementedException();
        }

        public string GetDefaultSoilTestMethod()
        {
            throw new NotImplementedException();
        }

        public DensityUnit GetDensityUnit(int Id)
        {
            return GetDensityUnits().SingleOrDefault(du => du.Id == Id);
        }

        public List<DensityUnit> GetDensityUnits()
        {
            return _context.DensityUnits.ToList();
        }

        public List<SelectListItem> GetDensityUnitsDll()
        {
            var units = GetDensityUnits();

            List<SelectListItem> unitsOptions = new List<SelectListItem>();

            foreach (var r in units)
            {
                var li = new SelectListItem()
                    { Id = r.Id, Value = r.Name };
                unitsOptions.Add(li);
            }

            return unitsOptions;
        }

        public AnimalsUsingWashWater GetAnimalsUsingWashWater()
        {
            var animalsUsingWashWater = new AnimalsUsingWashWater();
            animalsUsingWashWater.Animals = new List<AnimalUsingWashWater>();

            foreach (var animalSubType in GetAnimalSubTypes().Where(ast => ast.WashWater > 0))
            {
                animalsUsingWashWater.Animals.Add(new AnimalUsingWashWater {AnimalSubTypeId = animalSubType.Id});
            }

            return animalsUsingWashWater;
        }

        public bool DoesAnimalUseWashWater(int animalSubTypeId)
        {
            return GetAnimalsUsingWashWater().Animals.Any(a => a.AnimalSubTypeId == animalSubTypeId);
        }

        public DryMatter GetDryMatter(int ID)
        {
            return GetDryMatters().SingleOrDefault(dm => dm.Id == ID);
        }

        public List<DryMatter> GetDryMatters()
        {
            return _context.DryMatters.ToList();
        }

        public string GetExternalLink(string name)
        {
            return GetExternalLinks()
                .Where(el => el.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                .SingleOrDefault().Url;
        }

        public List<ExternalLink> GetExternalLinks()
        {
            return _context.ExternalLinks.ToList();
        }

        public Fertilizer GetFertilizer(string id)
        {
            return GetFertilizers().SingleOrDefault(f => f.Id == Convert.ToInt32(id));
        }

        public FertilizerMethod GetFertilizerMethod(string id)
        {
            return GetFertilizerMethods().SingleOrDefault(fm => fm.Id == Convert.ToInt32(id));
        }

        public List<FertilizerMethod> GetFertilizerMethods()
        {
            return _context.FertilizerMethods.ToList();
        }

        public List<SelectListItem> GetFertilizerMethodsDll()
        {
            var fertilizerMethods = GetFertilizerMethods();
            List<SelectListItem> fertilizerMethodOptions = new List<SelectListItem>();
            foreach (var r in fertilizerMethods)
            {
                SelectListItem li = new SelectListItem()
                    { Id = r.Id, Value = r.Name };
                fertilizerMethodOptions.Add(li);
            }

            return fertilizerMethodOptions;
        }

        public string GetFertilizerRptStdUnit(string dryLiquid)
        {
            string stdUnit = string.Empty;

            if (dryLiquid.Equals("dry", StringComparison.CurrentCultureIgnoreCase))
            {
                stdUnit = GetFertilizerUnit(Convert.ToInt16(_context.RptCompletedFertilizerRequiredStdUnits.FirstOrDefault().SolidUnitId)).Name;
            }
            else
            {
                stdUnit = GetFertilizerUnit(Convert.ToInt16(_context.RptCompletedFertilizerRequiredStdUnits.FirstOrDefault().LiquidUnitId)).Name;
            }

            return ParseStdUnit(stdUnit);
        }

        public List<Fertilizer> GetFertilizers()
        {
            return _context.Fertilizers.ToList();
        }

        public List<SelectListItem> GetFertilizersDll(string fertilizerType)
        {
            var types = GetFertilizers();

            types = types.OrderBy(n => n.SortNum).ThenBy(n => n.Name).ToList();

            List<SelectListItem> typesOptions = new List<SelectListItem>();

            foreach (var r in types)
            {
                if (r.DryLiquid.ToString() == fertilizerType)
                {
                    var li = new SelectListItem()
                        { Id = r.Id, Value = r.Name };
                    typesOptions.Add(li);
                }
            }

            return typesOptions;
        }

        public FertilizerType GetFertilizerType(string id)
        {
            return GetFertilizerTypes().SingleOrDefault(ft => ft.Id == Convert.ToInt32(id));
        }

        public FertilizerType GetFertilizerType(int fertilizerTypeID)
        {
            return GetFertilizerTypes().SingleOrDefault(ft => ft.Id == fertilizerTypeID);
        }

        public List<FertilizerType> GetFertilizerTypes()
        {
            return _context.FertilizerTypes.ToList();
        }

        public List<SelectListItem> GetFertilizerTypesDll()
        {
            var types = GetFertilizerTypes();

            List<SelectListItem> typesOptions = new List<SelectListItem>();

            foreach (var r in types)
            {
                var li = new SelectListItem()
                {
                    Id = r.Id,
                    Value = r.Name
                };
                typesOptions.Add(li);
            }

            return typesOptions;
        }

        public FertilizerUnit GetFertilizerUnit(int Id)
        {
            return GetFertilizerUnits().SingleOrDefault(fu => fu.Id == Id);
        }

        public List<FertilizerUnit> GetFertilizerUnits()
        {
            return _context.FertilizerUnits.ToList();
        }

        public List<SelectListItem> GetFertilizerUnitsDll(string unitType)
        {
            var units = GetFertilizerUnits();

            List<SelectListItem> unitsOptions = new List<SelectListItem>();

            foreach (var r in units)
            {
                if (r.DryLiquid == unitType)
                {
                    var li = new SelectListItem()
                        { Id = r.Id, Value = r.Name };
                    unitsOptions.Add(li);
                }
            }

            return unitsOptions;
        }

        public List<HarvestUnit> GetHarvestUnits()
        {
            return _context.HarvestUnits.ToList();
        }

        public int GetHarvestYieldDefaultDisplayUnit()
        {
            return CROP_YIELD_DEFAULT_DISPLAY_UNIT;
        }

        public int GetHarvestYieldDefaultUnit()
        {
            return CROP_YIELD_DEFAULT_CALCULATION_UNIT;
        }

        public string GetHarvestYieldDefaultUnitName()
        {
            return GetHarvestYieldUnitName(CROP_YIELD_DEFAULT_CALCULATION_UNIT.ToString());
        }

        public string GetHarvestYieldUnitName(string yieldUnit)
        {
            return GetHarvestUnits().SingleOrDefault(hu =>
                hu.Id.ToString().Equals(yieldUnit, StringComparison.CurrentCultureIgnoreCase)).Name;
        }
        public decimal GetIncludeWashWater(int Id)
        {
            return GetAnimalSubTypes().SingleOrDefault(ast => ast.Id == Id).WashWater;
        }

        public int GetInteriorId()
        {
            return GetLocations().FirstOrDefault().Id;
        }

        public List<LiquidFertilizerDensity> GetLiquidFertilizerDensities()
        {
            return _context.LiquidFertilizerDensities.ToList();
        }

        public LiquidFertilizerDensity GetLiquidFertilizerDensity(int fertilizerId, int densityId)
        {
            return GetLiquidFertilizerDensities()
                .SingleOrDefault(lfd => lfd.FertilizerId == fertilizerId && lfd.DensityUnitId == densityId);
        }

        public List<Location> GetLocations()
        {
            return _context.Locations.ToList();
        }

        public Manure GetManure(string manId)
        {
            return GetManures().SingleOrDefault(m => m.Id == Convert.ToInt32(manId));
        }

        public Manure GetManureByName(string manureName)
        {
            return GetManures()
                .SingleOrDefault(m => m.Name.Equals(manureName, StringComparison.CurrentCultureIgnoreCase));
        }

        public string GetManureRptStdUnit(string solidLiquid)
        {
            string stdUnit;

            if (solidLiquid.Equals("solid", StringComparison.CurrentCultureIgnoreCase))
            {
                stdUnit = GetUnit(_context.RptCompletedManureRequiredStdUnits.FirstOrDefault().SolidUnitId.ToString()).Name;
            }
            else
            {
                stdUnit = GetUnit(_context.RptCompletedManureRequiredStdUnits.FirstOrDefault().LiquidUnitId.ToString()).Name;
            }

            return ParseStdUnit(stdUnit);
        }

        public List<Manure> GetManures()
        {
            return _context.Manures.ToList();
        }

        public List<SelectListItem> GetManuresDll()
        {
            var manures = GetManures();

            manures = manures.OrderBy(n => n.SortNum).ThenBy(n => n.Name).ToList();

            List<SelectListItem> manOptions = new List<SelectListItem>();

            foreach (var r in manures)
            {
                var li = new SelectListItem()
                    { Id = r.Id, Value = r.Name };
                manOptions.Add(li);
            }

            return manOptions;
        }

        public BalanceMessages GetMessageByChemicalBalance(string balanceType, long balance, bool legume)
        {
            var balanceMessages = GetMessages()
                .Where(m => m.BalanceType.Equals(balanceType, StringComparison.CurrentCultureIgnoreCase) &&
                            balance >= Convert.ToInt64(m.BalanceLow) &&
                            balance <= Convert.ToInt64(m.BalanceHigh))
                .Select(bm => new BalanceMessages
                {
                    Chemical = balanceType,
                    Message = bm.DisplayMessage.Equals("Yes", StringComparison.CurrentCultureIgnoreCase)  ? 
                        string.Format(bm.Text, Math.Abs(balance).ToString()) : null,
                    Icon = bm.Icon,
                    IconText = GetNutrientIcon(bm.Icon).Definition
                })
                .SingleOrDefault();

            if (balanceMessages != null &&
                balanceType.Equals("AgrN", StringComparison.CurrentCultureIgnoreCase) &&
                balanceMessages.Icon.Equals("stop", StringComparison.CurrentCultureIgnoreCase))
            {
                // nitrogen does not need to be added even if there is a deficiency
                balanceMessages.Icon = "good";
                balanceMessages.Message = string.Empty;
            }

            return balanceMessages;
        }

        public string GetMessageByChemicalBalance(string balanceType, long balance, bool legume, decimal soilTest)
        {

            return GetMessages()
                .Where(m => m.BalanceType.Equals(balanceType, StringComparison.CurrentCultureIgnoreCase) &&
                            balance >= Convert.ToInt64(m.BalanceLow) &&
                            balance <= Convert.ToInt64(m.BalanceHigh) &&
                            soilTest >= m.SoilTestLow &&
                            soilTest <= m.SoilTestHigh)
                .Select(m => balanceType.Equals("AgrN", StringComparison.CurrentCultureIgnoreCase) && 
                                    legume &&
                                    m.BalanceHigh == 9999 ? 
                                        string.Empty : // If legume crop in field never display that more N is required
                                        string.Format(m.Text, Math.Abs(balance).ToString()))
                .SingleOrDefault();
        }

        public BalanceMessages GetMessageByChemicalBalance(string balanceType, long balance1, long balance2, string assignedChemical)
        {

            return GetMessages()
                .Where(m => m.BalanceType.Equals(balanceType, StringComparison.CurrentCultureIgnoreCase) &&
                            balance1 >= Convert.ToInt64(m.BalanceLow) &&
                            balance1 <= Convert.ToInt64(m.BalanceHigh) &&
                   balance2 >= Convert.ToInt64(m.Balance1Low) &&
                   balance2<= Convert.ToInt64(m.Balance1High))
                .Select(m => new BalanceMessages
                {
                    Chemical = assignedChemical,
                    Message = m.DisplayMessage.Equals("Yes", StringComparison.CurrentCultureIgnoreCase) ? m.Text : null,
                    Icon = m.Icon
                })
                .SingleOrDefault();
        }

        public List<Message> GetMessages()
        {
            return _context.Messages.ToList();
        }

        public decimal GetMilkProduction(int Id)
        {
            return GetAnimalSubTypes().SingleOrDefault(ast => ast.Id == Id).MilkProduction;
        }
        public List<NitrogenMineralization> GetNitrogeMineralizations()
        {
            return _context.NitrogenMineralizations.ToList();
        }

        public List<NitrogenRecommendation> GetNitrogenRecommendations()
        {
            return _context.NitrogenRecommendations.ToList();
        }

        public NitrogenMineralization GetNMineralization(int id, int locationid)
        {
            return GetNitrogeMineralizations().SingleOrDefault(nm => nm.Id == id && nm.LocationId == locationid);
        }

        public NutrientIcon GetNutrientIcon(string name)
        {
            return GetNutrientIcons()
                .SingleOrDefault(ni => ni.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        public List<NutrientIcon> GetNutrientIcons()
        {
            return _context.NutrientIcons.ToList();
        }

        public PreviousCropType GetPrevCropType(int id)
        {
            return GetPreviousCropTypes().SingleOrDefault(pct => pct.Id == id);
        }

        public List<SelectListItem> GetPrevCropTypesDll(string prevCropCd)
        {
            var types = GetPreviousCropTypes();

            List<SelectListItem> typesOptions = new List<SelectListItem>();

            foreach (var r in types)
            {
                if (r.PreviousCropCode.ToString() == prevCropCd)
                {
                    var li = new SelectListItem()
                        { Id = r.Id, Value = r.Name };
                    typesOptions.Add(li);
                }
            }

            return typesOptions;
        }

        public List<PreviousCropType> GetPreviousCropTypes()
        {
            return GetCrops().SelectMany(c => c.PreviousCropTypes).ToList();
        }

        public List<PreviousManureApplicationYear> GetPrevManureApplicationInPrevYears()
        {
            return _context.PrevManureApplicationYears.ToList();
        }

        public List<PreviousYearManureApplicationNitrogenDefault> GetPrevYearManureNitrogenCreditDefaults()
        {
            return _context.PrevYearManureApplicationNitrogenDefaults.ToList();
        }

        public Region GetRegion(int id)
        {
            return GetRegions().SingleOrDefault(r => r.Id == id);
        }

        public List<Region> GetRegions()
        {
            return _context.Regions.ToList();
        }

        public List<SelectListItem> GetRegionsDll()
        {
            List<Region> regions = GetRegions();

            regions = regions.OrderBy(n => n.SortNumber).ThenBy(n => n.Name).ToList();

            var regOptions = new List<SelectListItem>();


            foreach (var r in regions)
            {
                var li = new SelectListItem()
                    { Id = r.Id, Value = r.Name };
                regOptions.Add(li);
            }

            return regOptions;
        }

        public RptCompletedFertilizerRequiredStdUnit GetRptCompletedFertilizerRequiredStdUnit()
        {
            return _context.RptCompletedFertilizerRequiredStdUnits.FirstOrDefault();
        }

        public RptCompletedManureRequiredStdUnit GetRptCompletedManureRequiredStdUnit()
        {
            return _context.RptCompletedManureRequiredStdUnits.FirstOrDefault();
        }

        public List<SeasonApplication> GetSeasonApplications()
        {
            return _context.SeasonApplications.ToList();
        }

        public string GetSoilTestMethod(string id)
        {
            return GetSoilTestMethods().SingleOrDefault(stt => stt.Id == Convert.ToInt32(id)).Name;
        }

        public SoilTestMethod GetSoilTestMethodById(string id)
        {
            return GetSoilTestMethods().SingleOrDefault(stm => stm.Id == Convert.ToInt32(id));
        }

        public SoilTestMethod GetSoilTestMethodByMethod(string soilTest)
        {
            return GetSoilTestMethods()
                .SingleOrDefault(stm => stm.Name.Equals(soilTest, StringComparison.CurrentCultureIgnoreCase));
        }

        public List<SoilTestMethod> GetSoilTestMethods()
        {
            return _context.SoilTestMethods.ToList();
        }

        public List<SelectListItem> GetSoilTestMethodsDll()
        {
            var soilTestMethods = GetSoilTestMethods();
            List<SelectListItem> soilTestMethodOptions = new List<SelectListItem>();
            foreach (var r in soilTestMethods)
            {
                SelectListItem li = new SelectListItem()
                    { Id = r.Id, Value = r.Name };
                soilTestMethodOptions.Add(li);
            }

            return soilTestMethodOptions;
        }

        public decimal GetSoilTestNitratePPMToPoundPerAcreConversionFactor()
        {
            throw new NotImplementedException();
        }

        public List<SoilTestPhosphorousKelownaRange> GetSoilTestPhosphorousKelownaRanges()
        {
            return _context.SoilTestPhosphorousKelownaRanges.ToList();
        }

        public List<SoilTestPhosphorousRecommendation> GetSoilTestPhosphorousRecommendations()
        {
            return GetSoilTestPhosphorousKelownaRanges().SelectMany(stp => stp.SoilTestPhosphorousRecommendations).ToList();
        }

        public List<SoilTestPhosphorusRange> GetSoilTestPhosphorusRanges()
        {
            return _context.SoilTestPhosphorusRanges.ToList();
        }

        public List<SoilTestPotassiumKelownaRange> GetSoilTestPotassiumKelownaRanges()
        {
            return _context.SoilTestPotassiumKelownaRanges.ToList();
        }

        public List<SoilTestPotassiumRange> GetSoilTestPotassiumRanges()
        {
            return _context.SoilTestPotassiumRanges.ToList();
        }

        public List<SoilTestPotassiumRecommendation> GetSoilTestPotassiumRecommendations()
        {
            return GetSoilTestPotassiumKelownaRanges().SelectMany(stp => stp.SoilTestPotassiumRecommendations).ToList();
        }

        public string GetSoilTestWarning()
        {
            var message = GetUserPrompt("defaultsoiltest");

            return string.Format(message, GetDefaultSoilTest().pH,
                GetDefaultSoilTest().ConvertedKelownaP,
                GetDefaultSoilTest().ConvertedKelownaK);
        }

        public string GetStaticDataVersion()
        {
            throw new NotImplementedException();
        }

        public SoilTestPotassiumKelownaRange GetSTKKelownaRangeByPpm(int ppm)
        {
            return GetSoilTestPotassiumKelownaRanges()
                .SingleOrDefault(stp => ppm >= stp.RangeLow && ppm <= stp.RangeHigh);
        }

        public SoilTestPotassiumRecommendation GetSTKRecommend(int stk_kelowna_rangeid, int soil_test_potassium_region_cd, int potassium_crop_group_region_cd)
        {
            throw new NotImplementedException();
        }

        public SoilTestPhosphorousKelownaRange GetSTPKelownaRangeByPpm(int ppm)
        {
            throw new NotImplementedException();
        }

        public SoilTestPhosphorousRecommendation GetSTPRecommend(int stp_kelowna_rangeid, int soil_test_phosphorous_region_cd, int phosphorous_crop_group_region_cd)
        {
            throw new NotImplementedException();
        }

        public List<SelectListItem> GetSubtypesDll(int animalType)
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

        public string GetUserPrompt(string name)
        {
            throw new NotImplementedException();
        }

        public List<UserPrompt> GetUserPrompts()
        {
            throw new NotImplementedException();
        }

        public Version GetVersionData()
        {
            throw new NotImplementedException();
        }

        public Yield GetYieldById(int yieldId)
        {
            throw new NotImplementedException();
        }

        public List<Yield> GetYield(int yieldId)
        {
            throw new NotImplementedException();
        }
        public List<Yield> GetYields()
        {
            throw new NotImplementedException();
        }

        public bool IsCropGrainsAndOilseeds(int cropType)
        {
            throw new NotImplementedException();
        }

        public bool IsCropHarvestYieldDefaultUnit(int selectedCropYieldUnit)
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

        public bool IsManureClassCompostClassType(string manure_class)
        {
            throw new NotImplementedException();
        }

        public bool IsManureClassCompostType(string manure_class)
        {
            throw new NotImplementedException();
        }

        public bool IsManureClassOtherType(string manure_class)
        {
            throw new NotImplementedException();
        }

        public bool IsNitrateCreditApplicable(int? region, DateTime sampleDate, int yearOfAnalysis)
        {
            throw new NotImplementedException();
        }

        public bool IsRegionInteriorBC(int? region)
        {
            throw new NotImplementedException();
        }

        public string SoilTestRating(string chem, decimal value)
        {
            throw new NotImplementedException();
        }

        public bool wasManureAddedInPreviousYear(string userSelectedPrevYearsManureAdded)
        {
            throw new NotImplementedException();
        }

        public List<StaticDataValidationMessages> ValidateRelationship(string childNode, string childfield,
            string parentNode, string parentfield)
        {
            throw new NotImplementedException();
        }


        private string ParseStdUnit(string stdUnit)
        {
            int idx = stdUnit.LastIndexOf("/");
            if (idx > 0)
                stdUnit = stdUnit.Substring(0, idx);

            return stdUnit;
        }
    }
}

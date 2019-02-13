using Agri.Interfaces;
using Agri.Models;
using Agri.Models.Calculate;
using Agri.Models.Configuration;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Agri.Data
{
    public class AgriConfigurationRepository : IAgriConfigurationRepository
    {
        private AgriConfigurationContext _context;
        private IMapper _mapper;
        private const string MANURE_CLASS_COMPOST = "Compost";
        private const string MANURE_CLASS_COMPOST_BOOK = "Compost_Book";
        private const string MANURE_CLASS_OTHER = "Other";
        private const int CROPTYPE_GRAINS_OILSEEDS_ID = 4;
        private const int CROP_YIELD_DEFAULT_CALCULATION_UNIT = 1;
        private const int CROP_YIELD_DEFAULT_DISPLAY_UNIT = 2;

        public AgriConfigurationRepository(AgriConfigurationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            return GetAnimalSubTypes().SingleOrDefault(ast => ast.Id == id);
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
            return GetAnimals().Select(st => new SelectListItem {Id = st.Id, Value = st.Name}).ToList();
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
                .Include(c => c.CropYields)
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
            return GetCrops().SingleOrDefault(c => c.Id == cropId).ManureApplicationHistory;
        }

        public List<Crop> GetCrops()
        {
            return _context.Crops
                .Include(c => c.CropYields)
                .Include(c => c.CropSoilTestPhosphorousRegions)
                .Include(c => c.CropSoilTestPotassiumRegions)
                .Include(c => c.PreviousCropTypes)
                .OrderBy(c => c.SortNumber)
                .ToList();
        }

        public List<Crop> GetCrops(int cropType)
        {
            return GetCrops().Where(c => c.CropTypeId == cropType).ToList();
        }

        public List<Crop> GetCropsByManureApplicationHistory(int manureAppHistory)
        {
            return GetCrops().Where(c => c.ManureApplicationHistory == manureAppHistory).ToList();
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

        public CropSoilTestPhosphorousRegion GetCropSTPRegionCd(int cropId, int soilTestPhosphorousRegionCode)
        {
            return GetCropSoilTestPhosphorousRegions()
                .Where(p => p.CropId == cropId && p.SoilTestPhosphorousRegionCode == soilTestPhosphorousRegionCode)
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
            types = types.OrderBy(t => t.Id).ToList();

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
            return _context.DefaultSoilTests.FirstOrDefault();
        }

        public string GetDefaultSoilTestMethod()
        {
            return GetDefaultSoilTest().DefaultSoilTestMethodId;
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
            return _context.FertilizerMethods.ToList().OrderBy(ni => ni.Id).ToList();
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
            return _context.Fertilizers.OrderBy(f => f.SortNum).ToList();
        }

        public List<SelectListItem> GetFertilizersDll(string fertilizerType)
        {
            var types = GetFertilizers();

            types = types.OrderBy(n => n.SortNum).ToList();

            List<SelectListItem> typesOptions = new List<SelectListItem>();

            foreach (var r in types)
            {
                if (r.DryLiquid.Equals(fertilizerType, StringComparison.CurrentCultureIgnoreCase))
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
            types = types.OrderBy(t => t.Id).ToList();

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
                if (r.DryLiquid.Equals(unitType, StringComparison.CurrentCultureIgnoreCase))
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
            return GetLocations().First(l => l.Name.Equals("Interior", StringComparison.CurrentCultureIgnoreCase)).Id;
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
            return _context.Manures.OrderBy(m => m.SortNum).ToList();
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
                .SingleOrDefault(m => 
                        m.BalanceType.Equals(balanceType, StringComparison.CurrentCultureIgnoreCase) &&
                        balance >= Convert.ToInt64(m.BalanceLow) &&
                        balance <= Convert.ToInt64(m.BalanceHigh));

            if (balanceMessages != null)
            {
                var message = new BalanceMessages
                {
                    Chemical = balanceType,
                    Icon = balanceMessages.Icon
                };

                if (balanceMessages.DisplayMessage.Equals("Yes", StringComparison.CurrentCultureIgnoreCase))
                {
                    message.Message = string.Format(balanceMessages.Text, Math.Abs(balance).ToString());
                }

                if (balanceMessages != null &&
                    balanceType.Equals("AgrN", StringComparison.CurrentCultureIgnoreCase) &&
                    balanceMessages.Icon.Equals("stop", StringComparison.CurrentCultureIgnoreCase))
                {
                    // nitrogen does not need to be added even if there is a deficiency
                    message.Icon = "good";
                    message.Message = string.Empty;
                }

                message.IconText = GetNutrientIcon(balanceMessages.Icon).Definition;
                return message;
            }
            return new BalanceMessages();
        }

        public string GetMessageByChemicalBalance(string balanceType, long balance, bool legume, decimal soilTest)
        {

            var message = GetMessages()
                .SingleOrDefault(m => m.BalanceType.Equals(balanceType, StringComparison.CurrentCultureIgnoreCase) &&
                            balance >= Convert.ToInt64(m.BalanceLow) &&
                            balance <= Convert.ToInt64(m.BalanceHigh) &&
                            soilTest >= m.SoilTestLow &&
                            soilTest <= m.SoilTestHigh);

            if (message != null)
            {
                if (balanceType.Equals("AgrN", StringComparison.CurrentCultureIgnoreCase) &&
                    legume &&
                    message.BalanceHigh == 9999)
                {
                    return string.Empty; // If legume crop in field never display that more N is required
                }
                else
                {
                    return string.Format(message.Text, Math.Abs(balance).ToString());
                }
            }

            return null;
        }

        public BalanceMessages GetMessageByChemicalBalance(string balanceType, long balance1, long balance2, string assignedChemical)
        {

            var message = GetMessages()
                .SingleOrDefault(m => m.BalanceType.Equals(balanceType, StringComparison.CurrentCultureIgnoreCase) &&
                            balance1 >= Convert.ToInt64(m.BalanceLow) &&
                            balance1 <= Convert.ToInt64(m.BalanceHigh) &&
                   balance2 >= Convert.ToInt64(m.Balance1Low) &&
                   balance2<= Convert.ToInt64(m.Balance1High));

            if (message != null)
            {
                var balanceMessage = new BalanceMessages
                {
                    Chemical = assignedChemical,
                    Message = message.DisplayMessage.Equals("Yes", StringComparison.CurrentCultureIgnoreCase)
                        ? message.Text
                        : null,
                    Icon = message.Icon
                };

                return balanceMessage;
            }
            return new BalanceMessages();
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
            return _context.NutrientIcons.ToList().OrderBy(ni=>ni.Id).ToList();
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

        public PreviousManureApplicationYear GetPrevManureApplicationInPrevYearsByManureAppHistory(int manureAppHistory)
        {
            return GetPrevManureApplicationInPrevYears()
                .Where(c => c.FieldManureApplicationHistory == manureAppHistory).First();
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
            return _context.Regions.OrderBy(r => r.SortNumber).ToList();
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

        public List<SelectListItem> GetSubRegionsDll(int? regionId)
        {
            List<SubRegion> subRegions = _context.SubRegion.ToList();
            var subRegOptions = new List<SelectListItem>();


            foreach (var s in subRegions)
            {
                if (s.RegionId == regionId)
                {
                    var li = new SelectListItem()
                        {Id = s.Id, Value = s.Name};
                    subRegOptions.Add(li);
                }
            }

            return subRegOptions;
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
            return _context.SeasonApplications.OrderBy(sa => sa.SortNum).ToList();
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
            return _context.SoilTestMethods.OrderBy(stm => stm.SortNum).ToList();
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
            return GetConversionFactor().SoilTestPPMToPoundPerAcre;
        }

        public List<SoilTestPhosphorousKelownaRange> GetSoilTestPhosphorousKelownaRanges()
        {
            return _context.SoilTestPhosphorousKelownaRanges
                .Include(stpk => stpk.SoilTestPhosphorousRecommendations)
                .ToList();
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
            return _context.SoilTestPotassiumKelownaRanges
                .Include(stpk => stpk.SoilTestPotassiumRecommendations)
                .ToList();
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
            return _context.StaticDataVersions.FirstOrDefault().Version;
        }

        public SoilTestPotassiumKelownaRange GetSTKKelownaRangeByPpm(int ppm)
        {
            return GetSoilTestPotassiumKelownaRanges()
                .SingleOrDefault(stp => ppm >= stp.RangeLow && ppm <= stp.RangeHigh);
        }

        public SoilTestPotassiumRecommendation GetSTKRecommend(int stkKelownaRangeId, 
            int soilTestPotassiumRegionCode, 
            int potassiumCropGroupRegionCode)
        {
            return GetSoilTestPotassiumRecommendations().SingleOrDefault(stp =>
                stp.SoilTestPotassiumKelownaRangeId == stkKelownaRangeId &&
                stp.SoilTestPotassiumRegionCode == soilTestPotassiumRegionCode &&
                stp.PotassiumCropGroupRegionCode == potassiumCropGroupRegionCode);
        }

        public SoilTestPhosphorousKelownaRange GetSTPKelownaRangeByPpm(int ppm)
        {
            return GetSoilTestPhosphorousKelownaRanges()
                .SingleOrDefault(stp => ppm >= stp.RangeLow && ppm <= stp.RangeHigh);
        }

        public SoilTestPhosphorousRecommendation GetSTPRecommend(int stpKelownaRangeId, int soilTestPhosphorousRegionCode, int phosphorousCropGroupRegionCode)
        {
            return GetSoilTestPhosphorousRecommendations().SingleOrDefault(stp =>
                stp.SoilTestPhosphorousKelownaRangeId == stpKelownaRangeId &&
                stp.SoilTestPhosphorousRegionCode == soilTestPhosphorousRegionCode &&
                stp.PhosphorousCropGroupRegionCode == phosphorousCropGroupRegionCode);
        }

        public List<SelectListItem> GetSubtypesDll(int animalType)
        {
            var animalSubTypes = GetAnimalSubTypes();

            animalSubTypes = animalSubTypes.OrderBy(ast => ast.SortOrder).ThenBy(ast => ast.Name).ToList();

            List<SelectListItem> animalSubTypesOptions = new List<SelectListItem>();

            foreach (var r in animalSubTypes)
            {
                if (r.AnimalId == animalType)
                {
                    var li = new SelectListItem()
                        { Id = r.Id, Value = r.Name };
                    animalSubTypesOptions.Add(li);
                }
            }

            return animalSubTypesOptions;
        }

        public SubRegion GetSubRegion(int? subRegionId)
        {
            return GetSubRegions().SingleOrDefault(sr => sr.Id == subRegionId);
        }

        public List<SubRegion> GetSubRegions()
        {
            return _context.SubRegion.ToList();
        }

        public Unit GetUnit(string unitId)
        {
            return GetUnits().SingleOrDefault(u => u.Id == Convert.ToInt32(unitId));
        }

        public List<Unit> GetUnits()
        {
            return _context.Units.ToList();
        }

        public List<SelectListItem> GetUnitsDll(string unitType)
        {
            var units = GetUnits();

            List<SelectListItem> unitsOptions = new List<SelectListItem>();

            foreach (var r in units)
            {
                if (r.SolidLiquid.Equals(unitType, StringComparison.CurrentCultureIgnoreCase))
                {
                    var li = new SelectListItem()
                        { Id = r.Id, Value = r.Name };
                    unitsOptions.Add(li);
                }
            }

            return unitsOptions;
        }

        public string GetUserPrompt(string name)
        {
            return GetUserPrompts()
                .SingleOrDefault(up => up.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))?.Text;
        }

        public List<UserPrompt> GetUserPrompts()
        {
            return _context.UserPrompts.ToList();
        }

        public StaticDataVersion GetLatestVersionDataTree()
        {
            return _context.StaticDataVersions
                .OrderByDescending(v => v.Id)
                .Include(x => x.AmmoniaRetentions)
                .First();
        }

        public Yield GetYieldById(int yieldId)
        {
            return GetYields().SingleOrDefault(y => y.Id == yieldId);
        }

        public List<Yield> GetYields()
        {
            return _context.Yields.ToList();
        }

        public bool IsCropGrainsAndOilseeds(int cropType)
        {
            return (cropType == CROPTYPE_GRAINS_OILSEEDS_ID);
        }

        public bool IsCropHarvestYieldDefaultUnit(int selectedCropYieldUnit)
        {
            return (selectedCropYieldUnit == CROP_YIELD_DEFAULT_CALCULATION_UNIT);
        }

        public bool IsCustomFertilizer(int fertilizerTypeID)
        {
            return GetFertilizerTypes().Any(ft => ft.Id == fertilizerTypeID && ft.Custom);
        }

        public bool IsFertilizerTypeDry(int fertilizerTypeID)
        {
            return GetFertilizerTypes().Any(ft =>
                ft.Id == fertilizerTypeID && ft.DryLiquid.Equals("dry", StringComparison.CurrentCultureIgnoreCase));
        }

        public bool IsFertilizerTypeLiquid(int fertilizerTypeID)
        {
            return GetFertilizerTypes().Any(ft =>
                ft.Id == fertilizerTypeID && ft.DryLiquid.Equals("liquid", StringComparison.CurrentCultureIgnoreCase));
        }

        public bool IsManureClassCompostClassType(string manure_class)
        {
            return manure_class.Equals(MANURE_CLASS_COMPOST_BOOK, StringComparison.CurrentCultureIgnoreCase);
        }

        public bool IsManureClassCompostType(string manure_class)
        {
            return manure_class.Equals(MANURE_CLASS_COMPOST, StringComparison.CurrentCultureIgnoreCase);
        }

        public bool IsManureClassOtherType(string manure_class)
        {
            return manure_class.Equals(MANURE_CLASS_OTHER, StringComparison.CurrentCultureIgnoreCase);
        }

        public bool IsNitrateCreditApplicable(int? region, DateTime sampleDate, int yearOfAnalysis)
        {
            if ((region != null) && (sampleDate != null))
            {
                if (IsRegionInteriorBC(region))
                    return ((sampleDate >= GetInteriorNitrateSampleFromDt(yearOfAnalysis)) &&
                            (sampleDate <= GetInteriorNitrateSampleToDt(yearOfAnalysis)));
                else // coastal farm
                    return ((sampleDate >= GetCoastalNitrateSampleFromDt(yearOfAnalysis)) &&
                            (sampleDate <= GetCoastalNitrateSampleToDt(yearOfAnalysis)));
            }

            return false;
        }

        private DateTime GetInteriorNitrateSampleFromDt(int yearOfAnalysis)
        {
            var fromDate = _context.NitrateCreditSampleDates.Where(ncs =>
                ncs.Location.Equals("InteriorBC", StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault().FromDateMonth;

            return new DateTime(yearOfAnalysis - 1, Convert.ToInt16(fromDate), 01);
        }

        private DateTime GetInteriorNitrateSampleToDt(int yearOfAnalysis)
        {
            var toDate = _context.NitrateCreditSampleDates.Where(ncs =>
                    ncs.Location.Equals("InteriorBC", StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault().ToDateMonth;

            return new DateTime(yearOfAnalysis, Convert.ToInt16(toDate),
                DateTime.DaysInMonth(yearOfAnalysis, Convert.ToInt16(toDate)));
        }

        private DateTime GetCoastalNitrateSampleFromDt(int yearOfAnalysis)
        {
            var fromDate = _context.NitrateCreditSampleDates.Where(ncs =>
                    ncs.Location.Equals("CoastalBC", StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault().FromDateMonth;

            return new DateTime(yearOfAnalysis - 1, Convert.ToInt16(fromDate), 01);
        }

        private DateTime GetCoastalNitrateSampleToDt(int yearOfAnalysis)
        {
            var toDate = _context.NitrateCreditSampleDates.Where(ncs =>
                    ncs.Location.Equals("CoastalBC", StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault().ToDateMonth;

            return new DateTime(yearOfAnalysis, Convert.ToInt16(toDate),
                DateTime.DaysInMonth(yearOfAnalysis, Convert.ToInt16(toDate)));
        }


        public bool IsRegionInteriorBC(int? region)
        {
            return GetRegions().Any(r => region.HasValue && r.Id == region.Value && r.LocationId == GetInteriorId());
        }

        public string GetPotassiumSoilTestRating(decimal value)
        {
            return _context.PotassiumSoilTestRanges.FirstOrDefault(str => value >= str.LowerLimit && value <= str.UpperLimit)?.Rating ?? "Ukn";
        }

        public string GetPhosphorusSoilTestRating(decimal value)
        {
            return _context.PhosphorusSoilTestRanges.FirstOrDefault(str => value >= str.LowerLimit && value <= str.UpperLimit)?.Rating ?? "Ukn";
        }

        public List<PotassiumSoilTestRange> GetPotassiumSoilTestRanges()
        {
            return _context.PotassiumSoilTestRanges.ToList();
        }

        public List<PhosphorusSoilTestRange> GetPhosphorusSoilTestRanges()
        {
            return _context.PhosphorusSoilTestRanges.ToList();

        }

        public bool WasManureAddedInPreviousYear(string userSelectedPrevYearsManureAdded)
        {
            return _context.PrevManureApplicationYears
                       .First(pma => pma.FieldManureApplicationHistory == 0).Id != Convert.ToInt32(userSelectedPrevYearsManureAdded);
        }

        public List<MainMenu> GetMainMenus()
        {
            return _context.MainMenus
                .Include(x => x.SubMenus)
                .ToList();
        }

        public List<SelectListItem> GetMainMenusDll()
        {
            var mainMenus = GetMainMenus();

            List<SelectListItem> mainMenuOptions = new List<SelectListItem>();

            foreach (var r in mainMenus)
            {
                var li = new SelectListItem()
                    { Id = r.Id, Value = r.Name };
                mainMenuOptions.Add(li);
            }

            return mainMenuOptions;
        }

        public List<SubMenu> GetSubMenus()
        {
            return _context.MainMenus.SelectMany(mm => mm.SubMenus).ToList();
        }

        public List<SelectListItem> GetSubmenusDll()
        {
            var subMenus = GetSubMenus();

            subMenus = subMenus.OrderBy(n => n.Name).ToList();

            List<SelectListItem> subMenuoptions = new List<SelectListItem>();

            foreach (var r in subMenus)
            {
                var li = new SelectListItem()
                    { Id = r.Id, Value = r.Name };
                subMenuoptions.Add(li);
            }

            return subMenuoptions;
        }

        public List<StaticDataValidationMessages> ValidateRelationship(string childNode, string childfield,
            string parentNode, string parentfield)
        {
            //TODO: Will be depricated
            throw new NotImplementedException();
        }

        public ManureImportedDefault GetManureImportedDefault()
        {
            return _context.ManureImportedDefaults.First();
        }

        public List<SolidMaterialsConversionFactor> GetSolidMaterialsConversionFactors()
        {
            return _context.SolidMaterialsConversionFactors.ToList();
        }

        public List<LiquidMaterialsConversionFactor> GetLiquidMaterialsConversionFactors()
        {
            return _context.LiquidMaterialsConversionFactors.ToList();
        }
        public List<SolidMaterialApplicationTonPerAcreRateConversion> GetSolidMaterialApplicationTonPerAcreRateConversions()
        {
            return _context.SolidMaterialApplicationTonPerAcreRateConversions.ToList();
        }

        public List<LiquidMaterialApplicationUSGallonsPerAcreRateConversion> GetLiquidMaterialApplicationUSGallonsPerAcreRateConversion()
        {
            return _context.LiquidMaterialApplicationUsGallonsPerAcreRateConversions.ToList();
        }

        private string ParseStdUnit(string stdUnit)
        {
            int idx = stdUnit.LastIndexOf("/");
            if (idx > 0)
                stdUnit = stdUnit.Substring(0, idx);

            return stdUnit;
        }

        public List<Breed> GetBreeds()
        {
            return _context.Breed
                .Include(a => a.Animal)
                .ToList();
        }

        public List<SelectListItem> GetBreedsDll(int animalType)
        {
            var animalBreeds = GetBreeds();

            animalBreeds = animalBreeds.OrderBy(n => n.Id).ToList();

            List<SelectListItem> breedOptions = new List<SelectListItem>();

            foreach (var r in animalBreeds)
            {
                if (r.AnimalId == animalType)
                {
                    var li = new SelectListItem()
                        { Id = r.Id, Value = r.BreedName };
                    breedOptions.Add(li);
                }
            }

            return breedOptions;
        }

        public decimal GetBreedManureFactorByBreedId(int breedId)
        {
            return GetBreeds().SingleOrDefault(br => br.Id==breedId).BreedManureFactor;
        }

        public List<SelectListItem> GetBreed(int breedId)
        {
            var animalBreeds = GetBreeds();

            animalBreeds = animalBreeds.OrderBy(n => n.Id).ToList();

            List<SelectListItem> breedOptions = new List<SelectListItem>();

            foreach (var r in animalBreeds)
            {
                if (r.Id == breedId)
                {
                    var li = new SelectListItem()
                        { Id = r.Id, Value = r.BreedName };
                    breedOptions.Add(li);
                }
            }

            return breedOptions;
        }

        public LiquidSolidSeparationDefault GetLiquidSolidSeparationDefaults()
        {
            return _context.LiquidSolidSeparationDefaults.Single();
        }

        public MainMenu GetMainMenu(CoreSiteActions action)
        {
            return GetMainMenus()
                .SingleOrDefault(mm => mm.Action.Equals(action.ToString(), StringComparison.CurrentCultureIgnoreCase));
        }

        public List<SubMenu> GetSubMenus(int mainMenuId)
        {
            return GetSubMenus().Where(sb => sb.MainMenuId == mainMenuId).ToList();
        }

        public StaticDataVersion GetCurrentStaticDataVersion()
        {
            return _context.StaticDataVersions.OrderByDescending(sdv => sdv.Id).First();
        }

        public int ArchiveConfigurations()
        {
            var newId = 0;
            var currentVersion = GetLatestVersionDataTree();
            var datestamp = DateTime.Now;
            var nextId = currentVersion.Id + 1;
            var newVersion = new StaticDataVersion
            {
                Id = nextId,
                Version = $"{datestamp.Year}.{datestamp.DayOfYear}.{nextId}",
                //CreatedDateTime = datestamp
            };

            //var currentVersionData = 
            //Mapper.Initialize(cfg => cfg.CreateMap<AmmoniaRetention, AmmoniaRetention>());
            //var newAmmoniaRetentions =
            //    _mapper.Map<List<AmmoniaRetention>, List<AmmoniaRetention>>(currentVersion.AmmoniaRetentions).ToList();
            //newAmmoniaRetentions.Select(ar => {
            //    ar.Version = newVersion;
            //    ar.VersionId = newVersion.Id;
            //    return ar;
            //}).ToList(); 

            //newVersion.AmmoniaRetentions.AddRange(newAmmoniaRetentions);
            //_context.Versions.Add(newVersion);
            //_context.SaveChanges();
            return newId;
        }

    }
}

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
        private List<AmmoniaRetention> _ammoniaRetentions;
        private List<Animal> _animals;
        private BCSampleDateForNitrateCredit _bcSampleDateForNitrateCredit;
        private List<Breed> _breed;
        private List<Browser> _browsers;
        private ConversionFactor _conversionFactors;
        private List<Crop> _crops;
        private List<CropType> _cropTypes;
        private List<CropYield> _cropYields;
        private List<CropSoilTestPhosphorousRegion> _cropSoilTestPhosphorousRegions;
        private List<CropSoilTestPotassiumRegion> _cropSoilTestPotassiumRegions;
        private StaticDataVersion _currentStaticDataVersion;
        private DefaultSoilTest _defaultSoilTests;
        private List<DensityUnit> _densityUnits;
        private List<DryMatter> _dryMatters;
        private List<ExternalLink> _externalLinks;
        private List<FertilizerMethod> _fertilizerMethods;
        private List<Fertilizer> _fertilizers;
        private List<FertilizerType> _fertilizerTypes;
        private List<FertilizerUnit> _FertilizerUnits;
        private List<LiquidFertilizerDensity> _liquidFertilizerDensities;
        private List<HarvestUnit> _harvestUnits;
        private List<LiquidMaterialApplicationUSGallonsPerAcreRateConversion> _liquidMaterialApplicationUsGallonsPerAcreRateConversions;
        private List<LiquidMaterialsConversionFactor> _liquidMaterialsConversionFactors;
        private LiquidSolidSeparationDefault _liquidSolidSeparationDefaults;
        private List<Location> _locations;
        private List<MainMenu> _mainMenus;
        private List<Manure> _manures;
        private ManureImportedDefault _manureImportedDefault;
        private List<Message> _messages;
        private List<NitrateCreditSampleDate> _nitrateCreditSampleDates;
        private List<NitrogenMineralization> _nitrogenMineralizations;
        private List<NitrogenRecommendation> _nitrogenRecommendations;
        private List<NutrientIcon> _nutrientIcons;
        private List<PhosphorusSoilTestRange> _phosphorusSoilTestRanges;
        private List<PotassiumSoilTestRange> _potassiumSoilTestRanges;
        private List<PreviousManureApplicationYear> _prevManureApplicationYears;
        private List<PreviousYearManureApplicationNitrogenDefault> _prevYearManureApplicationNitrogenDefaults;
        private List<Region> _regions;
        private RptCompletedFertilizerRequiredStdUnit _rptCompletedFertilizerRequiredStdUnits;
        private RptCompletedManureRequiredStdUnit _rptCompletedManureRequiredStdUnits;
        private List<SeasonApplication> _seasonApplications;
        private List<SoilTestMethod> _soilTestMethods;
        private List<SoilTestPhosphorousKelownaRange> _soilTestPhosphorousKelownaRanges;
        private List<SoilTestPhosphorusRange> _soilTestPhosphorusRanges;
        private List<SoilTestPotassiumKelownaRange> _soilTestPotassiumKelownaRanges;
        private List<SoilTestPotassiumRange> _soilTestPotassiumRanges;
        private List<SolidMaterialApplicationTonPerAcreRateConversion> _solidMaterialApplicationTonPerAcreRateConversions;
        private List<SolidMaterialsConversionFactor> _solidMaterialsConversionFactors;
        private List<SubRegion> _subRegions;
        private List<Yield> _yields;
        private List<Unit> _units;
        private List<UserPrompt> _userPrompts;

        public AgriConfigurationRepository(AgriConfigurationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private int GetStaticDataVersionId()
        {
            return GetCurrentStaticDataVersion().Id;
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
            if (_browsers == null)
            {
                _browsers = _context.Browsers.AsNoTracking().ToList();
            }

            return _browsers;
        }

        public AmmoniaRetention GetAmmoniaRetention(int seasonApplicatonId, int dm)
        {
            return GetAmmoniaRetentions().SingleOrDefault(ar =>
                ar.SeasonApplicationId == seasonApplicatonId && ar.DryMatter == dm);
        }

        public List<AmmoniaRetention> GetAmmoniaRetentions()
        {
            if (_ammoniaRetentions == null)
            {
                _ammoniaRetentions = _context.AmmoniaRetentions.AsNoTracking()
                    .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                    .ToList();
            }

            return _ammoniaRetentions;
        }

        public Animal GetAnimal(int id)
        {
            return GetAnimals()
                .Where(a => a.StaticDataVersionId == GetStaticDataVersionId() && a.Id == id)
                .SingleOrDefault(); 
        }

        public List<Animal> GetAnimals()
        {
            if (_animals == null)
            {
                _animals = _context.Animals.AsNoTracking()
                    .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                    .Include(a => a.AnimalSubTypes)
                    .ToList();
            }

            return _animals;
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
            if (_seasonApplications == null)
            {
                _seasonApplications = _context.SeasonApplications.AsNoTracking()
                    .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                    .OrderBy(n => n.SortNum)
                    .ThenBy(n => n.Name)
                    .ToList();
            }

            return _seasonApplications;
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
            if (_bcSampleDateForNitrateCredit == null)
            {
                _bcSampleDateForNitrateCredit = _context.BCSampleDateForNitrateCredit.AsNoTracking()
                    .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                    .FirstOrDefault();
            }

            return _bcSampleDateForNitrateCredit;
        }

        public ConversionFactor GetConversionFactor()
        {
            if (_conversionFactors == null)
            {
                _conversionFactors = _context.ConversionFactors.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .FirstOrDefault();
            }

            return _conversionFactors;
        }

        public Crop GetCrop(int cropId)
        {
            return GetCrops()
                .Where(c => c.StaticDataVersionId == GetStaticDataVersionId() && c.Id == cropId)
                .SingleOrDefault();
        }

        public List<SelectListItem> GetCropHarvestUnitsDll()
        {
            if (_harvestUnits == null)
            {
                _harvestUnits = _context.HarvestUnits.AsNoTracking()
                    .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                    .ToList();
            }

            var harvestUnitsOptions = new List<SelectListItem>();
            foreach (var r in _harvestUnits)
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
            if (_crops == null)
            {
                _crops = _context.Crops.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .Include(c => c.CropYields)
                .Include(c => c.CropSoilTestPhosphorousRegions)
                .Include(c => c.CropSoilTestPotassiumRegions)
                .Include(c => c.PreviousCropTypes)
                .OrderBy(c => c.SortNumber)
                .ThenBy(n => n.CropName)
                .ToList();
            }

            return _crops;
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
            if (_cropSoilTestPhosphorousRegions == null)
            {
                _cropSoilTestPhosphorousRegions = _context.CropSoilTestPhosphorousRegions.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .ToList();
            }

            return _cropSoilTestPhosphorousRegions;
        }

        public List<CropSoilTestPotassiumRegion> GetCropSoilTestPotassiumRegions()
        {
            if (_cropSoilTestPotassiumRegions == null)
            {
                _cropSoilTestPotassiumRegions = _context.CropSoilTestPotassiumRegions.AsNoTracking()
                    .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                    .ToList();
            }

            return _cropSoilTestPotassiumRegions;
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
            if (_cropTypes == null)
            {
                _cropTypes = _context.CropTypes.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .ToList();
            }

            return _cropTypes;
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
            if (_cropYields == null)
            {
                _cropYields = _context.CropYields.AsNoTracking()
                    .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                    .ToList();
            }

            return _cropYields;
        }

        public DefaultSoilTest GetDefaultSoilTest()
        {
            if (_defaultSoilTests == null)
            {
                _defaultSoilTests = _context.DefaultSoilTests.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .FirstOrDefault();
            }

            return _defaultSoilTests;
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
            if (_densityUnits == null)
            {
                _densityUnits = _context.DensityUnits.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .ToList();
            }

            return _densityUnits;
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
            return GetDryMatters()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .SingleOrDefault(dm => dm.Id == ID);
        }

        public List<DryMatter> GetDryMatters()
        {
            if (_dryMatters == null)
            {
                _dryMatters = _context.DryMatters.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .ToList();
            }

            return _dryMatters;
        }

        public string GetExternalLink(string name)
        {
            return GetExternalLinks()
                .Where(el => el.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                .SingleOrDefault().Url;
        }

        public List<ExternalLink> GetExternalLinks()
        {
            if (_externalLinks == null)
            {
                _externalLinks = _context.ExternalLinks.AsNoTracking().ToList();
            }

            return _externalLinks;
        }

        public Fertilizer GetFertilizer(string id)
        {
            return GetFertilizers()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .SingleOrDefault(f => f.Id == Convert.ToInt32(id));
        }

        public FertilizerMethod GetFertilizerMethod(string id)
        {
            return GetFertilizerMethods().SingleOrDefault(fm => fm.Id == Convert.ToInt32(id));
        }

        public List<FertilizerMethod> GetFertilizerMethods()
        {
            if (_fertilizerMethods == null)
            {
                _fertilizerMethods = _context.FertilizerMethods.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .ToList().OrderBy(ni => ni.Id).ToList();
            }

            return _fertilizerMethods;
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
                var fertilizerUnitId = Convert.ToInt16(GetRptCompletedFertilizerRequiredStdUnit().SolidUnitId);
                stdUnit = GetFertilizerUnit(fertilizerUnitId).Name;
            }
            else
            {
                var fertilizerUnitId = Convert.ToInt16(GetRptCompletedFertilizerRequiredStdUnit().LiquidUnitId);
                stdUnit = GetFertilizerUnit(fertilizerUnitId).Name;
            }

            return ParseStdUnit(stdUnit);
        }

        public List<Fertilizer> GetFertilizers()
        {
            if (_fertilizers == null)
            {
                _fertilizers = _context.Fertilizers.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .OrderBy(f => f.SortNum)
                .ThenBy(f => f.Name)
                    .ToList();
            }

            return _fertilizers;
        }

        public List<SelectListItem> GetFertilizersDll(string fertilizerType)
        {
            var types = GetFertilizers();

            types = types.OrderBy(n => n.SortNum).ThenBy(n => n.Name).ToList();

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
            if (_fertilizerTypes == null)
            {
                _fertilizerTypes = _context.FertilizerTypes.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .ToList();
            }

            return _fertilizerTypes;
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
            if (_FertilizerUnits == null)
            {
                _FertilizerUnits = _context.FertilizerUnits.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .ToList();
            }

            return _FertilizerUnits;
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
            if (_harvestUnits == null)
            {
                _harvestUnits = _context.HarvestUnits.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .ToList();
            }

            return _harvestUnits;
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
            if (_liquidFertilizerDensities == null)
            {
                _liquidFertilizerDensities = _context.LiquidFertilizerDensities.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .ToList();
            }

            return _liquidFertilizerDensities;
        }

        public LiquidFertilizerDensity GetLiquidFertilizerDensity(int fertilizerId, int densityId)
        {
            return GetLiquidFertilizerDensities()
                .SingleOrDefault(lfd => lfd.FertilizerId == fertilizerId && lfd.DensityUnitId == densityId);
        }

        public List<Location> GetLocations()
        {
            if (_locations == null)
            {
                _locations = _context.Locations.AsNoTracking().ToList();
            }

            return _locations;
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
                
                   var unitId = GetRptCompletedManureRequiredStdUnit().SolidUnitId.ToString();
                stdUnit = GetUnit(unitId).Name;
            }
            else
            {
                var unitId = GetRptCompletedManureRequiredStdUnit().LiquidUnitId.ToString();
                stdUnit = GetUnit(unitId).Name;
            }

            return ParseStdUnit(stdUnit);
        }

        public List<Manure> GetManures()
        {
            if (_manures == null)
            {
                _manures = _context.Manures.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .OrderBy(m => m.SortNum)
                .ThenBy(n => n.Name)
                    .ToList();
            }

            return _manures;
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
                    !balanceMessages.Icon.Equals("stop", StringComparison.CurrentCultureIgnoreCase))
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
            if (_messages == null)
            {
                _messages = _context.Messages.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .ToList();
            }

            return _messages;
        }

        public decimal GetMilkProduction(int Id)
        {
            return GetAnimalSubTypes().SingleOrDefault(ast => ast.Id == Id).MilkProduction;
        }
        public List<NitrogenMineralization> GetNitrogeMineralizations()
        {
            if (_nitrogenMineralizations == null)
            {
                _nitrogenMineralizations = _context.NitrogenMineralizations.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .ToList();
            }

            return _nitrogenMineralizations;
        }

        public List<NitrogenRecommendation> GetNitrogenRecommendations()
        {
            if (_nitrogenRecommendations == null)
            {
                _nitrogenRecommendations = _context.NitrogenRecommendations.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .ToList();
            }

            return _nitrogenRecommendations;
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
            if (_nutrientIcons == null)
            {
                _nutrientIcons = _context.NutrientIcons.AsNoTracking().ToList().OrderBy(ni=>ni.Id).ToList();
            }

            return _nutrientIcons;
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
            return GetCrops()
                .SelectMany(c => c.PreviousCropTypes)
                .OrderBy(c=>c.Name)
                .ToList();
        }

        public List<PreviousManureApplicationYear> GetPrevManureApplicationInPrevYears()
        {
            if (_prevManureApplicationYears == null)
            {
                _prevManureApplicationYears = _context.PrevManureApplicationYears.AsNoTracking()
                    .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                    .OrderBy(p => p.FieldManureApplicationHistory)
                    .ToList();
            }

            return _prevManureApplicationYears;
        }

        public PreviousManureApplicationYear GetPrevManureApplicationInPrevYearsByManureAppHistory(int manureAppHistory)
        {
            return GetPrevManureApplicationInPrevYears()
                .Where(c => c.FieldManureApplicationHistory == manureAppHistory).First();
        }

        public List<PreviousYearManureApplicationNitrogenDefault> GetPrevYearManureNitrogenCreditDefaults()
        {
            if (_prevYearManureApplicationNitrogenDefaults == null)
            {
                _prevYearManureApplicationNitrogenDefaults = _context.PrevYearManureApplicationNitrogenDefaults.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .ToList();
            }

            return _prevYearManureApplicationNitrogenDefaults;
        }

        public Region GetRegion(int id)
        {
            return GetRegions().SingleOrDefault(r => r.Id == id);
        }

        public List<Region> GetRegions()
        {
            if (_regions == null)
            {
                _regions = _context.Regions.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .OrderBy(r => r.SortNumber)
                .ThenBy(n => n.Name)
                    .ToList();
            }

            return _regions;
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
            
            var subRegOptions = new List<SelectListItem>();
            
            foreach (var s in GetSubRegions())
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
            if (_rptCompletedFertilizerRequiredStdUnits == null)
            {
                _rptCompletedFertilizerRequiredStdUnits = _context.RptCompletedFertilizerRequiredStdUnits.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .FirstOrDefault();
            }

            return _rptCompletedFertilizerRequiredStdUnits;
        }

        public RptCompletedManureRequiredStdUnit GetRptCompletedManureRequiredStdUnit()
        {
            if (_rptCompletedManureRequiredStdUnits == null)
            {
                _rptCompletedManureRequiredStdUnits = _context.RptCompletedManureRequiredStdUnits.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .FirstOrDefault();
            }

            return _rptCompletedManureRequiredStdUnits;
        }

        public List<SeasonApplication> GetSeasonApplications()
        {
            if (_seasonApplications == null)
            {
                _seasonApplications = _context.SeasonApplications.AsNoTracking()
                    .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                    .OrderBy(sa => sa.SortNum).ToList();
            }
            return _seasonApplications;
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
            if (_soilTestMethods == null)
            {
                _soilTestMethods = _context.SoilTestMethods.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .OrderBy(stm => stm.SortNum)
                .ToList();
            }

            return _soilTestMethods;
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
            if (_soilTestPhosphorousKelownaRanges == null)
            {
                _soilTestPhosphorousKelownaRanges = _context.SoilTestPhosphorousKelownaRanges.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .Include(stpk => stpk.SoilTestPhosphorousRecommendations)
                .ToList();
            }

            return _soilTestPhosphorousKelownaRanges;
        }

        public List<SoilTestPhosphorousRecommendation> GetSoilTestPhosphorousRecommendations()
        {
            return GetSoilTestPhosphorousKelownaRanges().SelectMany(stp => stp.SoilTestPhosphorousRecommendations).ToList();
        }

        public List<SoilTestPhosphorusRange> GetSoilTestPhosphorusRanges()
        {
            if (_soilTestPhosphorusRanges == null)
            {
                _soilTestPhosphorusRanges = _context.SoilTestPhosphorusRanges.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .ToList();
            }

            return _soilTestPhosphorusRanges;
        }

        public List<SoilTestPotassiumKelownaRange> GetSoilTestPotassiumKelownaRanges()
        {
            if (_soilTestPotassiumKelownaRanges == null)
            {
                _soilTestPotassiumKelownaRanges = _context.SoilTestPotassiumKelownaRanges.AsNoTracking()
                    .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                    .Include(stpk => stpk.SoilTestPotassiumRecommendations)
                    .ToList();
            }

            return _soilTestPotassiumKelownaRanges;
        }

        public List<SoilTestPotassiumRange> GetSoilTestPotassiumRanges()
        {
            if (_soilTestPotassiumRanges == null)
            {
                _soilTestPotassiumRanges = _context.SoilTestPotassiumRanges.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .ToList();
            }

            return _soilTestPotassiumRanges;
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
            return GetCurrentStaticDataVersion().Id.ToString();
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
            if (_subRegions == null)
            {
                _subRegions = _context.SubRegion.AsNoTracking()
                    .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                    .ToList();
            }

            return _subRegions;
        }

        public Unit GetUnit(string unitId)
        {
            return GetUnits().SingleOrDefault(u => u.Id == Convert.ToInt32(unitId));
        }

        public List<Unit> GetUnits()
        {
            if (_units == null)
            {
                _units = _context.Units.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .ToList();
            }

            return _units;
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
            if (_userPrompts == null)
            {
                _userPrompts = _context.UserPrompts.AsNoTracking().ToList();
            }

            return _userPrompts;
        }

        public StaticDataVersion GetLatestVersionDataTree()
        {
            return _context.StaticDataVersions
                .OrderByDescending(v => v.Id)
                .Include(x => x.AmmoniaRetentions)
                .Include(x => x.Animals)
                .Include(x => x.AnimalSubTypes)
                .Include(x => x.BCSampleDateForNitrateCredits)
                .Include(x => x.Breeds)
                .Include(x => x.ConversionFactors)
                .Include(x => x.Crops)
                .Include(x => x.CropSoilTestPhosphorousRegions)
                .Include(x => x.CropSoilTestPotassiumRegions)
                .Include(x => x.CropTypes)
                .Include(x => x.CropYields)
                .Include(x => x.DefaultSoilTests)
                .Include(x => x.DensityUnits)
                .Include(x => x.DryMatters)
                .Include(x => x.Fertilizers)
                .Include(x => x.FertilizerMethods)
                .Include(x => x.FertilizerTypes)
                .Include(x => x.FertilizerUnits)
                .Include(x => x.HarvestUnits)
                .Include(x => x.LiquidFertilizerDensities)
                .Include(x => x.LiquidMaterialApplicationUsGallonsPerAcreRateConversions)
                .Include(x => x.LiquidMaterialsConversionFactors)
                .Include(x => x.LiquidSolidSeparationDefaults)
                .Include(x => x.ManureImportedDefaults)
                .Include(x => x.Manures)
                .Include(x => x.Messages)
                .Include(x => x.NitrateCreditSampleDates)
                .Include(x => x.NitrogenMineralizations)
                .Include(x => x.NitrogenRecommendations)
                .Include(x => x.PhosphorusSoilTestRanges)
                .Include(x => x.PotassiumSoilTestRanges)
                .Include(x => x.PreviousCropTypes)
                .Include(x => x.PrevManureApplicationYears)
                .Include(x => x.PrevYearManureApplicationNitrogenDefaults)
                .Include(x => x.Regions)
                .Include(x => x.RptCompletedFertilizerRequiredStdUnits)
                .Include(x => x.RptCompletedManureRequiredStdUnits)
                .Include(x => x.SeasonApplications)
                .Include(x => x.SoilTestMethods)
                .Include(x => x.SoilTestPhosphorusRanges)
                .Include(x => x.SoilTestPhosphorousKelownaRanges)
                .Include(x => x.SoilTestPhosphorousRecommendations)
                .Include(x => x.SoilTestPotassiumRanges)
                .Include(x => x.SoilTestPotassiumKelownaRanges)
                .Include(x => x.SoilTestPotassiumRecommendations)
                .Include(x => x.SolidMaterialApplicationTonPerAcreRateConversions)
                .Include(x => x.SolidMaterialsConversionFactors)
                .Include(x => x.Units)
                .Include(x => x.SubRegions)
                .Include(x => x.Yields)
                .First();
        }

        public Yield GetYieldById(int yieldId)
        {
            return GetYields().SingleOrDefault(y => y.Id == yieldId);
        }

        public List<Yield> GetYields()
        {
            if (_yields == null)
            {
                _yields = _context.Yields.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .ToList();
            }

            return _yields;
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

        private List<NitrateCreditSampleDate> GetNitrateCreditSampleDates()
        {
            if (_nitrateCreditSampleDates == null)
            {
                _nitrateCreditSampleDates = _context.NitrateCreditSampleDates.AsNoTracking().Where(ncs =>
                    ncs.StaticDataVersionId == GetStaticDataVersionId()).ToList();
            }

            return _nitrateCreditSampleDates;
        }

        private DateTime GetInteriorNitrateSampleFromDt(int yearOfAnalysis)
        {

            var fromDate = GetNitrateCreditSampleDates().Where(ncs =>
                ncs.Location.Equals("InteriorBC", StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault().FromDateMonth;

            return new DateTime(yearOfAnalysis - 1, Convert.ToInt16(fromDate), 01);
        }

        private DateTime GetInteriorNitrateSampleToDt(int yearOfAnalysis)
        {
            var toDate = GetNitrateCreditSampleDates().Where(ncs =>
                    ncs.Location.Equals("InteriorBC", StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault().ToDateMonth;

            return new DateTime(yearOfAnalysis, Convert.ToInt16(toDate),
                DateTime.DaysInMonth(yearOfAnalysis, Convert.ToInt16(toDate)));
        }

        private DateTime GetCoastalNitrateSampleFromDt(int yearOfAnalysis)
        {
            var fromDate = GetNitrateCreditSampleDates().Where(ncs =>
                    ncs.Location.Equals("CoastalBC", StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault().FromDateMonth;

            return new DateTime(yearOfAnalysis - 1, Convert.ToInt16(fromDate), 01);
        }

        private DateTime GetCoastalNitrateSampleToDt(int yearOfAnalysis)
        {
            var toDate = GetNitrateCreditSampleDates().Where(ncs =>
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
            return GetPotassiumSoilTestRanges()
                       .FirstOrDefault(str => value >= str.LowerLimit && value <= str.UpperLimit)?.Rating ?? "Ukn";
        }

        public string GetPhosphorusSoilTestRating(decimal value)
        {
            return GetPhosphorusSoilTestRanges()
                       .FirstOrDefault(str => value >= str.LowerLimit && value <= str.UpperLimit)?.Rating ?? "Ukn";
        }

        public List<PotassiumSoilTestRange> GetPotassiumSoilTestRanges()
        {
            if (_potassiumSoilTestRanges == null)
            {
                _potassiumSoilTestRanges = _context.PotassiumSoilTestRanges.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .ToList();
            }

            return _potassiumSoilTestRanges;
        }

        public List<PhosphorusSoilTestRange> GetPhosphorusSoilTestRanges()
        {
            if (_phosphorusSoilTestRanges == null)
            {
                _phosphorusSoilTestRanges = _context.PhosphorusSoilTestRanges.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .ToList();
            }

            return _phosphorusSoilTestRanges;

        }

        public bool WasManureAddedInPreviousYear(string userSelectedPrevYearsManureAdded)
        {
            return GetPrevManureApplicationInPrevYears()
                       .First(pma => pma.FieldManureApplicationHistory == 0)
                            .FieldManureApplicationHistory != Convert.ToInt32(userSelectedPrevYearsManureAdded);
        }

        public List<MainMenu> GetMainMenus()
        {
            if (_mainMenus == null)
            {
                _mainMenus = _context.MainMenus.AsNoTracking()
                .Include(x => x.SubMenus)
                .OrderBy(sm => sm.SortNumber)
                .ToList();
            }

            return _mainMenus;
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
            return GetMainMenus().SelectMany(mm => mm.SubMenus).ToList();
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
            if (_manureImportedDefault == null)
            {
                _manureImportedDefault = _context.ManureImportedDefaults.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .First();
            }

            return _manureImportedDefault;
        }

        public List<SolidMaterialsConversionFactor> GetSolidMaterialsConversionFactors()
        {
            if (_solidMaterialsConversionFactors == null)
            {
                _solidMaterialsConversionFactors = _context.SolidMaterialsConversionFactors.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .ToList();
            }

            return _solidMaterialsConversionFactors;
        }

        public List<LiquidMaterialsConversionFactor> GetLiquidMaterialsConversionFactors()
        {
            if (_liquidMaterialsConversionFactors == null)
            {
                _liquidMaterialsConversionFactors = _context.LiquidMaterialsConversionFactors.AsNoTracking()
                    .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                    .ToList();
            }

            return _liquidMaterialsConversionFactors;
        }

        public List<SolidMaterialApplicationTonPerAcreRateConversion> GetSolidMaterialApplicationTonPerAcreRateConversions()
        {
            if (_solidMaterialApplicationTonPerAcreRateConversions == null)
            {
                _solidMaterialApplicationTonPerAcreRateConversions = _context.SolidMaterialApplicationTonPerAcreRateConversions.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .ToList();
            }

            return _solidMaterialApplicationTonPerAcreRateConversions;
        }

        public List<LiquidMaterialApplicationUSGallonsPerAcreRateConversion> GetLiquidMaterialApplicationUSGallonsPerAcreRateConversion()
        {
            if (_liquidMaterialApplicationUsGallonsPerAcreRateConversions == null)
            {
                _liquidMaterialApplicationUsGallonsPerAcreRateConversions = _context.LiquidMaterialApplicationUsGallonsPerAcreRateConversions.AsNoTracking()
                    .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                    .ToList();
            }

            return _liquidMaterialApplicationUsGallonsPerAcreRateConversions;
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
            if (_breed == null)
            {
                _breed = _context.Breed.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .Include(a => a.Animal)
                .ToList();
            }

            return _breed;
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
            if (_liquidSolidSeparationDefaults == null)
            {
                _liquidSolidSeparationDefaults = _context.LiquidSolidSeparationDefaults.AsNoTracking()
                .Where(x => x.StaticDataVersionId == GetStaticDataVersionId())
                .Single();
            }

            return _liquidSolidSeparationDefaults;
        }

        public MainMenu GetMainMenu(CoreSiteActions action)
        {
            return GetMainMenus()
                .SingleOrDefault(mm => mm.Action.Equals(action.ToString(), StringComparison.CurrentCultureIgnoreCase));
        }

        public List<SubMenu> GetSubMenus(int mainMenuId)
        {
            return GetSubMenus()
                .Where(sb => sb.MainMenuId == mainMenuId)
                .OrderBy(sm => sm.SortNumber)
                .ToList();
        }

        public StaticDataVersion GetCurrentStaticDataVersion()
        {
            if (_currentStaticDataVersion == null)
            {
                _currentStaticDataVersion = _context.StaticDataVersions.AsNoTracking()
                .AsNoTracking()
                .OrderByDescending(sdv => sdv.Id).First();
            }

            return _currentStaticDataVersion;
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
                CreatedDateTime = datestamp
            };

            newVersion.AmmoniaRetentions = _mapper.Map<List<AmmoniaRetention>, List<AmmoniaRetention>>(currentVersion.AmmoniaRetentions).ToList();
            newVersion.Animals = _mapper.Map<List<Animal>, List<Animal>>(currentVersion.Animals).ToList();
            newVersion.AnimalSubTypes = _mapper.Map<List<AnimalSubType>, List<AnimalSubType>>(currentVersion.AnimalSubTypes).ToList();
            newVersion.BCSampleDateForNitrateCredits = _mapper.Map<List<BCSampleDateForNitrateCredit>, List<BCSampleDateForNitrateCredit>>(currentVersion.BCSampleDateForNitrateCredits).ToList();
            newVersion.Breeds = _mapper.Map<List<Breed>, List<Breed>>(currentVersion.Breeds).ToList();
            newVersion.ConversionFactors = _mapper.Map<List<ConversionFactor>, List<ConversionFactor>>(currentVersion.ConversionFactors).ToList();
            newVersion.Crops = _mapper.Map<List<Crop>, List<Crop>>(currentVersion.Crops).ToList();
            newVersion.CropSoilTestPhosphorousRegions = _mapper.Map<List<CropSoilTestPhosphorousRegion>, List<CropSoilTestPhosphorousRegion>>(currentVersion.CropSoilTestPhosphorousRegions).ToList();
            newVersion.CropSoilTestPotassiumRegions = _mapper.Map<List<CropSoilTestPotassiumRegion>, List<CropSoilTestPotassiumRegion>>(currentVersion.CropSoilTestPotassiumRegions).ToList();
            newVersion.CropTypes = _mapper.Map<List<CropType>, List<CropType>>(currentVersion.CropTypes).ToList();
            newVersion.CropYields = _mapper.Map<List<CropYield>, List<CropYield>>(currentVersion.CropYields).ToList();
            newVersion.DefaultSoilTests = _mapper.Map<List<DefaultSoilTest>, List<DefaultSoilTest>>(currentVersion.DefaultSoilTests).ToList();
            newVersion.DensityUnits = _mapper.Map<List<DensityUnit>, List<DensityUnit>>(currentVersion.DensityUnits).ToList();
            newVersion.DryMatters = _mapper.Map<List<DryMatter>, List<DryMatter>>(currentVersion.DryMatters).ToList();
            newVersion.Fertilizers = _mapper.Map<List<Fertilizer>, List<Fertilizer>>(currentVersion.Fertilizers).ToList();
            newVersion.FertilizerMethods = _mapper.Map<List<FertilizerMethod>, List<FertilizerMethod>>(currentVersion.FertilizerMethods).ToList();
            newVersion.FertilizerTypes = _mapper.Map<List<FertilizerType>, List<FertilizerType>>(currentVersion.FertilizerTypes).ToList();
            newVersion.FertilizerUnits = _mapper.Map<List<FertilizerUnit>, List<FertilizerUnit>>(currentVersion.FertilizerUnits).ToList();
            newVersion.HarvestUnits = _mapper.Map<List<HarvestUnit>, List<HarvestUnit>>(currentVersion.HarvestUnits).ToList();
            newVersion.LiquidFertilizerDensities = _mapper.Map<List<LiquidFertilizerDensity>, List<LiquidFertilizerDensity>>(currentVersion.LiquidFertilizerDensities).ToList();
            newVersion.LiquidMaterialApplicationUsGallonsPerAcreRateConversions = _mapper.Map<List<LiquidMaterialApplicationUSGallonsPerAcreRateConversion>, List<LiquidMaterialApplicationUSGallonsPerAcreRateConversion>>(currentVersion.LiquidMaterialApplicationUsGallonsPerAcreRateConversions).ToList();
            newVersion.LiquidMaterialsConversionFactors = _mapper.Map<List<LiquidMaterialsConversionFactor>, List<LiquidMaterialsConversionFactor>>(currentVersion.LiquidMaterialsConversionFactors).ToList();
            newVersion.LiquidSolidSeparationDefaults = _mapper.Map<List<LiquidSolidSeparationDefault>, List<LiquidSolidSeparationDefault>>(currentVersion.LiquidSolidSeparationDefaults).ToList();
            newVersion.ManureImportedDefaults = _mapper.Map<List<ManureImportedDefault>, List<ManureImportedDefault>>(currentVersion.ManureImportedDefaults).ToList();
            newVersion.Manures = _mapper.Map<List<Manure>, List<Manure>>(currentVersion.Manures).ToList();
            newVersion.Messages = _mapper.Map<List<Message>, List<Message>>(currentVersion.Messages).ToList();
            newVersion.NitrateCreditSampleDates = _mapper.Map<List<NitrateCreditSampleDate>, List<NitrateCreditSampleDate>>(currentVersion.NitrateCreditSampleDates).ToList();
            newVersion.NitrogenMineralizations = _mapper.Map<List<NitrogenMineralization>, List<NitrogenMineralization>>(currentVersion.NitrogenMineralizations).ToList();
            newVersion.NitrogenRecommendations = _mapper.Map<List<NitrogenRecommendation>, List<NitrogenRecommendation>>(currentVersion.NitrogenRecommendations).ToList();
            newVersion.PhosphorusSoilTestRanges = _mapper.Map<List<PhosphorusSoilTestRange>, List<PhosphorusSoilTestRange>>(currentVersion.PhosphorusSoilTestRanges).ToList();
            newVersion.PotassiumSoilTestRanges = _mapper.Map<List<PotassiumSoilTestRange>, List<PotassiumSoilTestRange>>(currentVersion.PotassiumSoilTestRanges).ToList();
            newVersion.PreviousCropTypes = _mapper.Map<List<PreviousCropType>, List<PreviousCropType>>(currentVersion.PreviousCropTypes).ToList();
            newVersion.PrevManureApplicationYears = _mapper.Map<List<PreviousManureApplicationYear>, List<PreviousManureApplicationYear>>(currentVersion.PrevManureApplicationYears).ToList();
            newVersion.PrevYearManureApplicationNitrogenDefaults = _mapper.Map<List<PreviousYearManureApplicationNitrogenDefault>, List<PreviousYearManureApplicationNitrogenDefault>>(currentVersion.PrevYearManureApplicationNitrogenDefaults).ToList();
            newVersion.Regions = _mapper.Map<List<Region>, List<Region>>(currentVersion.Regions).ToList();
            newVersion.RptCompletedFertilizerRequiredStdUnits = _mapper.Map<List<RptCompletedFertilizerRequiredStdUnit>, List<RptCompletedFertilizerRequiredStdUnit>>(currentVersion.RptCompletedFertilizerRequiredStdUnits).ToList();
            newVersion.RptCompletedManureRequiredStdUnits = _mapper.Map<List<RptCompletedManureRequiredStdUnit>, List<RptCompletedManureRequiredStdUnit>>(currentVersion.RptCompletedManureRequiredStdUnits).ToList();
            newVersion.SeasonApplications = _mapper.Map<List<SeasonApplication>, List<SeasonApplication>>(currentVersion.SeasonApplications).ToList();
            newVersion.SoilTestMethods = _mapper.Map<List<SoilTestMethod>, List<SoilTestMethod>>(currentVersion.SoilTestMethods).ToList();
            newVersion.SoilTestPhosphorusRanges = _mapper.Map<List<SoilTestPhosphorusRange>, List<SoilTestPhosphorusRange>>(currentVersion.SoilTestPhosphorusRanges).ToList();
            newVersion.SoilTestPhosphorousKelownaRanges = _mapper.Map<List<SoilTestPhosphorousKelownaRange>, List<SoilTestPhosphorousKelownaRange>>(currentVersion.SoilTestPhosphorousKelownaRanges).ToList();
            newVersion.SoilTestPhosphorousRecommendations = _mapper.Map<List<SoilTestPhosphorousRecommendation>, List<SoilTestPhosphorousRecommendation>>(currentVersion.SoilTestPhosphorousRecommendations).ToList();
            newVersion.SoilTestPotassiumRanges = _mapper.Map<List<SoilTestPotassiumRange>, List<SoilTestPotassiumRange>>(currentVersion.SoilTestPotassiumRanges).ToList();
            newVersion.SoilTestPotassiumKelownaRanges = _mapper.Map<List<SoilTestPotassiumKelownaRange>, List<SoilTestPotassiumKelownaRange>>(currentVersion.SoilTestPotassiumKelownaRanges).ToList();
            newVersion.SoilTestPotassiumRecommendations = _mapper.Map<List<SoilTestPotassiumRecommendation>, List<SoilTestPotassiumRecommendation>>(currentVersion.SoilTestPotassiumRecommendations).ToList();
            newVersion.SolidMaterialApplicationTonPerAcreRateConversions = _mapper.Map<List<SolidMaterialApplicationTonPerAcreRateConversion>, List<SolidMaterialApplicationTonPerAcreRateConversion>>(currentVersion.SolidMaterialApplicationTonPerAcreRateConversions).ToList();
            newVersion.SolidMaterialsConversionFactors = _mapper.Map<List<SolidMaterialsConversionFactor>, List<SolidMaterialsConversionFactor>>(currentVersion.SolidMaterialsConversionFactors).ToList();
            newVersion.Units = _mapper.Map<List<Unit>, List<Unit>>(currentVersion.Units).ToList();
            newVersion.SubRegions = _mapper.Map<List<SubRegion>, List<SubRegion>>(currentVersion.SubRegions).ToList();
            newVersion.Yields = _mapper.Map<List<Yield>, List<Yield>>(currentVersion.Yields).ToList();

            newVersion.AmmoniaRetentions.ForEach(n => n.SetVersion(newVersion));
            newVersion.Animals.ForEach(n => n.SetVersion(newVersion));
            newVersion.AnimalSubTypes.ForEach(n => n.SetVersion(newVersion));
            newVersion.BCSampleDateForNitrateCredits.ForEach(n => n.SetVersion(newVersion));
            newVersion.Breeds.ForEach(n => n.SetVersion(newVersion));
            newVersion.ConversionFactors.ForEach(n => n.SetVersion(newVersion));
            newVersion.Crops.ForEach(n => n.SetVersion(newVersion));
            newVersion.CropSoilTestPhosphorousRegions.ForEach(n => n.SetVersion(newVersion));
            newVersion.CropSoilTestPotassiumRegions.ForEach(n => n.SetVersion(newVersion));
            newVersion.CropTypes.ForEach(n => n.SetVersion(newVersion));
            newVersion.CropYields.ForEach(n => n.SetVersion(newVersion));
            newVersion.DefaultSoilTests.ForEach(n => n.SetVersion(newVersion));
            newVersion.DensityUnits.ForEach(n => n.SetVersion(newVersion));
            newVersion.DryMatters.ForEach(n => n.SetVersion(newVersion));
            newVersion.Fertilizers.ForEach(n => n.SetVersion(newVersion));
            newVersion.FertilizerMethods.ForEach(n => n.SetVersion(newVersion));
            newVersion.FertilizerTypes.ForEach(n => n.SetVersion(newVersion));
            newVersion.FertilizerUnits.ForEach(n => n.SetVersion(newVersion));
            newVersion.HarvestUnits.ForEach(n => n.SetVersion(newVersion));
            newVersion.LiquidFertilizerDensities.ForEach(n => n.SetVersion(newVersion));
            newVersion.LiquidMaterialApplicationUsGallonsPerAcreRateConversions.ForEach(n => n.SetVersion(newVersion));
            newVersion.LiquidMaterialsConversionFactors.ForEach(n => n.SetVersion(newVersion));
            newVersion.LiquidSolidSeparationDefaults.ForEach(n => n.SetVersion(newVersion));
            newVersion.ManureImportedDefaults.ForEach(n => n.SetVersion(newVersion));
            newVersion.Manures.ForEach(n => n.SetVersion(newVersion));
            newVersion.Messages.ForEach(n => n.SetVersion(newVersion));
            newVersion.NitrateCreditSampleDates.ForEach(n => n.SetVersion(newVersion));
            newVersion.NitrogenMineralizations.ForEach(n => n.SetVersion(newVersion));
            newVersion.NitrogenRecommendations.ForEach(n => n.SetVersion(newVersion));
            newVersion.PhosphorusSoilTestRanges.ForEach(n => n.SetVersion(newVersion));
            newVersion.PotassiumSoilTestRanges.ForEach(n => n.SetVersion(newVersion));
            newVersion.PreviousCropTypes.ForEach(n => n.SetVersion(newVersion));
            newVersion.PrevManureApplicationYears.ForEach(n => n.SetVersion(newVersion));
            newVersion.PrevYearManureApplicationNitrogenDefaults.ForEach(n => n.SetVersion(newVersion));
            newVersion.Regions.ForEach(n => n.SetVersion(newVersion));
            newVersion.RptCompletedFertilizerRequiredStdUnits.ForEach(n => n.SetVersion(newVersion));
            newVersion.RptCompletedManureRequiredStdUnits.ForEach(n => n.SetVersion(newVersion));
            newVersion.SeasonApplications.ForEach(n => n.SetVersion(newVersion));
            newVersion.SoilTestMethods.ForEach(n => n.SetVersion(newVersion));
            newVersion.SoilTestPhosphorusRanges.ForEach(n => n.SetVersion(newVersion));
            newVersion.SoilTestPhosphorousKelownaRanges.ForEach(n => n.SetVersion(newVersion));
            newVersion.SoilTestPhosphorousRecommendations.ForEach(n => n.SetVersion(newVersion));
            newVersion.SoilTestPotassiumRanges.ForEach(n => n.SetVersion(newVersion));
            newVersion.SoilTestPotassiumKelownaRanges.ForEach(n => n.SetVersion(newVersion));
            newVersion.SoilTestPotassiumRecommendations.ForEach(n => n.SetVersion(newVersion));
            newVersion.SolidMaterialApplicationTonPerAcreRateConversions.ForEach(n => n.SetVersion(newVersion));
            newVersion.SolidMaterialsConversionFactors.ForEach(n => n.SetVersion(newVersion));
            newVersion.Units.ForEach(n => n.SetVersion(newVersion));
            newVersion.SubRegions.ForEach(n => n.SetVersion(newVersion));
            newVersion.Yields.ForEach(n => n.SetVersion(newVersion));

            _context.StaticDataVersions.Add(newVersion);
            _context.SaveChanges();
            return newId;
        }

    }
}

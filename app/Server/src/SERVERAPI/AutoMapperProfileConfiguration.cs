using Agri.Models;
using Agri.Models.Configuration;
using Agri.Models.Farm;
using AutoMapper;
using SERVERAPI.ViewModels;

namespace SERVERAPI
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
            : this("MyProfile")
        {
        }

        protected AutoMapperProfileConfiguration(string profileName)
            : base(profileName)
        {
            CreateMap<AmmoniaRetention, AmmoniaRetention>();
            CreateMap<Animal, Animal>()
                .ForMember(x => x.AnimalSubTypes, opt => opt.Ignore())
                .ForMember(x => x.Breeds, opt => opt.Ignore());
            CreateMap<AnimalSubType, AnimalSubType>()
                .ForMember(x => x.Animal, opt => opt.Ignore())
                .ForMember(x => x.StaticDataVersionId, opt => opt.Ignore())
                .ForMember(x => x.Version, opt => opt.Ignore());
            CreateMap<BCSampleDateForNitrateCredit, BCSampleDateForNitrateCredit>();
            CreateMap<Breed, Breed>()
                .ForMember(x => x.Animal, opt => opt.Ignore());
            CreateMap<ConversionFactor, ConversionFactor>();
            CreateMap<Crop, Crop>()
                .ForMember(x => x.CropSoilTestPhosphorousRegions, opt => opt.Ignore())
                .ForMember(x => x.CropSoilTestPotassiumRegions, opt => opt.Ignore())
                .ForMember(x => x.CropType, opt => opt.Ignore())
                .ForMember(x => x.CropYields, opt => opt.Ignore())
                .ForMember(x => x.PreviousYearManureApplicationNitrogenDefault, opt => opt.Ignore())
                .ForMember(x => x.PreviousCropTypes, opt => opt.Ignore());
            CreateMap<CropSoilTestPhosphorousRegion, CropSoilTestPhosphorousRegion>()
                .ForMember(x => x.Crop, opt => opt.Ignore());
            CreateMap<CropSoilTestPotassiumRegion, CropSoilTestPotassiumRegion>()
                .ForMember(x => x.Crop, opt => opt.Ignore());
            CreateMap<CropType, CropType>()
                .ForMember(x => x.Crops, opt => opt.Ignore())
                .ForMember(x => x.PrevCropTypes, opt => opt.Ignore());
            CreateMap<CropYield, CropYield>()
                .ForMember(x => x.Crop, opt => opt.Ignore())
                .ForMember(x => x.Location, opt => opt.Ignore());
            CreateMap<DefaultSoilTest, DefaultSoilTest>();
            CreateMap<DensityUnit, DensityUnit>()
                .ForMember(x => x.LiquidFertilizerDensities, opt => opt.Ignore());
            CreateMap<DryMatter, DryMatter>()
                .ForMember(x => x.Manures, opt => opt.Ignore());
            CreateMap<Fertilizer, Fertilizer>()
                .ForMember(x => x.LiquidFertilizerDensities, opt => opt.Ignore());
            CreateMap<FertilizerMethod, FertilizerMethod>();
            CreateMap<FertilizerType, FertilizerType>();
            CreateMap<FertilizerUnit, FertilizerUnit>();
            CreateMap<HarvestUnit, HarvestUnit>();
            CreateMap<LiquidFertilizerDensity, LiquidFertilizerDensity>()
                .ForMember(x => x.Fertilizer, opt => opt.Ignore())
                .ForMember(x => x.DensityUnit, opt => opt.Ignore());
            CreateMap<LiquidMaterialApplicationUSGallonsPerAcreRateConversion, LiquidMaterialApplicationUSGallonsPerAcreRateConversion>();
            CreateMap<LiquidMaterialsConversionFactor, LiquidMaterialsConversionFactor>();
            CreateMap<LiquidSolidSeparationDefault, LiquidSolidSeparationDefault>();
            CreateMap<ManureImportedDefault, ManureImportedDefault>();
            CreateMap<Manure, Manure>()
                .ForMember(x => x.DryMatter, opt => opt.Ignore());
            CreateMap<Message, Message>();
            CreateMap<NitrateCreditSampleDate, NitrateCreditSampleDate>();
            CreateMap<NitrogenMineralization, NitrogenMineralization>()
                .ForMember(x => x.Location, opt => opt.Ignore());
            CreateMap<NitrogenRecommendation, NitrogenRecommendation>();
            CreateMap<PhosphorusSoilTestRange, PhosphorusSoilTestRange>();
            CreateMap<PotassiumSoilTestRange, PotassiumSoilTestRange>();
            CreateMap<PreviousCropType, PreviousCropType>();
            CreateMap<PreviousManureApplicationYear, PreviousManureApplicationYear>()
                .ForMember(x => x.PreviousYearManureApplicationNitrogenDefaults, opt => opt.Ignore());
            CreateMap<PreviousYearManureApplicationNitrogenDefault, PreviousYearManureApplicationNitrogenDefault>()
                .ForMember(x => x.Crops, opt => opt.Ignore())
                .ForMember(x => x.PreviousManureApplicationYear, opt => opt.Ignore());
            CreateMap<Region, Region>()
                .ForMember(x => x.Location, opt => opt.Ignore())
                .ForMember(x => x.SubRegions, opt => opt.Ignore());
            CreateMap<RptCompletedFertilizerRequiredStdUnit, RptCompletedFertilizerRequiredStdUnit>();
            CreateMap<RptCompletedManureRequiredStdUnit, RptCompletedManureRequiredStdUnit>();
            CreateMap<SeasonApplication, SeasonApplication>();
            CreateMap<SoilTestMethod, SoilTestMethod>();
            CreateMap<SoilTestPhosphorusRange, SoilTestPhosphorusRange>();
            CreateMap<SoilTestPhosphorousKelownaRange, SoilTestPhosphorousKelownaRange>()
                .ForMember(x => x.SoilTestPhosphorousRecommendations, opt => opt.Ignore());
            CreateMap<SoilTestPhosphorousRecommendation, SoilTestPhosphorousRecommendation>()
                .ForMember(x => x.SoilTestPhosphorousKelownaRange, opt => opt.Ignore());
            CreateMap<SoilTestPotassiumRange, SoilTestPotassiumRange>();
            CreateMap<SoilTestPotassiumKelownaRange, SoilTestPotassiumKelownaRange>()
                .ForMember(x => x.SoilTestPotassiumRecommendations, opt => opt.Ignore());
            CreateMap<SoilTestPotassiumRecommendation, SoilTestPotassiumRecommendation>()
                .ForMember(x => x.SoilTestPotassiumKelownaRange, opt => opt.Ignore());
            CreateMap<SolidMaterialApplicationTonPerAcreRateConversion, SolidMaterialApplicationTonPerAcreRateConversion>();
            CreateMap<SolidMaterialsConversionFactor, SolidMaterialsConversionFactor>();
            CreateMap<Unit, Unit>();
            CreateMap<SubRegion, SubRegion>()
                .ForMember(x => x.Region, opt => opt.Ignore()); ;
            CreateMap<Yield, Yield>();

            CreateMap<ManureStorageSystem, ManureStorageSystem>();
            CreateMap<ManureImportedDetailViewModel, ImportedManure>()
                .ForMember(dest => dest.Id, x => x.MapFrom(src => src.ManureImportId))
                .ForMember(dest => dest.ManureType, x => x.MapFrom(src => src.SelectedManureType))
                .ForMember(dest => dest.ManureTypeName, x => x.MapFrom(src => EnumHelper<ManureMaterialType>.GetDisplayValue(src.SelectedManureType)))
                .ForMember(dest => dest.Units, x => x.MapFrom(src => src.SelectedAnnualAmountUnit))
                .ReverseMap();
            CreateMap<ImportedManure, ImportedManure>();
            CreateMap<UserPrompt, UserPrompt>();
            CreateMap<SeparatedSolidManure, SeparatedSolidManure>();
            CreateMap<SubRegion, SubRegion>();

            CreateMap<StaticDataVersion, StaticDataVersion>();
        }
    }
}
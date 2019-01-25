using Agri.LegacyData.Models;
using Agri.Models.Configuration;
using Agri.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Version = Agri.Models.Configuration.Version;

namespace Agri.Data
{
    public class AgriConfigurationContext : DbContext
    {
        public AgriConfigurationContext(DbContextOptions<AgriConfigurationContext> options) : base (options)
        {
        }

        #region DbSets 
        public DbSet<AmmoniaRetention> AmmoniaRetentions { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<AnimalSubType> AnimalSubType { get; set; }
        public DbSet<BCSampleDateForNitrateCredit> BCSampleDateForNitrateCredit { get; set; }
        public DbSet<Breed> Breed { get; set; }
        public DbSet<Browser> Browsers { get; set; }
        public DbSet<ConversionFactor> ConversionFactors { get; set; }
        public DbSet<Crop> Crops { get; set; }
        public DbSet<CropYield> CropYields { get; set; }
        public DbSet<CropSoilTestPotassiumRegion> CropSoilTestPotassiumRegions { get; set; }
        public DbSet<CropSoilTestPhosphorousRegion> CropSoilTestPhosphorousRegions { get; set; }
        public DbSet<CropType> CropTypes { get; set; }
        public DbSet<DefaultSoilTest> DefaultSoilTests { get; set; }
        public DbSet<DensityUnit> DensityUnits { get; set; }
        public DbSet<DryMatter> DryMatters { get; set; }
        public DbSet<ExternalLink> ExternalLinks { get; set; }
        public DbSet<Fertilizer> Fertilizers { get; set; }
        public DbSet<FertilizerMethod> FertilizerMethods { get; set; }
        public DbSet<FertilizerType> FertilizerTypes { get; set; }
        public DbSet<FertilizerUnit> FertilizerUnits { get; set; }
        public DbSet<HarvestUnit> HarvestUnits { get; set; }
        public DbSet<LiquidFertilizerDensity> LiquidFertilizerDensities { get; set; }
        public DbSet<LiquidMaterialApplicationUSGallonsPerAcreRateConversion> LiquidMaterialApplicationUsGallonsPerAcreRateConversions { get; set; }
        public DbSet<LiquidMaterialsConversionFactor> LiquidMaterialsConversionFactors { get; set; }
        public DbSet<LiquidSolidSeparationDefault> LiquidSolidSeparationDefaults { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<MainMenu> MainMenus { get; set; }
        public DbSet<ManureImportedDefault> ManureImportedDefaults { get; set; }
        public DbSet<Manure> Manures { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<NitrateCreditSampleDate> NitrateCreditSampleDates { get; set; }
        public DbSet<NitrogenMineralization> NitrogenMineralizations { get; set; }
        public DbSet<NutrientIcon> NutrientIcons { get; set; }
        public DbSet<NitrogenRecommendation> NitrogenRecommendations { get; set; }
        public DbSet<PhosphorusSoilTestRange> PhosphorusSoilTestRanges { get; set; }
        public DbSet<PotassiumSoilTestRange> PotassiumSoilTestRanges { get; set; }
        public DbSet<PreviousManureApplicationYear> PrevManureApplicationYears { get; set; }
        public DbSet<PreviousYearManureApplicationNitrogenDefault> PrevYearManureApplicationNitrogenDefaults { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<SubRegion> SubRegion { get; set; }
        public DbSet<RptCompletedFertilizerRequiredStdUnit> RptCompletedFertilizerRequiredStdUnits { get; set; }
        public DbSet<RptCompletedManureRequiredStdUnit> RptCompletedManureRequiredStdUnits { get; set; }
        public DbSet<SeasonApplication> SeasonApplications { get; set; }
        public DbSet<SelectCodeItem> SelectCodeItems { get; set; }
        public DbSet<SelectListItem> SelectListItems { get; set; }
        public DbSet<SoilTestMethod> SoilTestMethods { get; set; }
        public DbSet<SoilTestPhosphorusRange> SoilTestPhosphorusRanges { get; set; }
        public DbSet<SoilTestPotassiumRange> SoilTestPotassiumRanges { get; set; }
        public DbSet<SoilTestPotassiumKelownaRange> SoilTestPotassiumKelownaRanges { get; set; }
        public DbSet<SoilTestPhosphorousKelownaRange> SoilTestPhosphorousKelownaRanges { get; set; }
        public DbSet<SolidMaterialApplicationTonPerAcreRateConversion> SolidMaterialApplicationTonPerAcreRateConversions { get; set; }
        public DbSet<SolidMaterialsConversionFactor> SolidMaterialsConversionFactors { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<UserPrompt> UserPrompts { get; set; }
        public DbSet<Version> Versions { get; set; }
        public DbSet<Yield> Yields { get; set; }

        public DbSet<AppliedMigrationSeedData> AppliedMigrationSeedData { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Composite Primary Key Definitions
            modelBuilder.Entity<AmmoniaRetention>()
                .HasKey(table => new
                {
                    table.SeasonApplicationId,
                    DM = table.DryMatter
                });

            modelBuilder.Entity<CropSoilTestPotassiumRegion>()
                .HasKey(table => new
                {
                    table.CropId,
                    table.SoilTestPotassiumRegionCode
                });
            
            modelBuilder.Entity<CropSoilTestPhosphorousRegion>()
                .HasKey(table => new
                {
                    table.CropId,
                    table.SoilTestPhosphorousRegionCode
                });

            modelBuilder.Entity<CropYield>()
                .HasKey(table => new {table.CropId, table.LocationId});

            modelBuilder.Entity<NitrogenMineralization>()
                .HasKey(table => new
                {
                    table.Id,
                    Locationid = table.LocationId
                });

            modelBuilder.Entity<SoilTestPotassiumRecommendation>()
                .HasKey(table => new
                {
                    table.SoilTestPotassiumKelownaRangeId,
                    table.SoilTestPotassiumRegionCode,
                    table.PotassiumCropGroupRegionCode
                });

            modelBuilder.Entity<SoilTestPhosphorousRecommendation>()
                .HasKey(table => new
                {
                    table.SoilTestPhosphorousKelownaRangeId,
                    table.SoilTestPhosphorousRegionCode,
                    table.PhosphorousCropGroupRegionCode
                });

            //Foreign Keys
            modelBuilder.Entity<Crop>()
                .HasOne(crop => crop.PreviousYearManureApplicationNitrogenDefault)
                .WithMany(p => p.Crops)
                .HasForeignKey(crop => crop.ManureApplicationHistory)
                .HasPrincipalKey(p => p.FieldManureApplicationHistory);

            modelBuilder.Entity<PreviousYearManureApplicationNitrogenDefault>()
                .HasOne(p => p.PreviousManureApplicationYear)
                .WithMany(manure => manure.PreviousYearManureApplicationNitrogenDefaults)
                .HasForeignKey(p => p.FieldManureApplicationHistory)
                .HasPrincipalKey(manure => manure.FieldManureApplicationHistory);
        }
    }
}

using Agri.LegacyData.Models;
using Agri.Models.Configuration;
using Microsoft.EntityFrameworkCore;
using Version = Agri.Models.Configuration.Version;

namespace Agri.Data
{
    public class AgriConfigurationContext : DbContext
    {
        private string connectionString;

        public AgriConfigurationContext(DbContextOptions<AgriConfigurationContext> options) : base (options)
        {
        }

        public AgriConfigurationContext(DbContextOptions<AgriConfigurationContext> options, 
            string connectionString) : base(options)
        {
            this.connectionString = connectionString;
        }

        #region DbSets 
        public DbSet<Agri.Models.Configuration.AmmoniaRetention> AmmoniaRetentions { get; set; }
        public DbSet<Agri.Models.Configuration.Animal> Animals { get; set; }
        public DbSet<Agri.Models.Configuration.BCSampleDateForNitrateCredit> BCSampleDateForNitrateCredit { get; set; }
        public DbSet<Agri.Models.Configuration.Browser> Browsers { get; set; }
        public DbSet<Agri.Models.Configuration.ConversionFactor> ConversionFactors { get; set; }
        public DbSet<Agri.Models.Configuration.Crop> Crops { get; set; }
        public DbSet<Agri.Models.Configuration.CropYield> CropYields { get; set; }
        public DbSet<Agri.Models.Configuration.CropSoilTestPotassiumRegion> CropSoilTestPotassiumRegions { get; set; }
        public DbSet<Agri.Models.Configuration.CropSoilTestPhosphorousRegion> CropSoilTestPhosphorousRegions { get; set; }
        public DbSet<Agri.Models.Configuration.CropType> CropTypes { get; set; }
        public DbSet<Agri.Models.Configuration.DefaultSoilTest> DefaultSoilTests { get; set; }
        public DbSet<Agri.Models.Configuration.DensityUnit> DensityUnits { get; set; }
        public DbSet<Agri.Models.Configuration.DryMatter> DryMatters { get; set; }
        public DbSet<Agri.Models.Configuration.ExternalLink> ExternalLinks { get; set; }
        public DbSet<Agri.Models.Configuration.Fertilizer> Fertilizers { get; set; }
        public DbSet<Agri.Models.Configuration.FertilizerMethod> FertilizerMethods { get; set; }
        public DbSet<Agri.Models.Configuration.FertilizerType> FertilizerTypes { get; set; }
        public DbSet<Agri.Models.Configuration.FertilizerUnit> FertilizerUnits { get; set; }
        public DbSet<Agri.Models.Configuration.HarvestUnit> HarvestUnits { get; set; }
        public DbSet<Agri.Models.Configuration.LiquidFertilizerDensity> LiquidFertilizerDensities { get; set; }
        public DbSet<Agri.Models.Configuration.Location> Locations { get; set; }
        public DbSet<Agri.Models.Configuration.Manure> Manures { get; set; }
        public DbSet<Agri.Models.Configuration.Message> Messages { get; set; }
        public DbSet<Agri.Models.Configuration.NitrogenMineralization> NitrogenMineralizations { get; set; }
        public DbSet<Agri.Models.Configuration.NutrientIcon> NutrientIcons { get; set; }
        public DbSet<Agri.Models.Configuration.NitrogenRecommendation> NitrogenRecommendations { get; set; }
        public DbSet<Agri.Models.Configuration.PreviousManureApplicationYear> PrevManureApplicationYears { get; set; }
        public DbSet<Agri.Models.Configuration.PreviousYearManureApplicationNitrogenDefault> PrevYearManureApplicationNitrogenDefaults { get; set; }
        public DbSet<Agri.Models.Configuration.Region> Regions { get; set; }
        public DbSet<Agri.Models.Configuration.RptCompletedFertilizerRequiredStdUnit> RptCompletedFertilizerRequiredStdUnits { get; set; }
        public DbSet<Agri.Models.Configuration.RptCompletedManureRequiredStdUnit> RptCompletedManureRequiredStdUnits { get; set; }
        public DbSet<Agri.Models.Configuration.SeasonApplication> SeasonApplications { get; set; }
        public DbSet<Agri.Models.Configuration.SelectCodeItem> SelectCodeItems { get; set; }
        public DbSet<Agri.Models.Configuration.SelectListItem> SelectListItems { get; set; }
        public DbSet<Agri.Models.Configuration.SoilTestMethod> SoilTestMethods { get; set; }
        public DbSet<Agri.Models.Configuration.SoilTestPhosphorusRange> SoilTestPhosphorusRanges { get; set; }
        public DbSet<Agri.Models.Configuration.SoilTestPotassiumRange> SoilTestPotassiumRanges { get; set; }
        public DbSet<Agri.Models.Configuration.SoilTestPotassiumKelownaRange> SoilTestPotassiumKelownaRanges { get; set; }
        public DbSet<Agri.Models.Configuration.SoilTestPhosphorousKelownaRange> SoilTestPhosphorousKelownaRanges { get; set; }
        public DbSet<Agri.Models.Configuration.Unit> Units { get; set; }
        public DbSet<Agri.Models.Configuration.UserPrompt> UserPrompts { get; set; }
        public DbSet<Agri.Models.Configuration.Version> Versions { get; set; }
        public DbSet<Agri.Models.Configuration.Yield> Yields { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseNpgsql();

            base.OnConfiguring(optionsBuilder);
        }

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agri.Models;
using Microsoft.EntityFrameworkCore;
using Version = Agri.Models.Version;

namespace Agri.Data
{
    public class AgriConfigurationContext : DbContext
    {
        public AgriConfigurationContext(DbContextOptions<AgriConfigurationContext> options) : base (options)
        {
        }

        public DbSet<AmmoniaRetention> AmmoniaRetentions { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Browser> Browsers { get; set; }
        public DbSet<ConversionFactor> ConversionFactors { get; set; }
        public DbSet<Crop> Crops { get; set; }
        public DbSet<DefaultSoilTest> DefaultSoilTests { get; set; }
        public DbSet<ExternalLink> ExternalLinks { get; set; }
        public DbSet<FertilizerMethod> FertilizerMethods { get; set; }
        public DbSet<FertilizerType> FertilizerTypes { get; set; }
        public DbSet<FertilizerUnit> FertilizerUnits { get; set; }
        public DbSet<LiquidFertilizerDensity> LiquidFertilizerDensities { get; set; }
        public DbSet<Manure> Manures { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<NutrientIcon> NutrientIcons { get; set; }
        public DbSet<PrevManureApplicationYear> PrevManureApplicationYears { get; set; }
        public DbSet<PrevYearManureApplDefaultNitrogen> PrevYearManureApplDefaultNitrogens { get; set; }
        public DbSet<SeasonApplication> SeasonApplications { get; set; }
        public DbSet<SelectCodeItem> SelectCodeItems { get; set; }
        public DbSet<SelectListItem> SelectListItems { get; set; }
        public DbSet<SoilTestMethod> SoilTestMethods { get; set; }
        public DbSet<SoilTestPhosphorousRange> SoilTestPhosphorousRanges { get; set; }
        public DbSet<SoilTestPotassiumRange> SoilTestPotassiumRanges { get; set; }
        public DbSet<STKKelownaRange> STKKelownaRanges { get; set; }
        public DbSet<STPKelownaRange> STPKelownaRanges { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Version> Versions { get; set; }
        public DbSet<Yield> Yields { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Composite Primary Key Definitions
            modelBuilder.Entity<CropSTKRegionCd>().HasKey(table => new {table.CropId, table.PotassiumCropGroupRegionCd});
            modelBuilder.Entity<CropSTPRegionCd>().HasKey(table => new {table.CropId, table.PhosphorousCropGroupRegionCd});
            modelBuilder.Entity<CropYield>().HasKey(table => new {table.CropId, table.LocationId});

            modelBuilder.Entity<AmmoniaRetention>().HasData();
        }
    }
}

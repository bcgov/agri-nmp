using System;
using Agri.Models.Configuration;
using Agri.Models.Data;
using Agri.Models.Security;
using Microsoft.EntityFrameworkCore;

namespace Agri.Data
{
    public class AgriConfigurationContext : DbContext
    {
        public AgriConfigurationContext(DbContextOptions<AgriConfigurationContext> options) : base(options)
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
        public DbSet<DailyFeedRequirement> DailyFeedRequirements { get; set; }
        public DbSet<DefaultSoilTest> DefaultSoilTests { get; set; }
        public DbSet<DensityUnit> DensityUnits { get; set; }
        public DbSet<DryMatter> DryMatters { get; set; }
        public DbSet<ExternalLink> ExternalLinks { get; set; }
        public DbSet<FeedEfficiency> FeedEfficiencies { get; set; }
        public DbSet<Fertilizer> Fertilizers { get; set; }
        public DbSet<FertilizerMethod> FertilizerMethods { get; set; }
        public DbSet<FertilizerType> FertilizerTypes { get; set; }
        public DbSet<FertilizerUnit> FertilizerUnits { get; set; }
        public DbSet<HarvestUnit> HarvestUnits { get; set; }
        public DbSet<Journey> Journeys { get; set; }
        public DbSet<LiquidFertilizerDensity> LiquidFertilizerDensities { get; set; }
        public DbSet<LiquidMaterialApplicationUSGallonsPerAcreRateConversion> LiquidMaterialApplicationUsGallonsPerAcreRateConversions { get; set; }
        public DbSet<LiquidMaterialsConversionFactor> LiquidMaterialsConversionFactors { get; set; }
        public DbSet<LiquidSolidSeparationDefault> LiquidSolidSeparationDefaults { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<MainMenu> MainMenus { get; set; }
        public DbSet<Manure> Manures { get; set; }
        public DbSet<ManureImportedDefault> ManureImportedDefaults { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MiniApp> MiniApps { get; set; }
        public DbSet<MiniAppLabel> MiniAppLabels { get; set; }
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
        public DbSet<SoilTestMethod> SoilTestMethods { get; set; }
        public DbSet<SoilTestPhosphorusRange> SoilTestPhosphorusRanges { get; set; }
        public DbSet<SoilTestPotassiumRange> SoilTestPotassiumRanges { get; set; }
        public DbSet<SoilTestPotassiumKelownaRange> SoilTestPotassiumKelownaRanges { get; set; }
        public DbSet<SoilTestPhosphorousKelownaRange> SoilTestPhosphorousKelownaRanges { get; set; }
        public DbSet<SolidMaterialApplicationTonPerAcreRateConversion> SolidMaterialApplicationTonPerAcreRateConversions { get; set; }
        public DbSet<SolidMaterialsConversionFactor> SolidMaterialsConversionFactors { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<UserPrompt> UserPrompts { get; set; }
        public DbSet<StaticDataVersion> StaticDataVersions { get; set; }
        public DbSet<Yield> Yields { get; set; }
        public DbSet<ManageVersionUser> ManageVersionUsers { get; set; }

        #endregion DbSets

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Composite Primary Key Definitions
            modelBuilder.Entity<AmmoniaRetention>()
                .HasKey(table => new
                {
                    table.SeasonApplicationId,
                    table.DryMatter,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<Animal>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<AnimalSubType>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<BCSampleDateForNitrateCredit>()
                .HasKey(table => new
                {
                    table.CoastalFromDateMonth,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<Breed>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<ConversionFactor>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<Crop>(b =>
            {
                b.HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });
            });

            modelBuilder.Entity<CropSoilTestPotassiumRegion>()
                .HasKey(table => new
                {
                    table.CropId,
                    table.SoilTestPotassiumRegionCode,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<CropSoilTestPhosphorousRegion>()
                .HasKey(table => new
                {
                    table.CropId,
                    table.SoilTestPhosphorousRegionCode,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<CropType>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<CropYield>()
                .HasKey(table => new
                {
                    table.CropId,
                    table.LocationId,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<DailyFeedRequirement>()
               .HasKey(table => new
               {
                   table.Id,
                   table.StaticDataVersionId
               });

            modelBuilder.Entity<DefaultSoilTest>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<DensityUnit>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<DryMatter>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<FeedEfficiency>()
              .HasKey(table => new
              {
                  table.Id,
                  table.StaticDataVersionId
              });

            modelBuilder.Entity<Fertilizer>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<FertilizerMethod>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<FertilizerType>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<FertilizerUnit>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<HarvestUnit>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<Journey>().ToTable("Journey");

            modelBuilder.Entity<LiquidFertilizerDensity>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<LiquidMaterialApplicationUSGallonsPerAcreRateConversion>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<LiquidMaterialsConversionFactor>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<LiquidSolidSeparationDefault>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<Manure>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<ManureImportedDefault>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<Message>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<NitrateCreditSampleDate>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<NitrogenMineralization>()
                .HasKey(table => new
                {
                    table.Id,
                    table.LocationId,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<NitrogenRecommendation>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<PhosphorusSoilTestRange>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<PotassiumSoilTestRange>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<PreviousCropType>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<PreviousManureApplicationYear>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<PreviousYearManureApplicationNitrogenDefault>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<Region>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<RptCompletedFertilizerRequiredStdUnit>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<RptCompletedManureRequiredStdUnit>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<SeasonApplication>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<SolidMaterialsConversionFactor>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<SoilTestMethod>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<SoilTestPhosphorousKelownaRange>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<SoilTestPhosphorousRecommendation>()
                .HasKey(table => new
                {
                    table.SoilTestPhosphorousKelownaRangeId,
                    table.SoilTestPhosphorousRegionCode,
                    table.PhosphorousCropGroupRegionCode,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<SoilTestPhosphorusRange>()
                .HasKey(table => new
                {
                    table.UpperLimit,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<SoilTestPotassiumKelownaRange>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<SoilTestPotassiumRange>()
                .HasKey(table => new
                {
                    table.UpperLimit,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<SoilTestPotassiumRecommendation>()
                .HasKey(table => new
                {
                    table.SoilTestPotassiumKelownaRangeId,
                    table.SoilTestPotassiumRegionCode,
                    table.PotassiumCropGroupRegionCode,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<SolidMaterialApplicationTonPerAcreRateConversion>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<SubRegion>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<Unit>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            modelBuilder.Entity<Yield>()
                .HasKey(table => new
                {
                    table.Id,
                    table.StaticDataVersionId
                });

            //Foreign Keys
            modelBuilder.Entity<AnimalSubType>(b =>
            {
                b.HasOne(child => child.Version)
                    .WithMany(version => version.AnimalSubTypes)
                    .HasForeignKey(child => child.StaticDataVersionId)
                    .HasPrincipalKey(version => version.Id);

                b.HasOne(subtype => subtype.Animal)
                    .WithMany(animal => animal.AnimalSubTypes)
                    .HasForeignKey(subtype => new { subtype.AnimalId, subtype.StaticDataVersionId })
                    .HasPrincipalKey(animal => new { animal.Id, animal.StaticDataVersionId });
            });

            modelBuilder.Entity<Breed>(b =>
            {
                b.HasOne(child => child.Version)
                    .WithMany(version => version.Breeds)
                    .HasForeignKey(child => child.StaticDataVersionId)
                    .HasPrincipalKey(version => version.Id);

                b.HasOne(breed => breed.Animal)
                    .WithMany(animal => animal.Breeds)
                    .HasForeignKey(breed => new { breed.AnimalId, breed.StaticDataVersionId })
                    .HasPrincipalKey(animal => new { animal.Id, animal.StaticDataVersionId });
            });

            modelBuilder.Entity<Crop>(b =>
            {
                b.HasOne(child => child.Version)
                    .WithMany(version => version.Crops)
                    .HasForeignKey(child => child.StaticDataVersionId)
                    .HasPrincipalKey(version => version.Id);

                b.HasOne(crop => crop.PreviousYearManureApplicationNitrogenDefault)
                    .WithMany(p => p.Crops)
                    .HasForeignKey(crop => new { crop.ManureApplicationHistory, crop.StaticDataVersionId })
                    .HasPrincipalKey(p => new { p.FieldManureApplicationHistory, p.StaticDataVersionId });

                b.HasOne(crop => crop.CropType)
                    .WithMany(p => p.Crops)
                    .HasForeignKey(crop => new { crop.CropTypeId, crop.StaticDataVersionId })
                    .HasPrincipalKey(p => new { p.Id, p.StaticDataVersionId });
            });

            modelBuilder.Entity<CropYield>(b =>
            {
                b.HasOne(child => child.Version)
                    .WithMany(version => version.CropYields)
                    .HasForeignKey(child => child.StaticDataVersionId)
                    .HasPrincipalKey(version => version.Id);

                b.HasOne(cropYield => cropYield.Crop)
                    .WithMany(crop => crop.CropYields)
                    .HasForeignKey(cropYield => new { cropYield.CropId, cropYield.StaticDataVersionId })
                    .HasPrincipalKey(crop => new { crop.Id, crop.StaticDataVersionId });

                b.HasOne(cropYield => cropYield.Location)
                    .WithMany(locaction => locaction.CropYields)
                    .HasForeignKey(cropYield => cropYield.LocationId)
                    .HasPrincipalKey(location => location.Id);
            });

            modelBuilder.Entity<CropSoilTestPhosphorousRegion>(b =>
            {
                b.HasOne(child => child.Version)
                    .WithMany(version => version.CropSoilTestPhosphorousRegions)
                    .HasForeignKey(child => child.StaticDataVersionId)
                    .HasPrincipalKey(version => version.Id);

                b.HasOne(test => test.Crop)
                    .WithMany(crop => crop.CropSoilTestPhosphorousRegions)
                    .HasForeignKey(test => new { test.CropId, test.StaticDataVersionId })
                    .HasPrincipalKey(crop => new { crop.Id, crop.StaticDataVersionId });
            });

            modelBuilder.Entity<CropSoilTestPotassiumRegion>(b =>
            {
                b.HasOne(child => child.Version)
                    .WithMany(version => version.CropSoilTestPotassiumRegions)
                    .HasForeignKey(child => child.StaticDataVersionId)
                    .HasPrincipalKey(version => version.Id);

                b.HasOne(test => test.Crop)
                    .WithMany(crop => crop.CropSoilTestPotassiumRegions)
                    .HasForeignKey(test => new { test.CropId, test.StaticDataVersionId })
                    .HasPrincipalKey(crop => new { crop.Id, crop.StaticDataVersionId });
            });

            modelBuilder.Entity<LiquidFertilizerDensity>(b =>
            {
                b.HasOne(child => child.Version)
                    .WithMany(version => version.LiquidFertilizerDensities)
                    .HasForeignKey(child => child.StaticDataVersionId)
                    .HasPrincipalKey(version => version.Id);

                b.HasOne(density => density.Fertilizer)
                    .WithMany(fertilizer => fertilizer.LiquidFertilizerDensities)
                    .HasForeignKey(density => new { density.FertilizerId, density.StaticDataVersionId })
                    .HasPrincipalKey(fertilizer => new { fertilizer.Id, fertilizer.StaticDataVersionId });

                b.HasOne(density => density.DensityUnit)
                    .WithMany(denisty => denisty.LiquidFertilizerDensities)
                    .HasForeignKey(density => new { density.DensityUnitId, density.StaticDataVersionId })
                    .HasPrincipalKey(denisty => new { denisty.Id, denisty.StaticDataVersionId });
            });

            modelBuilder.Entity<Manure>(b =>
            {
                b.HasOne(child => child.Version)
                    .WithMany(version => version.Manures)
                    .HasForeignKey(child => child.StaticDataVersionId)
                    .HasPrincipalKey(version => version.Id);

                b.HasOne(manure => manure.DryMatter)
                    .WithMany(dm => dm.Manures)
                    .HasForeignKey(manure => new { manure.DryMatterId, manure.StaticDataVersionId })
                    .HasPrincipalKey(dm => new { dm.Id, dm.StaticDataVersionId });

                //This join is forcing the creation of a Unique Constraint, which isn't possible with
                //this current schema and would require too much refactoring.
                //This is a known issue now, thus the values for NMineralizationId need
                //to be manually ensured as existing in the NitrogenMineralization table
                //b.HasMany(manure => manure.NMineralization)
                //    .WithMany(nm => nm.Manures)
                //    .HasForeignKey(manure => new { manure.NMineralizationId, manure.StaticDataVersionId })
                //    .HasPrincipalKey(nm => new { nm.Id, nm.StaticDataVersionId });
            });

            modelBuilder.Entity<NitrogenMineralization>(b =>
            {
                b.HasOne(child => child.Version)
                    .WithMany(version => version.NitrogenMineralizations)
                    .HasForeignKey(child => child.StaticDataVersionId)
                    .HasPrincipalKey(version => version.Id);

                b.HasOne(nm => nm.Location)
                    .WithMany(location => location.NitrogenMineralizations)
                    .HasForeignKey(nm => nm.LocationId)
                    .HasPrincipalKey(location => location.Id);
            });

            modelBuilder.Entity<PreviousCropType>(b =>
            {
                b.HasOne(child => child.Version)
                    .WithMany(version => version.PreviousCropTypes)
                    .HasForeignKey(child => child.StaticDataVersionId)
                    .HasPrincipalKey(version => version.Id);

                b.HasOne(pct => pct.Crop)
                    .WithMany(crop => crop.PreviousCropTypes)
                    .HasForeignKey(pct => new { pct.CropId, pct.PreviousCropCode, pct.StaticDataVersionId })
                    .HasPrincipalKey(crop => new { crop.Id, crop.PreviousCropCode, crop.StaticDataVersionId });
            });

            modelBuilder.Entity<PreviousYearManureApplicationNitrogenDefault>(b =>
        {
            b.HasOne(child => child.Version)
                .WithMany(version => version.PrevYearManureApplicationNitrogenDefaults)
                .HasForeignKey(child => child.StaticDataVersionId)
                .HasPrincipalKey(version => version.Id);

            b.HasOne(p => p.PreviousManureApplicationYear)
                .WithMany(manure => manure.PreviousYearManureApplicationNitrogenDefaults)
                .HasForeignKey(p => new { p.FieldManureApplicationHistory, p.StaticDataVersionId })
                .HasPrincipalKey(manure => new { manure.FieldManureApplicationHistory, manure.StaticDataVersionId });
        });

            modelBuilder.Entity<Region>(b =>
            {
                b.HasOne(child => child.Version)
                    .WithMany(version => version.Regions)
                    .HasForeignKey(child => child.StaticDataVersionId)
                    .HasPrincipalKey(version => version.Id);

                b.HasOne(region => region.Location)
                    .WithMany(location => location.Regions)
                    .HasForeignKey(region => region.LocationId)
                    .HasPrincipalKey(location => location.Id);
            });

            modelBuilder.Entity<SoilTestPhosphorousRecommendation>(b =>
            {
                b.HasOne(child => child.Version)
                    .WithMany(version => version.SoilTestPhosphorousRecommendations)
                    .HasForeignKey(child => child.StaticDataVersionId)
                    .HasPrincipalKey(version => version.Id);

                b.HasOne(test => test.SoilTestPhosphorousKelownaRange)
                    .WithMany(range => range.SoilTestPhosphorousRecommendations)
                    .HasForeignKey(test => new { test.SoilTestPhosphorousKelownaRangeId, test.StaticDataVersionId })
                    .HasPrincipalKey(range => new { range.Id, range.StaticDataVersionId });
            });

            modelBuilder.Entity<SoilTestPotassiumRecommendation>(b =>
            {
                b.HasOne(child => child.Version)
                    .WithMany(version => version.SoilTestPotassiumRecommendations)
                    .HasForeignKey(child => child.StaticDataVersionId)
                    .HasPrincipalKey(version => version.Id);

                b.HasOne(test => test.SoilTestPotassiumKelownaRange)
                    .WithMany(range => range.SoilTestPotassiumRecommendations)
                    .HasForeignKey(test => new { test.SoilTestPotassiumKelownaRangeId, test.StaticDataVersionId })
                    .HasPrincipalKey(range => new { range.Id, range.StaticDataVersionId });
            });

            modelBuilder.Entity<SubMenu>(b =>
            {
                b.HasOne(subMenu => subMenu.MainMenu)
                    .WithMany(main => main.SubMenus)
                    .HasForeignKey(subMenu => subMenu.MainMenuId)
                    .HasPrincipalKey(main => main.Id);
            });

            modelBuilder.Entity<SubRegion>(b =>
            {
                b.HasOne(child => child.Version)
                    .WithMany(version => version.SubRegions)
                    .HasForeignKey(child => child.StaticDataVersionId)
                    .HasPrincipalKey(version => version.Id);

                b.HasOne(subRegion => subRegion.Region)
                    .WithMany(region => region.SubRegions)
                    .HasForeignKey(subRegion => new { subRegion.RegionId, subRegion.StaticDataVersionId })
                    .HasPrincipalKey(region => new { region.Id, region.StaticDataVersionId });
            });

            //Default Values
            modelBuilder.Entity<AmmoniaRetention>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<Animal>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<AnimalSubType>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<BCSampleDateForNitrateCredit>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<Breed>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<ConversionFactor>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<Crop>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<CropSoilTestPhosphorousRegion>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<CropSoilTestPotassiumRegion>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<CropType>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<CropYield>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<DailyFeedRequirement>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<DefaultSoilTest>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<DensityUnit>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<DryMatter>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<Fertilizer>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<FertilizerMethod>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<FertilizerType>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<FertilizerUnit>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<HarvestUnit>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<LiquidFertilizerDensity>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<LiquidMaterialApplicationUSGallonsPerAcreRateConversion>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<LiquidMaterialsConversionFactor>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<LiquidSolidSeparationDefault>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<Manure>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<ManureImportedDefault>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<Message>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<NitrateCreditSampleDate>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<NitrogenMineralization>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<NitrogenRecommendation>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<PhosphorusSoilTestRange>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<PotassiumSoilTestRange>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<PreviousCropType>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<PreviousManureApplicationYear>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<PreviousYearManureApplicationNitrogenDefault>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<Region>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<RptCompletedFertilizerRequiredStdUnit>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<RptCompletedManureRequiredStdUnit>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<SeasonApplication>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<SoilTestMethod>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<SoilTestPhosphorousKelownaRange>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<SoilTestPhosphorousRecommendation>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<SoilTestPhosphorusRange>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<SoilTestPotassiumKelownaRange>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<SoilTestPotassiumRange>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<SoilTestPotassiumRecommendation>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<SolidMaterialApplicationTonPerAcreRateConversion>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<SolidMaterialsConversionFactor>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<SubRegion>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<Unit>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);
            modelBuilder.Entity<Yield>().Property(x => x.StaticDataVersionId).HasDefaultValue(1);

            modelBuilder.Entity<StaticDataVersion>()
                .Property(s => s.CreatedDateTime)
                .HasDefaultValueSql("NOW()");

            //Unique Columns
            modelBuilder.Entity<ManageVersionUser>()
                .HasIndex(table => table.UserName)
                .IsUnique();

            modelBuilder.Entity<UserPrompt>()
                .HasIndex(table => table.Name)
                .IsUnique();

            if (!this.Database.IsNpgsql())
            {
                modelBuilder.Entity<PreviousYearManureApplicationNitrogenDefault>()
                    .Property(e => e.DefaultNitrogenCredit)
                    .HasConversion(v => new ArrayWrapper(v), v => v.Values);
            }
        }

        // Note this makes the mapper happy, but won't be called in 2.1.
        private struct ArrayWrapper
        {
            public ArrayWrapper(int[] values) => Values = values;

            public int[] Values { get; }
        }
    }
}
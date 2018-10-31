﻿// <auto-generated />
using System;
using Agri.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SERVERAPI.Migrations
{
    [DbContext(typeof(AgriConfigurationContext))]
    [Migration("20181031034809_AmmoninaRetentionsCompositeKey")]
    partial class AmmoninaRetentionsCompositeKey
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Agri.Models.StaticData.AmmoniaRetention", b =>
                {
                    b.Property<int>("SeasonApplicationId");

                    b.Property<int>("DM");

                    b.Property<decimal?>("Value");

                    b.HasKey("SeasonApplicationId", "DM");

                    b.HasAlternateKey("DM", "SeasonApplicationId");

                    b.ToTable("AmmoniaRetentions");
                });

            modelBuilder.Entity("Agri.Models.StaticData.Animal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Animals");
                });

            modelBuilder.Entity("Agri.Models.StaticData.AnimalSubType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AnimalId");

                    b.Property<decimal>("LiquidPerGalPerAnimalPerDay");

                    b.Property<string>("Name");

                    b.Property<decimal>("SolidLiquidSeparationPercentage");

                    b.Property<decimal>("SolidPerGalPerAnimalPerDay");

                    b.Property<decimal>("SolidPerPoundPerAnimalPerDay");

                    b.HasKey("Id");

                    b.HasIndex("AnimalId");

                    b.ToTable("AnimalSubType");
                });

            modelBuilder.Entity("Agri.Models.StaticData.Browser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("MinVersion");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Browsers");
                });

            modelBuilder.Entity("Agri.Models.StaticData.ConversionFactor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DefaultApplicationOfManureInPrevYears");

                    b.Property<int>("DefaultSoilTestKelownaK");

                    b.Property<int>("DefaultSoilTestKelownaP");

                    b.Property<decimal>("KgPerHa_lbPerAc_Conversion");

                    b.Property<decimal>("NProteinConversion");

                    b.Property<decimal>("PhosphorousAvailabilityFirstYear");

                    b.Property<decimal>("PhosphorousAvailabilityLongTerm");

                    b.Property<decimal>("PhosphorousPtoP2O5KConversion");

                    b.Property<decimal>("PotassiumAvailabilityFirstYear");

                    b.Property<decimal>("PotassiumAvailabilityLongTerm");

                    b.Property<decimal>("PotassiumKtoK2Oconversion");

                    b.Property<decimal>("UnitConversion");

                    b.Property<decimal>("lbPer1000ftSquared_lbPerAc_Conversion");

                    b.Property<decimal>("lbPerTonConversion");

                    b.HasKey("Id");

                    b.ToTable("ConversionFactors");
                });

            modelBuilder.Entity("Agri.Models.StaticData.Crop", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CropName");

                    b.Property<decimal?>("CropRemovalFactorK2O");

                    b.Property<decimal?>("CropRemovalFactorP2O5");

                    b.Property<decimal?>("CropRemovalFactor_N");

                    b.Property<int>("CropTypeId");

                    b.Property<decimal?>("HarvestBushelsPerTon");

                    b.Property<decimal?>("N_High_lbPerAc");

                    b.Property<decimal>("N_RecommCd");

                    b.Property<decimal?>("N_Recomm_lbPerAc");

                    b.Property<int>("PrevCropCd");

                    b.Property<int>("PrevYearManureAppl_VolCatCd");

                    b.Property<int>("SortNum");

                    b.Property<int>("YieldCd");

                    b.HasKey("Id");

                    b.HasIndex("CropTypeId");

                    b.ToTable("Crops");
                });

            modelBuilder.Entity("Agri.Models.StaticData.CropSTKRegionCd", b =>
                {
                    b.Property<int>("CropId");

                    b.Property<int?>("PotassiumCropGroupRegionCd");

                    b.Property<int>("SoilTestPotassiumRegionCd");

                    b.HasKey("CropId", "PotassiumCropGroupRegionCd");

                    b.HasAlternateKey("CropId", "SoilTestPotassiumRegionCd");

                    b.ToTable("CropSTKRegionCd");
                });

            modelBuilder.Entity("Agri.Models.StaticData.CropSTPRegionCd", b =>
                {
                    b.Property<int>("CropId");

                    b.Property<int?>("PhosphorousCropGroupRegionCd");

                    b.Property<int>("SoilTestPhosphorousRegionCd");

                    b.HasKey("CropId", "PhosphorousCropGroupRegionCd");

                    b.HasAlternateKey("CropId", "SoilTestPhosphorousRegionCd");

                    b.ToTable("CropSTPRegionCd");
                });

            modelBuilder.Entity("Agri.Models.StaticData.CropType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("CoverCrop");

                    b.Property<bool>("CrudeProteinRequired");

                    b.Property<bool>("CustomCrop");

                    b.Property<bool>("ModifyNitrogen");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("CropType");
                });

            modelBuilder.Entity("Agri.Models.StaticData.CropYield", b =>
                {
                    b.Property<int>("CropId");

                    b.Property<int>("LocationId");

                    b.Property<decimal?>("Amt");

                    b.HasKey("CropId", "LocationId");

                    b.HasIndex("LocationId");

                    b.ToTable("CropYield");
                });

            modelBuilder.Entity("Agri.Models.StaticData.DefaultSoilTest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ConvertedKelownaK");

                    b.Property<int>("ConvertedKelownaP");

                    b.Property<decimal>("Nitrogen");

                    b.Property<decimal>("Phosphorous");

                    b.Property<decimal>("Potassium");

                    b.Property<decimal>("pH");

                    b.HasKey("Id");

                    b.ToTable("DefaultSoilTests");
                });

            modelBuilder.Entity("Agri.Models.StaticData.DensityUnit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("ConvFactor");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("DensityUnit");
                });

            modelBuilder.Entity("Agri.Models.StaticData.DM", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("DM");
                });

            modelBuilder.Entity("Agri.Models.StaticData.ExternalLink", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.ToTable("ExternalLinks");
                });

            modelBuilder.Entity("Agri.Models.StaticData.Fertilizer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DryLiquid");

                    b.Property<string>("Name");

                    b.Property<decimal>("Nitrogen");

                    b.Property<decimal>("Phosphorous");

                    b.Property<decimal>("Potassium");

                    b.Property<int>("SortNum");

                    b.HasKey("Id");

                    b.ToTable("Fertilizer");
                });

            modelBuilder.Entity("Agri.Models.StaticData.FertilizerMethod", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("FertilizerMethods");
                });

            modelBuilder.Entity("Agri.Models.StaticData.FertilizerType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Custom");

                    b.Property<string>("DryLiquid");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("FertilizerTypes");
                });

            modelBuilder.Entity("Agri.Models.StaticData.FertilizerUnit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("ConvToImpGalPerAc");

                    b.Property<string>("DryLiquid");

                    b.Property<decimal>("FarmReqdNutrientsStdUnitsAreaConversion");

                    b.Property<decimal>("FarmReqdNutrientsStdUnitsConversion");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("FertilizerUnits");
                });

            modelBuilder.Entity("Agri.Models.StaticData.LiquidFertilizerDensity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DensityUnitId");

                    b.Property<int>("FertilizerId");

                    b.Property<decimal>("Value");

                    b.HasKey("Id");

                    b.HasIndex("DensityUnitId");

                    b.HasIndex("FertilizerId");

                    b.ToTable("LiquidFertilizerDensities");
                });

            modelBuilder.Entity("Agri.Models.StaticData.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Location");
                });

            modelBuilder.Entity("Agri.Models.StaticData.Manure", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Ammonia");

                    b.Property<decimal>("CubicYardConversion");

                    b.Property<int>("DMId");

                    b.Property<string>("ManureClass");

                    b.Property<string>("Moisture");

                    b.Property<int>("NMineralizationId");

                    b.Property<string>("Name");

                    b.Property<decimal>("Nitrate");

                    b.Property<decimal>("Nitrogen");

                    b.Property<decimal>("Phosphorous");

                    b.Property<decimal>("Potassium");

                    b.Property<string>("SolidLiquid");

                    b.Property<int>("SortNum");

                    b.HasKey("Id");

                    b.HasIndex("DMId");

                    b.HasIndex("NMineralizationId");

                    b.ToTable("Manures");
                });

            modelBuilder.Entity("Agri.Models.StaticData.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Balance1High");

                    b.Property<int>("Balance1Low");

                    b.Property<int>("BalanceHigh");

                    b.Property<int>("BalanceLow");

                    b.Property<string>("BalanceType");

                    b.Property<string>("DisplayMessage");

                    b.Property<string>("Icon");

                    b.Property<decimal>("SoilTestHigh");

                    b.Property<decimal>("SoilTestLow");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Agri.Models.StaticData.NMineralization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("FirstYearValue");

                    b.Property<int>("Locationid");

                    b.Property<decimal>("LongTermValue");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("NMineralization");
                });

            modelBuilder.Entity("Agri.Models.StaticData.NutrientIcon", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("definition");

                    b.Property<string>("name");

                    b.HasKey("id");

                    b.ToTable("NutrientIcons");
                });

            modelBuilder.Entity("Agri.Models.StaticData.PrevCropType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CropTypeId");

                    b.Property<int?>("CropTypeId1");

                    b.Property<string>("Name");

                    b.Property<int>("PrevCropCd");

                    b.Property<int>("nCreditImperial");

                    b.Property<int>("nCreditMetric");

                    b.HasKey("Id");

                    b.HasIndex("CropTypeId");

                    b.HasIndex("CropTypeId1");

                    b.ToTable("PrevCropType");
                });

            modelBuilder.Entity("Agri.Models.StaticData.PrevManureApplicationYear", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("PrevManureApplicationYears");
                });

            modelBuilder.Entity("Agri.Models.StaticData.PrevYearManureApplDefaultNitrogen", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int[]>("DefaultNitrogenCredit");

                    b.Property<string>("PrevYearManureAppFrequency");

                    b.HasKey("Id");

                    b.ToTable("PrevYearManureApplDefaultNitrogens");
                });

            modelBuilder.Entity("Agri.Models.StaticData.Region", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("LocationId");

                    b.Property<string>("Name");

                    b.Property<int>("SoilTestPhosphorousRegionCd");

                    b.Property<int>("SoilTestPotassiumRegionCd");

                    b.Property<int>("SortNum");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.ToTable("Region");
                });

            modelBuilder.Entity("Agri.Models.StaticData.SeasonApplication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationMethod");

                    b.Property<string>("Compost");

                    b.Property<decimal>("DM_1_5");

                    b.Property<decimal>("DM_5_10");

                    b.Property<decimal>("DM_gt10");

                    b.Property<decimal>("DM_lt1");

                    b.Property<string>("ManureType");

                    b.Property<string>("Moisture");

                    b.Property<string>("Name");

                    b.Property<string>("PoultrySolid");

                    b.Property<string>("Season");

                    b.Property<int>("SortNum");

                    b.HasKey("Id");

                    b.ToTable("SeasonApplications");
                });

            modelBuilder.Entity("Agri.Models.StaticData.SelectCodeItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Cd");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.ToTable("SelectCodeItems");
                });

            modelBuilder.Entity("Agri.Models.StaticData.SelectListItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.ToTable("SelectListItems");
                });

            modelBuilder.Entity("Agri.Models.StaticData.SoilTestMethod", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("ConvertToKelownaK");

                    b.Property<decimal>("ConvertToKelownaPge72");

                    b.Property<decimal>("ConvertToKelownaPlt72");

                    b.Property<string>("Name");

                    b.Property<int>("SortNum");

                    b.HasKey("Id");

                    b.ToTable("SoilTestMethods");
                });

            modelBuilder.Entity("Agri.Models.StaticData.SoilTestPhosphorousRange", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Rating");

                    b.Property<int>("UpperLimit");

                    b.HasKey("Id");

                    b.ToTable("SoilTestPhosphorousRanges");
                });

            modelBuilder.Entity("Agri.Models.StaticData.SoilTestPotassiumRange", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Rating");

                    b.Property<int>("UpperLimit");

                    b.HasKey("Id");

                    b.ToTable("SoilTestPotassiumRanges");
                });

            modelBuilder.Entity("Agri.Models.StaticData.STKKelownaRange", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Range");

                    b.Property<int>("RangeHigh");

                    b.Property<int>("RangeLow");

                    b.HasKey("Id");

                    b.ToTable("STKKelownaRanges");
                });

            modelBuilder.Entity("Agri.Models.StaticData.STKRecommend", b =>
                {
                    b.Property<int>("STKKelownaRangeId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("K2O_Recommend_kgPeHa");

                    b.Property<int>("PotassiumCropGroupRegionCd");

                    b.Property<int?>("STKKelownaRangeId1");

                    b.Property<int>("SoilTestPotassiumRegionCd");

                    b.HasKey("STKKelownaRangeId");

                    b.HasIndex("STKKelownaRangeId1");

                    b.ToTable("STKRecommend");
                });

            modelBuilder.Entity("Agri.Models.StaticData.STPKelownaRange", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Range");

                    b.Property<int>("RangeHigh");

                    b.Property<int>("RangeLow");

                    b.HasKey("Id");

                    b.ToTable("STPKelownaRanges");
                });

            modelBuilder.Entity("Agri.Models.StaticData.STPRecommend", b =>
                {
                    b.Property<int>("STPKelownaRangeId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("P2O5_Recommend_KgPerHa");

                    b.Property<int>("PhosphorousCropGroupRegionCd");

                    b.Property<int>("SoilTestPhosphorousRegionCd");

                    b.Property<int?>("StpKelownaRangeId");

                    b.HasKey("STPKelownaRangeId");

                    b.HasIndex("StpKelownaRangeId");

                    b.ToTable("STPRecommend");
                });

            modelBuilder.Entity("Agri.Models.StaticData.Unit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Conversion_lbTon");

                    b.Property<decimal>("CostApplications");

                    b.Property<string>("CostUnits");

                    b.Property<string>("DollarUnitArea");

                    b.Property<decimal>("FarmReqdNutrientsStdUnitsAreaConversion");

                    b.Property<decimal>("FarmReqdNutrientsStdUnitsConversion");

                    b.Property<string>("Name");

                    b.Property<string>("NutrientContentUnits");

                    b.Property<string>("NutrientRateUnits");

                    b.Property<string>("SolidLiquid");

                    b.Property<string>("ValueMaterialUnits");

                    b.Property<decimal>("Value_K2O");

                    b.Property<decimal>("Value_N");

                    b.Property<decimal>("Value_P2O5");

                    b.HasKey("Id");

                    b.ToTable("Units");
                });

            modelBuilder.Entity("Agri.Models.StaticData.Version", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("StaticDataVersion");

                    b.HasKey("Id");

                    b.ToTable("Versions");
                });

            modelBuilder.Entity("Agri.Models.StaticData.Yield", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("YieldDesc");

                    b.HasKey("Id");

                    b.ToTable("Yields");
                });

            modelBuilder.Entity("Agri.Models.StaticData.AnimalSubType", b =>
                {
                    b.HasOne("Agri.Models.StaticData.Animal", "Animal")
                        .WithMany("AnimalSubTypes")
                        .HasForeignKey("AnimalId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Agri.Models.StaticData.Crop", b =>
                {
                    b.HasOne("Agri.Models.StaticData.CropType", "CropType")
                        .WithMany("Crops")
                        .HasForeignKey("CropTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Agri.Models.StaticData.CropSTKRegionCd", b =>
                {
                    b.HasOne("Agri.Models.StaticData.Crop")
                        .WithMany("CropSTKRegionCds")
                        .HasForeignKey("CropId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Agri.Models.StaticData.CropSTPRegionCd", b =>
                {
                    b.HasOne("Agri.Models.StaticData.Crop")
                        .WithMany("CropSTPRegionCds")
                        .HasForeignKey("CropId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Agri.Models.StaticData.CropYield", b =>
                {
                    b.HasOne("Agri.Models.StaticData.Crop", "Crop")
                        .WithMany("CropYields")
                        .HasForeignKey("CropId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Agri.Models.StaticData.Location", "Location")
                        .WithMany("CropYields")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Agri.Models.StaticData.LiquidFertilizerDensity", b =>
                {
                    b.HasOne("Agri.Models.StaticData.DensityUnit", "DensityUnit")
                        .WithMany("LiquidFertilizerDensities")
                        .HasForeignKey("DensityUnitId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Agri.Models.StaticData.Fertilizer", "Fertilizer")
                        .WithMany("LiquidFertilizerDensities")
                        .HasForeignKey("FertilizerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Agri.Models.StaticData.Manure", b =>
                {
                    b.HasOne("Agri.Models.StaticData.DM", "Dm")
                        .WithMany()
                        .HasForeignKey("DMId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Agri.Models.StaticData.NMineralization", "NMineralization")
                        .WithMany("Manures")
                        .HasForeignKey("NMineralizationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Agri.Models.StaticData.PrevCropType", b =>
                {
                    b.HasOne("Agri.Models.StaticData.CropType")
                        .WithMany("PrevCropTypes")
                        .HasForeignKey("CropTypeId");

                    b.HasOne("Agri.Models.StaticData.PrevCropType", "CropType")
                        .WithMany()
                        .HasForeignKey("CropTypeId1");
                });

            modelBuilder.Entity("Agri.Models.StaticData.Region", b =>
                {
                    b.HasOne("Agri.Models.StaticData.Location")
                        .WithMany("Regions")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Agri.Models.StaticData.STKRecommend", b =>
                {
                    b.HasOne("Agri.Models.StaticData.STKKelownaRange", "STKKelownaRange")
                        .WithMany("STKRecommendations")
                        .HasForeignKey("STKKelownaRangeId1");
                });

            modelBuilder.Entity("Agri.Models.StaticData.STPRecommend", b =>
                {
                    b.HasOne("Agri.Models.StaticData.STPKelownaRange", "StpKelownaRange")
                        .WithMany("STPRecommendations")
                        .HasForeignKey("StpKelownaRangeId");
                });
#pragma warning restore 612, 618
        }
    }
}

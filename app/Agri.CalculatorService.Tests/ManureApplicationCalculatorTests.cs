using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Agri.Data;
using Agri.Models;
using Agri.Models.Calculate;
using Agri.Models.Configuration;
using Agri.Models.Farm;
using Newtonsoft.Json;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Agri.CalculateService.Tests
{
    public class ManureApplicationCalculatorTests
    {
        private readonly YearData _yearData;

        public ManureApplicationCalculatorTests()
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream("Agri.CalculateService.Tests.TestData.YearDataMock.json"))
            using (var reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();
                _yearData = JsonConvert.DeserializeObject<YearData>(json);
            }
        }

        [Fact]
        public void GetFieldsAppliedWithStoredManure()
        {
            //Arrange
            var storageSystem = new ManureStorageSystem { Id = 1 };

            //act
            var result = _yearData.GetFieldsAppliedWithManure(storageSystem);

            //assess
            result.ShouldNotBeNull();
        }

        [Fact]
        public void GetAppliedLiquidStoredManure()
        {
            //Arrange
            var repository = Substitute.For<IAgriConfigurationRepository>();
            repository.GetLiquidMaterialApplicationUSGallonsPerAcreRateConversion()
                .Returns(new List<LiquidMaterialApplicationUSGallonsPerAcreRateConversion>()
                {
                    new LiquidMaterialApplicationUSGallonsPerAcreRateConversion()
                    {
                        ApplicationRateUnit = ApplicationRateUnits.ImperialGallonsPerAcre,
                        USGallonsPerAcreConversion = 1.20095m
                    }
                });
            var convertCalculator = new ManureUnitConversionCalculator(repository);
            var calculator = new ManureApplicationCalculator(convertCalculator, repository);
            var farmManure = new FarmManure() { SourceOfMaterialId = "StorageSystem, 1", Stored_Imported = NutrientAnalysisTypes.Stored };

            //Act
            var result = calculator.GetAppliedStoredManure(_yearData, farmManure);

            //Assess
            result.ShouldNotBeNull();
            result.FieldAppliedManures.Count.ShouldBeGreaterThan(0);
            result.FieldAppliedManures[0].ShouldNotBeNull();
            result.FieldAppliedManures[0].USGallonsApplied.ShouldNotBeNull();
            result.FieldAppliedManures[0].USGallonsApplied.Value.ShouldBe(120.0950000M);
            result.TotalAnnualManureToApply.ShouldBe(2989m);
            result.WholePercentAppiled.ShouldBe(4);
        }

        //[Fact]
        //public void GetAppliedSolidStoredManure()
        //{
        //    //Arrange
        //    var repository = Substitute.For<IAgriConfigurationRepository>();
        //    repository.GetSolidMaterialApplicationTonPerAcreRateConversions()
        //        .Returns(new List<SolidMaterialApplicationTonPerAcreRateConversion>()
        //        {
        //            new SolidMaterialApplicationTonPerAcreRateConversion
        //            {
        //                ApplicationRateUnit = ApplicationRateUnits.TonsPerAcre,
        //                TonsPerAcreConversion = "1"
        //            }
        //        });
        //    var convertCalculator = new ManureUnitConversionCalculator(repository);
        //    var calculator = new ManureApplicationCalculator(convertCalculator);
        //    var manureStorageSystemId = 1;

        //    //Act
        //    var result = calculator.GetAppliedStoredManure(_yearData, manureStorageSystemId);

        //    //Assess
        //    result.ShouldNotBeNull();
        //    result.FieldAppliedManures.Count > 0);
        //   result.FieldAppliedManures[0]);
        //   result.FieldAppliedManures[0].USGallonsApplied);
        //   result.FieldAppliedManures[0].USGallonsApplied.Value == 120.0950000M);
        //    result.TotalAnnualManureToApply == 2989m);
        //    result.WholePercentAppiled == 4);
        //}

        [Fact]
        public void GetFieldsAppliedWithImportedManure()
        {
            //arrange
            var farmManure = new FarmManure() { SourceOfMaterialId = "Imported, 1", Stored_Imported = NutrientAnalysisTypes.Imported };

            //act
            var result = _yearData.GetFieldsAppliedWithManure(farmManure);

            //assess
            result.ShouldNotBeNull();
        }

        [Fact]
        public void GetAppliedSolidImportedManure()
        {
            //Arrange
            var repository = Substitute.For<IAgriConfigurationRepository>();
            repository.GetSolidMaterialApplicationTonPerAcreRateConversions()
                .Returns(new List<SolidMaterialApplicationTonPerAcreRateConversion>()
                {
                    new SolidMaterialApplicationTonPerAcreRateConversion
                    {
                        ApplicationRateUnit = ApplicationRateUnits.TonsPerAcre,
                        TonsPerAcreConversion = "1"
                    }
                });
            var convertCalculator = new ManureUnitConversionCalculator(repository);
            var calculator = new ManureApplicationCalculator(convertCalculator, repository);
            //var farmManure = new FarmManure {sourceOfMaterialId = "Imported, 1", stored_imported = NutrientAnalysisTypes.Imported};
            var farmManure = _yearData.FarmManures.Single(fm => fm.Id == 2);

            //Act
            var result = calculator.GetAppliedImportedManure(_yearData, farmManure);

            //Assess
            result.ShouldNotBeNull();
            result.FieldAppliedManures.Count.ShouldBeGreaterThan(0);
            result.FieldAppliedManures[0].ShouldNotBeNull();
            result.FieldAppliedManures[0].TonsApplied.ShouldNotBeNull();
            result.FieldAppliedManures[0].TonsApplied.Value.ShouldBe(100);
            result.TotalAnnualManureToApply.ShouldBe(1000m);
            result.WholePercentAppiled.ShouldBe(10);
        }

        [Fact]
        public void GetAppliedManureForStoredFarmManure()
        {
            //Arrange
            var repository = Substitute.For<IAgriConfigurationRepository>();
            repository.GetLiquidMaterialApplicationUSGallonsPerAcreRateConversion()
                .Returns(new List<LiquidMaterialApplicationUSGallonsPerAcreRateConversion>()
                {
                    new LiquidMaterialApplicationUSGallonsPerAcreRateConversion()
                    {
                        ApplicationRateUnit = ApplicationRateUnits.ImperialGallonsPerAcre,
                        USGallonsPerAcreConversion = 1.20095m
                    }
                });

            var convertCalculator = new ManureUnitConversionCalculator(repository);
            var calculator = new ManureApplicationCalculator(convertCalculator, repository);
            var farmManure = _yearData.FarmManures.Single(fm => fm.Id == 1);
            var expectedUnAllocatedMessage =
                "Storage System: Liquid Storage System , Material: Heavy Feeders Unallocated 2868.905000000 US gallons - 100% of Total Stored";

            //Act
            var result = calculator.GetAppliedManure(_yearData, farmManure);

            //Assess
            result.ShouldNotBeNull();
            (result as AppliedStoredManure).ShouldNotBeNull();
            result.FieldAppliedManures.Count.ShouldBeGreaterThan(0);
            result.FieldAppliedManures[0].ShouldNotBeNull();
            result.FieldAppliedManures[0].USGallonsApplied.ShouldNotBeNull();
            result.FieldAppliedManures[0].USGallonsApplied.Value.ShouldBe(120.0950000M);
            result.TotalAnnualManureToApply.ShouldBe(2989m);
            result.WholePercentAppiled.ShouldBe(4);
            (result as AppliedStoredManure).ListUnallocatedMaterialAsPercentOfTotalStored[0].ShouldBe(expectedUnAllocatedMessage);
        }

        [Fact]
        public void GetAppliedManureForImportedManure()
        {
            //Arrange
            var repository = Substitute.For<IAgriConfigurationRepository>();
            repository.GetSolidMaterialApplicationTonPerAcreRateConversions()
                .Returns(new List<SolidMaterialApplicationTonPerAcreRateConversion>()
                {
                    new SolidMaterialApplicationTonPerAcreRateConversion
                    {
                        ApplicationRateUnit = ApplicationRateUnits.TonsPerAcre,
                        TonsPerAcreConversion = "1"
                    }
                });
            var convertCalculator = new ManureUnitConversionCalculator(repository);
            var calculator = new ManureApplicationCalculator(convertCalculator, repository);
            var farmManure = _yearData.FarmManures.Single(fm => fm.Id == 2);

            //Act
            var result = calculator.GetAppliedManure(_yearData, farmManure);

            //Assess
            result.ShouldNotBeNull();
            (result as AppliedImportedManure).ShouldNotBeNull();
            result.FieldAppliedManures.Count.ShouldBe(0);
            result.FieldAppliedManures[0].ShouldNotBeNull();
            result.FieldAppliedManures[0].TonsApplied.ShouldNotBeNull();
            result.FieldAppliedManures[0].TonsApplied.Value.ShouldBe(100);
            result.TotalAnnualManureToApply.ShouldBe(1000m);
            result.WholePercentAppiled.ShouldBe(10);
        }
    }
}
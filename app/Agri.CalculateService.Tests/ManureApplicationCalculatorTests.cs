using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Agri.Interfaces;
using Agri.Models;
using Agri.Models.Configuration;
using Agri.Models.Farm;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using NSubstitute;

namespace Agri.CalculateService.Tests
{
    [TestClass]
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

        [TestMethod]
        public void GetFieldsAppliedWithStoredManure()
        {
            //act
            var result = _yearData.GetFieldsAppliedWithStoredManure(1);

            //assess
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetAppliedLiquidStoredManure()
        {
            //Arrange
            var repository = Substitute.For<IAgriConfigurationRepository>();
            repository.GetLiquidMaterialsConversionFactors()
                .Returns(new List<LiquidMaterialsConversionFactor>()
                {
                    new LiquidMaterialsConversionFactor
                    {
                        InputUnit = AnnualAmountUnits.ImperialGallons, USGallonsOutput = 1.20095m
                    }
                });
            var convertCalculator = new ManureUnitConversionCalculator(repository);
            var calculator = new ManureApplicationCalculator(convertCalculator);
            var manureStorageSystemId = 1;

            //Act
            var result = calculator.GetAppliedStoredManure(_yearData, manureStorageSystemId);

            //Assess
            Assert.IsNotNull(result);
            Assert.IsTrue(result.FieldAppliedManures.Count > 0);
            Assert.IsNotNull(result.FieldAppliedManures[0]);
            Assert.IsNotNull(result.FieldAppliedManures[0].USGallonsApplied);
            Assert.IsNotNull(result.FieldAppliedManures[0].USGallonsApplied.Value == 120.0950000M);
            Assert.IsTrue(result.TotalAnnualManureToApply == 2989m);
            Assert.IsTrue(result.WholePercentAppiled == 4);
        }

        [TestMethod]
        public void GetAppliedSolidStoredManure()
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
            var calculator = new ManureApplicationCalculator(convertCalculator);
            var manureStorageSystemId = 1;

            //Act
            var result = calculator.GetAppliedStoredManure(_yearData, manureStorageSystemId);

            //Assess
            Assert.IsNotNull(result);
            Assert.IsTrue(result.FieldAppliedManures.Count > 0);
            Assert.IsNotNull(result.FieldAppliedManures[0]);
            Assert.IsNotNull(result.FieldAppliedManures[0].USGallonsApplied);
            Assert.IsNotNull(result.FieldAppliedManures[0].USGallonsApplied.Value == 120.0950000M);
            Assert.IsTrue(result.TotalAnnualManureToApply == 2989m);
            Assert.IsTrue(result.WholePercentAppiled == 4);
        }


        [TestMethod]
        public void GetFieldsAppliedWithImportedManure()
        {
            //act
            var result = _yearData.GetFieldsAppliedWithUnstorableImportedManure(1);

            //assess
            Assert.IsNotNull(result);
        }
        
        [TestMethod]
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
            var calculator = new ManureApplicationCalculator(convertCalculator);
            var importedManureId = 1;

            //Act
            var result = calculator.GetAppliedImportedManure(_yearData, importedManureId);

            //Assess
            Assert.IsNotNull(result);
            Assert.IsTrue(result.FieldAppliedManures.Count > 0);
            Assert.IsNotNull(result.FieldAppliedManures[0]);
            Assert.IsNotNull(result.FieldAppliedManures[0].TonsApplied);
            Assert.IsNotNull(result.FieldAppliedManures[0].TonsApplied.Value == 100);
            Assert.IsTrue(result.TotalAnnualManureToApply == 1000m);
            Assert.IsTrue(result.WholePercentAppiled == 10);
        }
    }

     
}

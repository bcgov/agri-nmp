using Agri.Interfaces;
using Agri.Models.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Extensions;
using System.Collections.Generic;
using Agri.Models;
using System;

namespace Agri.CalculateService.Tests
{
    [TestClass]
    public class ManureUnitConversionCalculatorTests
    {
        public ManureUnitConversionCalculatorTests()
        {
        }

        [TestMethod]
        public void GetVolumeSuccessfullyForCubicYardsInput()
        {
            //Arrange
            var repository = Substitute.For<IAgriConfigurationRepository>();
            repository.GetSolidMaterialsConversionFactors()
                .Returns(new List<SolidMaterialsConversionFactor>()
                {
                    new SolidMaterialsConversionFactor
                    {
                        InputUnit = AnnualAmountUnits.Yards, CubicYardsOutput = 1, CubicMetersOutput = 99
                    }
                });

            var calculator = new ManureUnitConversionCalculator(repository);
            var expected = 1;

            //Act
            var result = calculator.GetCubicYardsVolume(ManureMaterialType.Solid, 1, AnnualAmountUnits.Yards);
            
            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetVolumeSuccessfullyForTonsInput()
        {
            //Arrange
            var repository = Substitute.For<IAgriConfigurationRepository>();
            repository.GetSolidMaterialsConversionFactors()
                .Returns(new List<SolidMaterialsConversionFactor>()
                {
                    new SolidMaterialsConversionFactor
                    {
                        InputUnit = AnnualAmountUnits.tons, CubicYardsOutput = 1, CubicMetersOutput = 99
                    }
                });

            var calculator = new ManureUnitConversionCalculator(repository);
            var expectedCubicYards = 1;
            var expectedCubicMeters = 99;

            //Act
            var resultCubicYards = calculator.GetCubicYardsVolume(ManureMaterialType.Solid, 1, AnnualAmountUnits.tons);
            var resultCubicMeters = calculator.GetCubicYardsVolume(ManureMaterialType.Solid, 1, AnnualAmountUnits.tons);

            //Assert
            Assert.AreEqual(expectedCubicYards, resultCubicYards);
            Assert.AreEqual(expectedCubicMeters, resultCubicMeters);
        }

        [TestMethod]
        public void GetVolumeSuccessfullyForCubicMetersInput()
        {
            //Arrange
            var repository = Substitute.For<IAgriConfigurationRepository>();
            repository.GetSolidMaterialsConversionFactors()
                .Returns(new List<SolidMaterialsConversionFactor>()
                {
                    new SolidMaterialsConversionFactor
                    {
                        InputUnit = AnnualAmountUnits.CubicMeters, CubicYardsOutput = 1, CubicMetersOutput = 99
                    }
                });

            var calculator = new ManureUnitConversionCalculator(repository);

            var expectedCubicYards = 1;
            var expectedCubicMeters = 99;

            //Act
            var resultCubicYards = calculator.GetCubicYardsVolume(ManureMaterialType.Solid, 1, AnnualAmountUnits.CubicMeters);
            var resultCubicMeters = calculator.GetCubicYardsVolume(ManureMaterialType.Solid, 1, AnnualAmountUnits.CubicMeters);

            //Assert
            Assert.AreEqual(expectedCubicYards, resultCubicYards);
            Assert.AreEqual(expectedCubicMeters, resultCubicMeters);
        }

        [TestMethod]
        public void GetVolumeSuccessfullyForTonnesInput()
        {
            //Arrange
            var repository = Substitute.For<IAgriConfigurationRepository>();
            repository.GetSolidMaterialsConversionFactors()
                .Returns(new List<SolidMaterialsConversionFactor>()
                {
                    new SolidMaterialsConversionFactor
                    {
                        InputUnit = AnnualAmountUnits.tonnes, CubicYardsOutput = 1, CubicMetersOutput = 99
                    }
                });

            var calculator = new ManureUnitConversionCalculator(repository);

            var expectedCubicYards = 1;
            var expectedCubicMeters = 99;

            //Act
            var resultCubicYards = calculator.GetCubicYardsVolume(ManureMaterialType.Solid, 1, AnnualAmountUnits.tonnes);
            var resultCubicMeters = calculator.GetCubicYardsVolume(ManureMaterialType.Solid, 1, AnnualAmountUnits.tonnes);

            //Assert
            Assert.AreEqual(expectedCubicYards, resultCubicYards);
            Assert.AreEqual(expectedCubicMeters, resultCubicMeters);
        }

        [TestMethod]
        public void GetWeightSuccessfullyForTonsInput()
        {
            //Arrange
            var repository = Substitute.For<IAgriConfigurationRepository>();
            repository.GetSolidMaterialsConversionFactors()
                .Returns(new List<SolidMaterialsConversionFactor>()
                {
                    new SolidMaterialsConversionFactor
                    {
                        InputUnit = AnnualAmountUnits.tons, MetricTonsOutput = 1
                    }
                });

            var calculator = new ManureUnitConversionCalculator(repository);
            var expected = "1 tons";

            //Act
            var result = calculator.GetTonsWeight(ManureMaterialType.Solid, 1, AnnualAmountUnits.tons);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetLiquidVolumeSuccessfullyForUSGallonsInput()
        {
            //Arrange
            var repository = Substitute.For<IAgriConfigurationRepository>();
            repository.GetLiquidMaterialsConversionFactors()
                .Returns(new List<LiquidMaterialsConversionFactor>()
                {
                    new LiquidMaterialsConversionFactor
                    {
                        InputUnit = AnnualAmountUnits.USGallons, USGallonsOutput = 1
                    }
                });

            var calculator = new ManureUnitConversionCalculator(repository);
            var expected = 1;

            //Act
            var result = calculator.GetUSGallonsVolume(ManureMaterialType.Liquid, 1, AnnualAmountUnits.USGallons);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetLiquidVolumeSuccessfullyForImpGallonsInput()
        {
            //Arrange
            var repository = Substitute.For<IAgriConfigurationRepository>();
            repository.GetLiquidMaterialsConversionFactors()
                .Returns(new List<LiquidMaterialsConversionFactor>()
                {
                    new LiquidMaterialsConversionFactor
                    {
                        InputUnit = AnnualAmountUnits.ImperialGallons, USGallonsOutput = 1
                    }
                });

            var calculator = new ManureUnitConversionCalculator(repository);
            var expected = 1;

            //Act
            var result = calculator.GetUSGallonsVolume(ManureMaterialType.Liquid, 1, AnnualAmountUnits.ImperialGallons);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetLiquidVolumeSuccessfullyForCubicMetersInput()
        {
            //Arrange
            var repository = Substitute.For<IAgriConfigurationRepository>();
            repository.GetLiquidMaterialsConversionFactors()
                .Returns(new List<LiquidMaterialsConversionFactor>()
                {
                    new LiquidMaterialsConversionFactor
                    {
                        InputUnit = AnnualAmountUnits.CubicMeters, USGallonsOutput = 1
                    }
                });

            var calculator = new ManureUnitConversionCalculator(repository);
            var expected = 1;

            //Act
            var result = calculator.GetUSGallonsVolume(ManureMaterialType.Liquid, 1, AnnualAmountUnits.CubicMeters);

            //Assert
            Assert.AreEqual(expected, result);
        }
    }
}

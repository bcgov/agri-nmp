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
        public void GetDensityFactoredConversion()
        {
            //Arrange
            var repository = Substitute.For<IAgriConfigurationRepository>();
            var calculator = new ManureUnitConversionCalculator(repository);

            var expectedLessThan40 = .27m;
            var expectedBetween40And82 = 0.483825m;
            var expectedHigherThan82 = 0.837m;

            var lessThan40Result = calculator.GetDensity(35);
            var between40And82Result = calculator.GetDensity(50);
            var higherThan82Result = calculator.GetDensity(83);

            Assert.AreEqual(expectedLessThan40, lessThan40Result);
            Assert.AreEqual(expectedBetween40And82, between40And82Result);
            Assert.AreEqual(expectedHigherThan82, higherThan82Result);
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
                        InputUnit = AnnualAmountUnits.Yards,
                        CubicYardsOutput = "1",
                        CubicMetersOutput = "0.764555",
                        MetricTonsOutput = "1*density"
                    }
                });

            var calculator = new ManureUnitConversionCalculator(repository);
            var moisture = 50m;
            var amount = 5;
            var expectedCubicYards = 5;
            var expectedCubicMeters = 4;
            var expectedMetricTons = 2;

            //Act
            var cubicYardsResult = calculator.GetCubicYardsVolume(ManureMaterialType.Solid, moisture, amount, AnnualAmountUnits.Yards);
            var cubicMetersResult = calculator.GetCubicMetersVolume(ManureMaterialType.Solid, moisture, amount, AnnualAmountUnits.Yards);
            var tonsResult = calculator.GetTonsWeight(ManureMaterialType.Solid, moisture, amount, AnnualAmountUnits.Yards);

            //Assert
            Assert.AreEqual(expectedCubicYards, Math.Round(cubicYardsResult));
            Assert.AreEqual(expectedCubicMeters, Math.Round(cubicMetersResult));
            Assert.AreEqual(expectedMetricTons, Math.Round(tonsResult));
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
                        InputUnit = AnnualAmountUnits.tons,
                        CubicYardsOutput = "1/density",
                        CubicMetersOutput = "(1/density)*0.764555",
                        MetricTonsOutput = "1"
                    }
                });

            var calculator = new ManureUnitConversionCalculator(repository);
            var moisture = 50m;
            var amount = 6;
            var expectedCubicYards = 12;
            var expectedCubicMeters = 9;
            var expectedMetricTons = 6;

            //Act
            var cubicYardsResult = calculator.GetCubicYardsVolume(ManureMaterialType.Solid, moisture, amount, AnnualAmountUnits.tons);
            var cubicMetersResult = calculator.GetCubicMetersVolume(ManureMaterialType.Solid, moisture, amount, AnnualAmountUnits.tons);
            var metricTonsResult = calculator.GetTonsWeight(ManureMaterialType.Solid, moisture, amount, AnnualAmountUnits.tons);

            //Assert
            Assert.AreEqual(expectedCubicYards, Math.Round(cubicYardsResult));
            Assert.AreEqual(expectedCubicMeters, Math.Round(cubicMetersResult));
            Assert.AreEqual(expectedMetricTons, Math.Round(metricTonsResult));
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
                        InputUnit = AnnualAmountUnits.CubicMeters,
                        CubicYardsOutput = "1.30795",
                        CubicMetersOutput = "1",
                        MetricTonsOutput = "1.30795*density"
                    }
                });

            var calculator = new ManureUnitConversionCalculator(repository);

            var moisture = 50m;
            var amount = 5;
            var expectedCubicYards = 7;
            var expectedCubicMeters = 5;
            var expectedMetricTons = 3;

            //Act
            var cubicYardsResult = calculator.GetCubicYardsVolume(ManureMaterialType.Solid, moisture, amount, AnnualAmountUnits.CubicMeters);
            var cubicMetersResult = calculator.GetCubicMetersVolume(ManureMaterialType.Solid, moisture, amount, AnnualAmountUnits.CubicMeters);
            var metricTonsResult = calculator.GetTonsWeight(ManureMaterialType.Solid, moisture, amount, AnnualAmountUnits.CubicMeters);

            //Assert
            Assert.AreEqual(expectedCubicYards, Math.Round(cubicYardsResult));
            Assert.AreEqual(expectedCubicMeters, Math.Round(cubicMetersResult));
            Assert.AreEqual(expectedMetricTons, Math.Round(metricTonsResult));
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
                        InputUnit = AnnualAmountUnits.tonnes,
                        CubicYardsOutput = "1.10231/density",
                        CubicMetersOutput = "(1.10231/density)*0.764555",
                        MetricTonsOutput = "1.10231"
                    }
                });

            var calculator = new ManureUnitConversionCalculator(repository);

            var moisture = 50m;
            var amount = 40;
            var expectedCubicYards = 91;
            var expectedCubicMeters = 70;
            var expectedMetricTons = 44;

            //Act
            var cubicYardsResult = calculator.GetCubicYardsVolume(ManureMaterialType.Solid, moisture, amount, AnnualAmountUnits.tonnes);
            var cubicMetersResult = calculator.GetCubicMetersVolume(ManureMaterialType.Solid, moisture, amount, AnnualAmountUnits.tonnes);
            var metricTonsResult = calculator.GetTonsWeight(ManureMaterialType.Solid, moisture, amount, AnnualAmountUnits.tonnes);

            //Assert
            Assert.AreEqual(expectedCubicYards, Math.Round(cubicYardsResult));
            Assert.AreEqual(expectedCubicMeters, Math.Round(cubicMetersResult));
            Assert.AreEqual(expectedMetricTons, Math.Round(metricTonsResult));
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
                        InputUnit = AnnualAmountUnits.tons,
                        CubicYardsOutput = "1/density",
                        CubicMetersOutput = "(1/density)*0.764555",
                        MetricTonsOutput = "1"
                    }
                });

            var calculator = new ManureUnitConversionCalculator(repository);

            var moisture = 50m;
            var amount = 6;
            var expectedCubicYards = 12;
            var expectedCubicMeters = 9;
            var expectedMetricTons = 6;

            //Act
            var cubicYardsResult = calculator.GetCubicYardsVolume(ManureMaterialType.Solid, moisture, amount, AnnualAmountUnits.tons);
            var cubicMetersResult = calculator.GetCubicMetersVolume(ManureMaterialType.Solid, moisture, amount, AnnualAmountUnits.tons);
            var metricTonsResult = calculator.GetTonsWeight(ManureMaterialType.Solid, moisture, amount, AnnualAmountUnits.tons);

            //Assert
            Assert.AreEqual(expectedCubicYards, Math.Round(cubicYardsResult));
            Assert.AreEqual(expectedCubicMeters, Math.Round(cubicMetersResult));
            Assert.AreEqual(expectedMetricTons, Math.Round(metricTonsResult));
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
            var expectedUSGallons = 1;

            //Act
            var usGallonsResult = calculator.GetUSGallonsVolume(ManureMaterialType.Liquid, 1, AnnualAmountUnits.CubicMeters);

            //Assert
            Assert.AreEqual(expectedUSGallons, usGallonsResult);
        }

        [TestMethod]
        public void GetSolidsTonsPerAcreApplicationRateCubicYardsInput()
        {
            //Arrange
            var repository = Substitute.For<IAgriConfigurationRepository>();
            repository.GetSolidMaterialApplicationTonPerAcreRateConversions()
                .Returns(new List<SolidMaterialApplicationTonPerAcreRateConversion>()
                {
                    new SolidMaterialApplicationTonPerAcreRateConversion
                    {
                        ApplicationRateUnit = ApplicationRateUnits.CubicYardsPerAcre,
                        TonsPerAcreConversion = "1*density"
                    }
                });

            var calculator = new ManureUnitConversionCalculator(repository);

            var moisture = 75m;
            var amount = 10;
            var expectedTonsPerAcre = 7;

            //Act
            var resultTonsPerAcre = calculator.GetSolidsTonsPerAcreApplicationRate(moisture, amount, ApplicationRateUnits.CubicYardsPerAcre);

            //Assert
            Assert.AreEqual(expectedTonsPerAcre, Math.Round(resultTonsPerAcre, 1));
        }

        [TestMethod]
        public void GetSolidsTonsPerAcreApplicationRateTonsInput()
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

            var calculator = new ManureUnitConversionCalculator(repository);

            var moisture = 75m;
            var amount = 6;
            var expectedTonsPerAcre = 6;

            //Act
            var resultTonsPerAcre = calculator.GetSolidsTonsPerAcreApplicationRate(moisture, amount, ApplicationRateUnits.TonsPerAcre);

            //Assert
            Assert.AreEqual(expectedTonsPerAcre, Math.Round(resultTonsPerAcre,1));
        }

        [TestMethod]
        public void GetSolidsTonsPerAcreApplicationRateCubicMetersPerHectareInput()
        {
            //Arrange
            var repository = Substitute.For<IAgriConfigurationRepository>();
            repository.GetSolidMaterialApplicationTonPerAcreRateConversions()
                .Returns(new List<SolidMaterialApplicationTonPerAcreRateConversion>()
                {
                    new SolidMaterialApplicationTonPerAcreRateConversion
                    {
                        ApplicationRateUnit = ApplicationRateUnits.CubicMetersPerHectare,
                        TonsPerAcreConversion = "1*1.30795*density/2.47105"
                    }
                });

            var calculator = new ManureUnitConversionCalculator(repository);

            var moisture = 75m;
            var amount = 10;
            var expectedTonsPerAcre = 3.7m;

            //Act
            var resultTonsPerAcre = calculator.GetSolidsTonsPerAcreApplicationRate(moisture, amount, ApplicationRateUnits.CubicMetersPerHectare);

            //Assert
            Assert.AreEqual(expectedTonsPerAcre, Math.Round(resultTonsPerAcre, 1));
        }

        [TestMethod]
        public void GetSolidsTonsPerAcreApplicationRateTonnesPerHectareInput()
        {
            //Arrange
            var repository = Substitute.For<IAgriConfigurationRepository>();
            repository.GetSolidMaterialApplicationTonPerAcreRateConversions()
                .Returns(new List<SolidMaterialApplicationTonPerAcreRateConversion>()
                {
                    new SolidMaterialApplicationTonPerAcreRateConversion
                    {
                        ApplicationRateUnit = ApplicationRateUnits.TonnesPerHecatre,
                        TonsPerAcreConversion = "1*1.10231/2.47105"
                    }
                });

            var calculator = new ManureUnitConversionCalculator(repository);

            var moisture = 75m;
            var amount = 40;
            var expectedTonsPerAcre = 17.8m;

            //Act
            var resultTonsPerAcre = calculator.GetSolidsTonsPerAcreApplicationRate(moisture, amount, ApplicationRateUnits.TonnesPerHecatre);

            //Assert
            Assert.AreEqual(expectedTonsPerAcre, Math.Round(resultTonsPerAcre, 1));
        }

        [TestMethod]
        public void GetSolidsTonsPerAcreApplicationRateCubicYardsInputWithManureId()
        {
            //Arrange
            var repository = Substitute.For<IAgriConfigurationRepository>();
            repository.GetSolidMaterialApplicationTonPerAcreRateConversions()
                .Returns(new List<SolidMaterialApplicationTonPerAcreRateConversion>()
                {
                    new SolidMaterialApplicationTonPerAcreRateConversion
                    {
                        ApplicationRateUnit = ApplicationRateUnits.CubicYardsPerAcre,
                        TonsPerAcreConversion = "1*density"
                    }
                });
            repository.GetManure("1")
                .ReturnsForAnyArgs(new Manure
                {
                    CubicYardConversion = .51m
                });

            var calculator = new ManureUnitConversionCalculator(repository);

            var manureId = 1; //"Beef-feedlot, solid (dry)"
            var amount = 10;
            var expectedTonsPerAcre = 5.1m;

            //Act
            var resultTonsPerAcre = calculator.GetSolidsTonsPerAcreApplicationRate(manureId, amount, ApplicationRateUnits.CubicYardsPerAcre);

            //Assert
            Assert.AreEqual(expectedTonsPerAcre, Math.Round(resultTonsPerAcre, 1));
        }
    }
}

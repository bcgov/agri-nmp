using Agri.Data;
using Agri.Models;
using Agri.Models.Configuration;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using Xunit;

namespace Agri.CalculateService.Tests
{
    public class ManureUnitConversionCalculatorTests
    {
        public ManureUnitConversionCalculatorTests()
        {
        }

        [Fact]
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

            expectedLessThan40.ShouldBe(lessThan40Result);
            expectedBetween40And82.ShouldBe(between40And82Result);
            expectedHigherThan82.ShouldBe(higherThan82Result);
        }

        [Fact]
        public void GetVolumeSuccessfullyForCubicYardsInput()
        {
            //Arrange
            var repository = Substitute.For<IAgriConfigurationRepository>();
            repository.GetSolidMaterialsConversionFactors()
                .Returns(new List<SolidMaterialsConversionFactor>()
                {
                    new SolidMaterialsConversionFactor
                    {
                        InputUnit = AnnualAmountUnits.CubicYards,
                        CubicYardsOutput = "1",
                        CubicMetersOutput = "0.764555",
                        MetricTonsOutput = "1*density"
                    }
                });

            var calculator = new ManureUnitConversionCalculator(repository);
            var moisture = 50m;
            var amount = 5;
            var expectedCubicYards = 5m;
            var expectedCubicMeters = 4m;
            var expectedMetricTons = 2m;

            //Act
            var cubicYardsResult = calculator.GetCubicYardsVolume(ManureMaterialType.Solid, moisture, amount, AnnualAmountUnits.CubicYards);
            var cubicMetersResult = calculator.GetCubicMetersVolume(ManureMaterialType.Solid, moisture, amount, AnnualAmountUnits.CubicYards);
            var tonsResult = calculator.GetTonsWeight(ManureMaterialType.Solid, moisture, amount, AnnualAmountUnits.CubicYards);

            //Assert
            expectedCubicYards.ShouldBe(Math.Round(cubicYardsResult));
            expectedCubicMeters.ShouldBe(Math.Round(cubicMetersResult));
            expectedMetricTons.ShouldBe(Math.Round(tonsResult));
        }

        [Fact]
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
            var expectedCubicYards = 12m;
            var expectedCubicMeters = 9m;
            var expectedMetricTons = 6m;

            //Act
            var cubicYardsResult = calculator.GetCubicYardsVolume(ManureMaterialType.Solid, moisture, amount, AnnualAmountUnits.tons);
            var cubicMetersResult = calculator.GetCubicMetersVolume(ManureMaterialType.Solid, moisture, amount, AnnualAmountUnits.tons);
            var metricTonsResult = calculator.GetTonsWeight(ManureMaterialType.Solid, moisture, amount, AnnualAmountUnits.tons);

            //Assert
            expectedCubicYards.ShouldBe(Math.Round(cubicYardsResult));
            expectedCubicMeters.ShouldBe(Math.Round(cubicMetersResult));
            expectedMetricTons.ShouldBe(Math.Round(metricTonsResult));
        }

        [Fact]
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
            var expectedCubicYards = 7m;
            var expectedCubicMeters = 5m;
            var expectedMetricTons = 3m;

            //Act
            var cubicYardsResult = calculator.GetCubicYardsVolume(ManureMaterialType.Solid, moisture, amount, AnnualAmountUnits.CubicMeters);
            var cubicMetersResult = calculator.GetCubicMetersVolume(ManureMaterialType.Solid, moisture, amount, AnnualAmountUnits.CubicMeters);
            var metricTonsResult = calculator.GetTonsWeight(ManureMaterialType.Solid, moisture, amount, AnnualAmountUnits.CubicMeters);

            //Assert
            expectedCubicYards.ShouldBe(Math.Round(cubicYardsResult));
            expectedCubicMeters.ShouldBe(Math.Round(cubicMetersResult));
            expectedMetricTons.ShouldBe(Math.Round(metricTonsResult));
        }

        [Fact]
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
            var expectedCubicYards = 91m;
            var expectedCubicMeters = 70m;
            var expectedMetricTons = 44m;

            //Act
            var cubicYardsResult = calculator.GetCubicYardsVolume(ManureMaterialType.Solid, moisture, amount, AnnualAmountUnits.tonnes);
            var cubicMetersResult = calculator.GetCubicMetersVolume(ManureMaterialType.Solid, moisture, amount, AnnualAmountUnits.tonnes);
            var metricTonsResult = calculator.GetTonsWeight(ManureMaterialType.Solid, moisture, amount, AnnualAmountUnits.tonnes);

            //Assert
            expectedCubicYards.ShouldBe(Math.Round(cubicYardsResult));
            expectedCubicMeters.ShouldBe(Math.Round(cubicMetersResult));
            expectedMetricTons.ShouldBe(Math.Round(metricTonsResult));
        }

        [Fact]
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
            var expectedCubicYards = 12m;
            var expectedCubicMeters = 9m;
            var expectedMetricTons = 6m;

            //Act
            var cubicYardsResult = calculator.GetCubicYardsVolume(ManureMaterialType.Solid, moisture, amount, AnnualAmountUnits.tons);
            var cubicMetersResult = calculator.GetCubicMetersVolume(ManureMaterialType.Solid, moisture, amount, AnnualAmountUnits.tons);
            var metricTonsResult = calculator.GetTonsWeight(ManureMaterialType.Solid, moisture, amount, AnnualAmountUnits.tons);

            //Assert
            expectedCubicYards.ShouldBe(Math.Round(cubicYardsResult));
            expectedCubicMeters.ShouldBe(Math.Round(cubicMetersResult));
            expectedMetricTons.ShouldBe(Math.Round(metricTonsResult));
        }

        [Fact]
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
            var expected = 1m;

            //Act
            var result = calculator.GetUSGallonsVolume(ManureMaterialType.Liquid, 1, AnnualAmountUnits.USGallons);

            //Assert
            expected.ShouldBe(result);
        }

        [Fact]
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
            var expected = 1m;

            //Act
            var result = calculator.GetUSGallonsVolume(ManureMaterialType.Liquid, 1, AnnualAmountUnits.ImperialGallons);

            //Assert
            expected.ShouldBe(result);
        }

        [Fact]
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
            var expectedUSGallons = 1m;

            //Act
            var usGallonsResult = calculator.GetUSGallonsVolume(ManureMaterialType.Liquid, 1, AnnualAmountUnits.CubicMeters);

            //Assert
            expectedUSGallons.ShouldBe(usGallonsResult);
        }

        [Fact]
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
            var expectedTonsPerAcre = 7m;

            //Act
            var resultTonsPerAcre = calculator.GetSolidsTonsPerAcreApplicationRate(moisture, amount, ApplicationRateUnits.CubicYardsPerAcre);

            //Assert
            expectedTonsPerAcre.ShouldBe(Math.Round(resultTonsPerAcre, 1));
        }

        [Fact]
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
            var expectedTonsPerAcre = 6m;

            //Act
            var resultTonsPerAcre = calculator.GetSolidsTonsPerAcreApplicationRate(moisture, amount, ApplicationRateUnits.TonsPerAcre);

            //Assert
            expectedTonsPerAcre.ShouldBe(Math.Round(resultTonsPerAcre, 1));
        }

        [Fact]
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
            expectedTonsPerAcre.ShouldBe(Math.Round(resultTonsPerAcre, 1));
        }

        [Fact]
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
            expectedTonsPerAcre.ShouldBe(Math.Round(resultTonsPerAcre, 1));
        }

        [Fact]
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
            expectedTonsPerAcre.ShouldBe(Math.Round(resultTonsPerAcre, 1));
        }
    }
}
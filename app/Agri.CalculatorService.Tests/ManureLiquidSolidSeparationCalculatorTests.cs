using System;
using System.Collections.Generic;
using Agri.Data;
using Agri.Models;
using Agri.Models.Configuration;
using Agri.Models.Farm;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Agri.CalculateService.Tests
{
    public class ManureLiquidSolidSeparationCalculatorTests
    {
        [Fact]
        public void GetCalculatedSeparatedManure()
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

            repository.GetLiquidMaterialsConversionFactors()
                .Returns(new List<LiquidMaterialsConversionFactor>()
                {
                    new LiquidMaterialsConversionFactor
                    {
                        InputUnit = AnnualAmountUnits.CubicMeters, USGallonsOutput = 264.172M
                    }
                });

            var conversionCalculator = new ManureUnitConversionCalculator(repository);

            var totalLiquidVolumeToSeparate = 2789687;
            var wholePercentSeparation = 10;
            var calculator = new ManureLiquidSolidSeparationCalculator(conversionCalculator);

            //Act
            var actual = calculator.CalculateSeparatedManure(totalLiquidVolumeToSeparate, wholePercentSeparation);
            var expectedLiquidsUSGallons = 2510718;
            //38580
            var expectedSolidsTons = 925;

            //Assert
            expectedLiquidsUSGallons.ShouldBe((int)actual.LiquidUSGallons);
            expectedSolidsTons.ShouldBe((int)actual.SolidTons);
        }

        [Fact]
        public void GetCalculatedSeparatedManureWithRunOffAndUncoveredArea()
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

            repository.GetLiquidMaterialsConversionFactors()
                .Returns(new List<LiquidMaterialsConversionFactor>()
                {
                    new LiquidMaterialsConversionFactor
                    {
                        InputUnit = AnnualAmountUnits.CubicMeters, USGallonsOutput = 264.172M
                    }
                });

            var storage = new ManureStorageSystem
            {
                ManureMaterialType = ManureMaterialType.Liquid,
                GetsRunoffFromRoofsOrYards = true,
                RunoffAreaSquareFeet = 4000,
                ManureStorageStructures =
                {
                    new ManureStorageStructure
                    {
                        UncoveredAreaSquareFeet = 3000
                    }
                },
                ImportedManuresIncludedInSystem = new List<ImportedManure>
                {
                    new ImportedManure
                    {
                        ManureType = ManureMaterialType.Liquid,
                        AnnualAmountUSGallonsVolume = 100000,
                        Units = AnnualAmountUnits.USGallons
                    }
                }
            };

            var conversionCalculator = new ManureUnitConversionCalculator(repository);
            //171,797 + 100000 = 271797

            var totalLiquidVolumeToSeparate = storage.AnnualTotalAmountofManureInStorage;
            var wholePercentSeparation = 10;
            var calculator = new ManureLiquidSolidSeparationCalculator(conversionCalculator);

            //Act
            var actual = calculator.CalculateSeparatedManure(totalLiquidVolumeToSeparate, wholePercentSeparation);
            var expectedLiquidsUSGallons = 244617;
            var expectedSolidsTons = 90;

            //Assert
            Convert.ToInt32(storage.AnnualTotalPrecipitation).ShouldBe(171797);
            expectedLiquidsUSGallons.ShouldBe(Convert.ToInt32(actual.LiquidUSGallons));
            expectedSolidsTons.ShouldBe((int)actual.SolidTons);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agri.Interfaces;
using Agri.Models;
using Agri.Models.Configuration;
using Agri.Models.Farm;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Agri.CalculateService.Tests
{
    [TestClass]
    public class ManureLiquidSolidSeparationCalculatorTests
    {
        [TestMethod]
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
            Assert.AreEqual(expectedLiquidsUSGallons, actual.LiquidUSGallons);
            Assert.AreEqual(expectedSolidsTons, actual.SolidTons);

        }

        [TestMethod]
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
            Assert.AreEqual(Convert.ToInt32(storage.AnnualTotalPrecipitation), 171797);
            Assert.AreEqual(expectedLiquidsUSGallons, Convert.ToInt32(actual.LiquidUSGallons));
            Assert.AreEqual(expectedSolidsTons, actual.SolidTons);

        }
    }
}

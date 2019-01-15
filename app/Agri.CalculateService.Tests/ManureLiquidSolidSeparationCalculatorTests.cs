using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agri.Interfaces;
using Agri.Models;
using Agri.Models.Configuration;
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

            var totalLiquidVolumeToSeparate = 850000;
            var wholePercentSeparation = 10;
            var calculator = new ManureLiquidSolidSeparationCalculator(conversionCalculator);
            
            //Act
            var actual = calculator.CalculateSeparatedManure(totalLiquidVolumeToSeparate, wholePercentSeparation);
            var expectedLiquidsUSGallons = 85000;
            var expectedSolidsTons = 2537;

            //Assert
            Assert.AreEqual(expectedLiquidsUSGallons, actual.LiquidUSGallons);
            Assert.AreEqual(expectedSolidsTons, actual.SolidTons);

        }
    }
}

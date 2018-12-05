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
            var expected = "1 yards³ (99 m³)";

            //Act
            var result = calculator.GetVolume(ManureMaterialType.Solid, 1, AnnualAmountUnits.Yards);
            
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
            var expected = "1 yards³ (99 m³)";

            //Act
            var result = calculator.GetVolume(ManureMaterialType.Solid, 1, AnnualAmountUnits.tons);

            //Assert
            Assert.AreEqual(expected, result);
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
            var expected = "1 yards³ (99 m³)";

            //Act
            var result = calculator.GetVolume(ManureMaterialType.Solid, 1, AnnualAmountUnits.CubicMeters);

            //Assert
            Assert.AreEqual(expected, result);
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
            var expected = "1 yards³ (99 m³)";

            //Act
            var result = calculator.GetVolume(ManureMaterialType.Solid, 1, AnnualAmountUnits.tonnes);

            //Assert
            Assert.AreEqual(expected, result);
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
            var result = calculator.GetWeight(ManureMaterialType.Solid, 1, AnnualAmountUnits.tons);

            //Assert
            Assert.AreEqual(expected, result);
        }
    }
}

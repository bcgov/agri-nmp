using Agri.Data;
using Agri.Models.Farm;
using System;

namespace Agri.CalculateService
{
    public interface ISoilTestConverter
    {
        int GetConvertedSTK(string testingMethod, SoilTest soilTest);

        int GetConvertedSTP(string testingMethod, SoilTest soilTest);
    }

    /// <summary>
    /// Replaces SERVERAPI/Utility/SoilTestConversions.cs
    /// </summary>
    public class SoilTestConverter : ISoilTestConverter
    {
        private readonly IAgriConfigurationRepository _sd;

        public SoilTestConverter(IAgriConfigurationRepository sd)
        {
            _sd = sd;
        }

        public int GetConvertedSTP(string selectedSoilTestMethod, SoilTest soilTest)
        {
            int convertedP = 0;

            //get soil test method selected by user
            if (selectedSoilTestMethod == null)
                selectedSoilTestMethod = _sd.GetDefaultSoilTestMethod();

            var retreivedSoilTestMethod = _sd.GetSoilTestMethodById(selectedSoilTestMethod);

            if (soilTest.valPH >= 7.2M) //ph of 7.2 is a constant boundary
            {
                convertedP = Convert.ToInt16(Decimal.Multiply(retreivedSoilTestMethod.ConvertToKelownaPHGreaterThanEqual72, soilTest.ValP));
            }
            else
            {
                convertedP = Convert.ToInt16(Decimal.Multiply(retreivedSoilTestMethod.ConvertToKelownaPHLessThan72, soilTest.ValP));
            }

            return convertedP;
        }

        public int GetConvertedSTK(string selectedSoilTestMethod, SoilTest soilTest)
        {
            //get soil test method selected by user
            if (selectedSoilTestMethod == null)
                selectedSoilTestMethod = _sd.GetDefaultSoilTestMethod();

            var retreivedSoilTestMethod = _sd.GetSoilTestMethodById(selectedSoilTestMethod);

            var convertedK = Convert.ToInt16(Decimal.Multiply(retreivedSoilTestMethod.ConvertToKelownaK, soilTest.valK));

            return convertedK;
        }
    }
}
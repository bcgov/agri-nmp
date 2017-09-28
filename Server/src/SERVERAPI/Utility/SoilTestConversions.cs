using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SERVERAPI.Models.Impl;
using Microsoft.AspNetCore.Hosting;
using SERVERAPI.Models;
using static SERVERAPI.Models.StaticData;

namespace SERVERAPI.Utility
{
    public class SoilTestConversions
    {        
        private UserData _ud;
        private Models.Impl.StaticData _sd;

        public SoilTestConversions(UserData ud, Models.Impl.StaticData sd)
        {        
            _ud = ud;
            _sd = sd;
        }

        public int GetConvertedSTP(SoilTest soilTest)
        {
            int convertedP = 0;

            try
            {
                //get soil test method selected by user
                FarmDetails fd = _ud.FarmDetails();

                SoilTestMethod soilTestMethod = _sd.GetSoilTestMethodById(fd.testingMethod);

                if (soilTest.valPH >= 7.2M) //ph of 7.2 is a constant boundary
                {
                    convertedP = Convert.ToInt16(Decimal.Multiply(soilTestMethod.ConvertToKelownaPge72, soilTest.ValP));
                }
                else
                {
                    convertedP = Convert.ToInt16(Decimal.Multiply(soilTestMethod.ConvertToKelownaPlt72, soilTest.ValP));
                }

            }
            catch (Exception ex)
            {
                // display error
                throw;
            }

            return convertedP;
        }

        public int GetConvertedSTK(SoilTest soilTest)
        {
            int convertedK = 0;

            try
            {
                //get soil test method selected by user
                FarmDetails fd = _ud.FarmDetails();

                SoilTestMethod soilTestMethod = _sd.GetSoilTestMethodById(fd.testingMethod);
                
                convertedK = Convert.ToInt16(Decimal.Multiply(soilTestMethod.ConvertToKelownaK, soilTest.valK));
            }
            catch (Exception ex)
            {
                // display error
                throw;
            }

            return convertedK;
        }

        internal void UpdateSTPSTK(List<Field> fields)
        {
            if (fields.Count > 0)
            {
                foreach (Field field in fields)
                {
                    if (field.soilTest != null)
                    {
                        field.soilTest.ConvertedKelownaP = GetConvertedSTP(field.soilTest);
                        field.soilTest.ConvertedKelownaK = GetConvertedSTK(field.soilTest);
                        _ud.UpdateFieldSoilTest(field);
                    }
                }
            }
            
        }
    }
}

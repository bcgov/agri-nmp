using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SERVERAPI.Models.Impl;
using Microsoft.AspNetCore.Hosting;
using SERVERAPI.Models;
using static SERVERAPI.Models.StaticData;
using System.Data;


namespace SERVERAPI.Utility
{
    public class ChemicalBalanceMessage
    {
        //private IHostingEnvironment _env;
        private UserData _ud;
        private Models.Impl.StaticData _sd;

        //public int balance_AgrN { get; set; }
        //public int balance_AgrP2O5 { get; set; }
        //public int balance_AgrK2O { get; set; }
        //public int balance_CropN { get; set; }
        //public int balance_CropP2O5 { get; set; }
        //public int balance_CropK2O { get; set; }
        public ChemicalBalances chemicalBalances = new ChemicalBalances();
        public bool displayBalances { get; set; }

        public ChemicalBalanceMessage(UserData ud, Models.Impl.StaticData sd)
        {
            //_env = env;
            _ud = ud;
            _sd = sd;
        }

        //public string DetermineBalanceMessage(string balanceType, int balance, bool legume)
        //{ 
        //    string message = null;
        //    message = _sd.GetMessageByChemicalBalance(balanceType, balance, legume);
        //    return message;
        //}

        public List<BalanceMessages> DetermineBalanceMessages(string fieldName)
        {
            List<BalanceMessages> messages = new List<BalanceMessages>();
            bool legume = false;
            string message = string.Empty;

            //get soil test values
            ConversionFactor _cf = _sd.GetConversionFactor();

            //get the chemical balances
            ChemicalBalances cb = GetChemicalBalances(fieldName);

            //determine if a legume is included in the crops
            List<FieldCrop> fieldCrops = _ud.GetFieldCrops(fieldName);
            
            if (fieldCrops.Count > 0)
            {
                foreach (var _crop in fieldCrops)
                {
                    Crop crop = _sd.GetCrop(Convert.ToInt16(_crop.cropId));
                    if (crop.n_recommcd == 1) //is a legume
                        legume = true;
                }

                message = _sd.GetMessageByChemicalBalance("AgrN", chemicalBalances.balance_AgrN, legume);
                if (!string.IsNullOrEmpty(message))
                    messages.Add(new BalanceMessages { Message = message, Chemical = "AgrN" });

                message = _sd.GetMessageByChemicalBalance("AgrP2O5", chemicalBalances.balance_AgrP2O5, legume);
                if (!string.IsNullOrEmpty(message))
                    messages.Add(new BalanceMessages { Message = message, Chemical = "AgrP2O5" });

                message = _sd.GetMessageByChemicalBalance("AgrK2O", chemicalBalances.balance_AgrK2O, legume);
                if (!string.IsNullOrEmpty(message))
                    messages.Add(new BalanceMessages { Message = message, Chemical = "AgrK2O" });

                message = _sd.GetMessageByChemicalBalance("CropN", chemicalBalances.balance_CropN, legume);
                if (!string.IsNullOrEmpty(message))
                    messages.Add(new BalanceMessages { Message = message, Chemical = "CropN" });

                message = _sd.GetMessageByChemicalBalance("CropP2O5", chemicalBalances.balance_CropP2O5, legume, _cf.defaultSoilTestKelownaP);
                if (!string.IsNullOrEmpty(message))
                    messages.Add(new BalanceMessages { Message = message, Chemical = "CropP2O5" });

                message = _sd.GetMessageByChemicalBalance("CropK2O", chemicalBalances.balance_CropK2O, legume, _cf.defaultSoilTestKelownaK);
                if (!string.IsNullOrEmpty(message))
                    messages.Add(new BalanceMessages { Message = message, Chemical = "CropK2O" });
            }
            return messages;
        }

        public ChemicalBalances GetChemicalBalances(string fldName)
        {
            //ChemicalBalances cb = new ChemicalBalances();
            displayBalances = false;

            chemicalBalances.balance_AgrN = 0;
            chemicalBalances.balance_AgrP2O5 = 0;
            chemicalBalances.balance_AgrK2O = 0;
            chemicalBalances.balance_CropN = 0;
            chemicalBalances.balance_CropP2O5 = 0;
            chemicalBalances.balance_CropK2O = 0;

            List<FieldCrop> crps = _ud.GetFieldCrops(fldName);
            foreach (var c in crps)
            {

                chemicalBalances.balance_AgrN -= Convert.ToInt16(c.reqN);
                chemicalBalances.balance_AgrP2O5 -= Convert.ToInt16(c.reqP2o5);
                chemicalBalances.balance_AgrK2O -= Convert.ToInt16(c.reqK2o);
                chemicalBalances.balance_CropN -= Convert.ToInt16(c.remN);
                chemicalBalances.balance_CropP2O5 -= Convert.ToInt16(c.remP2o5);
                chemicalBalances.balance_CropK2O -= Convert.ToInt16(c.remK2o);
            }

            List<NutrientManure> manures = _ud.GetFieldNutrientsManures(fldName);
            foreach (var m in manures)
            {
                chemicalBalances.balance_AgrN += Convert.ToInt16(m.yrN);
                chemicalBalances.balance_AgrP2O5 += Convert.ToInt16(m.yrP2o5);
                chemicalBalances.balance_AgrK2O += Convert.ToInt16(m.yrK2o);
                chemicalBalances.balance_CropN += Convert.ToInt16(m.ltN);
                chemicalBalances.balance_CropP2O5 += Convert.ToInt16(m.ltP2o5);
                chemicalBalances.balance_CropK2O += Convert.ToInt16(m.ltK2o);
            }

            List<NutrientOther> others = _ud.GetFieldNutrientsOthers(fldName);
            foreach (var m in others)
            {
                chemicalBalances.balance_AgrN += Convert.ToInt16(m.nitrogen);
                chemicalBalances.balance_AgrP2O5 += Convert.ToInt16(m.phospherous);
                chemicalBalances.balance_AgrK2O += Convert.ToInt16(m.potassium);
                chemicalBalances.balance_CropN += Convert.ToInt16(m.nitrogen);
                chemicalBalances.balance_CropP2O5 += Convert.ToInt16(m.phospherous);
                chemicalBalances.balance_CropK2O += Convert.ToInt16(m.potassium);
            }

            if (crps.Count > 0 && (manures.Count > 0 || others.Count > 0))
                displayBalances = true;
            
            return chemicalBalances;
        }


    }
}

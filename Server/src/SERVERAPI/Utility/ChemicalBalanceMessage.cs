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
        private IHostingEnvironment _env;
        private UserData _ud;
        private Models.Impl.StaticData _sd;

        public int balance_AgrN { get; set; }
        public int balance_AgrP2O5 { get; set; }
        public int balance_AgrK2O { get; set; }
        public int balance_CropP2O5 { get; set; }
        public int balance_CropK2O { get; set; }

        public ChemicalBalanceMessage(IHostingEnvironment env, UserData ud, Models.Impl.StaticData sd)
        {
            _env = env;
            _ud = ud;
            _sd = sd;
        }

        public string DetermineBalanceMessage(string balanceType, int balance, bool legume)
        { 
            string message = null;
            message = _sd.GetMessageByChemicalBalance(balanceType, balance, legume);
            return message;
        }

        public List<string> DetermineBalanceMessages(string fieldName)
        {
            List<string> messages = null;
            bool legume = false;
            string message = string.Empty;

            //determine if a legue is included in the crops
            List<FieldCrop> fieldCrops = _ud.GetFieldCrops(fieldName);
            
            foreach (var _crop in fieldCrops)
            {
                Crop crop = _sd.GetCrop(Convert.ToInt16(_crop.cropId));
                if (crop.n_recommcd == 1) //is a legume
                    legume = true;
            }

            message = DetermineBalanceMessage("AgrN", balance_AgrN, legume);
            if (!string.IsNullOrEmpty(message))
                messages.Add(message);

            message = DetermineBalanceMessage("AgrP2O5", balance_AgrP2O5, legume);
            if (!string.IsNullOrEmpty(message))
                messages.Add(message);

            message = DetermineBalanceMessage("AgrK2O", balance_AgrK2O, legume);
            if (!string.IsNullOrEmpty(message))
                messages.Add(message);

            message = DetermineBalanceMessage("CropP2O5", balance_CropP2O5, legume);
            if (!string.IsNullOrEmpty(message))
                messages.Add(message);

            message = DetermineBalanceMessage("CropK2O", balance_CropK2O, legume);
            if (!string.IsNullOrEmpty(message))
                messages.Add(message);

            return messages;
        }




    }
}

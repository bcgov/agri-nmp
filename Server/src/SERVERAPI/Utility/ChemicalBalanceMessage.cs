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
        private UserData _ud;
        private Models.Impl.StaticData _sd;

        private const string MESSAGE_ICON_NONE = "none";
        private const string MESSAGE_ICON_GOOD = "good";
        private const string MESSAGE_ICON_WARNING = "warning";

        public ChemicalBalances chemicalBalances = new ChemicalBalances();        
        public bool displayBalances { get; set; }

        public ChemicalBalanceMessage(UserData ud, Models.Impl.StaticData sd)
        {
            _ud = ud;
            _sd = sd;
        }

        public List<BalanceMessages> DetermineBalanceMessages(string fieldName)
        {
            List<BalanceMessages> messages = new List<BalanceMessages>();
            bool legume = false;
            string message = string.Empty;
            BalanceMessages bm = new BalanceMessages();


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

                if (legume)
                {
                    // get sum of agronomic N for manure, fertilizer and other for field
                    // sum the above number with the crop removal N
                    // use the resulting number to determine message
                    int LegumeAgronomicN = GetLegumeAgronomicN(fieldName);
                    int LegumeRemovalN = GetLegumeRemovalN(fieldName);
                    bm = _sd.GetMessageByChemicalBalance("AgrN", LegumeAgronomicN + LegumeRemovalN, legume);
                    if (!string.IsNullOrEmpty(bm.Chemical))
                        messages.Add(bm);
                }
                
                bm = _sd.GetMessageByChemicalBalance("AgrN", chemicalBalances.balance_AgrN, legume);
                if (!string.IsNullOrEmpty(bm.Chemical))
                    messages.Add(bm);

                bm = _sd.GetMessageByChemicalBalance("AgrP2O5", chemicalBalances.balance_AgrP2O5, legume);
                if (!string.IsNullOrEmpty(bm.Chemical))
                    messages.Add(bm);

                bm = _sd.GetMessageByChemicalBalance("AgrK2O", chemicalBalances.balance_AgrK2O, legume);
                if (!string.IsNullOrEmpty(bm.Chemical))
                    messages.Add(bm);

                bm = _sd.GetMessageByChemicalBalance("CropN", chemicalBalances.balance_CropN, legume);
                if (!string.IsNullOrEmpty(bm.Chemical))
                    messages.Add(bm);

                bm = _sd.GetMessageByChemicalBalance("CropP2O5", chemicalBalances.balance_CropP2O5, legume);
                if (!string.IsNullOrEmpty(bm.Chemical))
                    messages.Add(bm);

                bm = _sd.GetMessageByChemicalBalance("CropK2O", chemicalBalances.balance_CropK2O, legume);
                if (!string.IsNullOrEmpty(bm.Chemical))
                    messages.Add(bm);

                bm = _sd.GetMessageByChemicalBalance("AgrP2O5CropP2O5", chemicalBalances.balance_AgrP2O5, chemicalBalances.balance_CropP2O5, "CropP2O5");
                if (!string.IsNullOrEmpty(bm.Chemical))
                    messages.Add(bm);

            }
            return messages;
        }

        public ChemicalBalances GetChemicalBalances(string fldName)
        {            
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

            List<NutrientFertilizer> fertilizers = _ud.GetFieldNutrientsFertilizers(fldName);
            foreach (var f in fertilizers)
            {
                chemicalBalances.balance_AgrN += Convert.ToInt16(f.fertN);
                chemicalBalances.balance_AgrP2O5 += Convert.ToInt16(f.fertP2o5);
                chemicalBalances.balance_AgrK2O += Convert.ToInt16(f.fertK2o);
                chemicalBalances.balance_CropN += Convert.ToInt16(f.fertN);
                chemicalBalances.balance_CropP2O5 += Convert.ToInt16(f.fertP2o5);
                chemicalBalances.balance_CropK2O += Convert.ToInt16(f.fertK2o);
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

            if (crps.Count > 0 && (manures.Count > 0 || others.Count > 0 || fertilizers.Count > 0))
                displayBalances = true;

            return chemicalBalances;
        }

        // This routine will typically be triggered after soil tests for a particular field have been updated
        // This routine will recalculate for all crops in a field the nutrients all that are dependant on the soil tests
        public void RecalcCropsSoilTestMessagesByField(string fieldName)
        {
            CalculateCropRequirementRemoval ccrr = new CalculateCropRequirementRemoval(_ud, _sd);

            //iterate through the crops and update the crop requirements
            List<FieldCrop> fieldCrops = _ud.GetFieldCrops(fieldName);

            if (fieldCrops.Count > 0)
            {
                foreach (var _crop in fieldCrops)
                {
                    CropType crpTyp = new CropType();
                    FieldCrop cf = _ud.GetFieldCrop(fieldName, _crop.id);
                    if (cf.cropId != null)
                    {
                        Crop cp = _sd.GetCrop(Convert.ToInt32(cf.cropId));
                        crpTyp = _sd.GetCropType(cp.croptypeid);
                    }
                    else
                    {
                        crpTyp.modifynitrogen = false;
                    }

                    CropRequirementRemoval crr = new CropRequirementRemoval();
                    ccrr.cropid = Convert.ToInt16(_crop.cropId);
                    ccrr.previousCropid = _crop.prevCropId;
                    ccrr.yield = _crop.yield;
                    ccrr.crudeProtien = _crop.crudeProtien;
                    ccrr.coverCropHarvested = _crop.coverCropHarvested;
                    ccrr.fieldName = fieldName;

                    crr = ccrr.GetCropRequirementRemoval();

                    if (!crpTyp.modifynitrogen)
                    {
                        cf.reqN = crr.N_Requirement;
                    }
                    cf.reqP2o5 = crr.P2O5_Requirement;
                    cf.reqK2o = crr.K2O_Requirement;
                    cf.remN = crr.N_Removal;
                    cf.remP2o5 = crr.P2O5_Removal;
                    cf.remK2o = crr.K2O_Removal;

                    _ud.UpdateFieldCrop(fieldName, cf);
                }
            }
        }

        public void RecalcCropsSoilTestMessagesByFarm()
        {
            List<Field> fields = _ud.GetFields();

            foreach (Field field in fields)
            {
                RecalcCropsSoilTestMessagesByField(field.fieldName);
            }
        }

        public int GetLegumeAgronomicN(string fldName)
        {
            int LegumeAgronomicN = 0;

            List<NutrientManure> manures = _ud.GetFieldNutrientsManures(fldName);
            foreach (var m in manures)
            {
                LegumeAgronomicN += Convert.ToInt16(m.yrN);
            }

            List<NutrientFertilizer> fertilizers = _ud.GetFieldNutrientsFertilizers(fldName);
            foreach (var f in fertilizers)
            {
                LegumeAgronomicN += Convert.ToInt16(f.fertN);
            }

            List<NutrientOther> others = _ud.GetFieldNutrientsOthers(fldName);
            foreach (var m in others)
            {
                LegumeAgronomicN += Convert.ToInt16(m.nitrogen);
            }

            return LegumeAgronomicN;
        }

        public int GetLegumeRemovalN(string fldName)
        {
            int LegumeRemovalN = 0;

            List<FieldCrop> crps = _ud.GetFieldCrops(fldName);
            foreach (var c in crps)
            {
                LegumeRemovalN -= Convert.ToInt16(c.remN);
            }

            return LegumeRemovalN;
        }

    }

}
using System;
using System.Collections.Generic;
using Agri.Data;
using System.Linq;
using Agri.Models.Calculate;
using Agri.Models.Farm;
using Agri.Models.Configuration;

namespace Agri.CalculateService
{
    public interface IChemicalBalanceMessage
    {
        int CalcPrevYearManureApplDefault(Field field);

        decimal CalcSoitTestNitrateDefault(Field field);

        List<BalanceMessages> DetermineBalanceMessages(Field field, int farmRegionId, string year);

        ChemicalBalances GetChemicalBalances(Field field, int farmRegionId, string year);

        long GetLegumeAgronomicN(Field field);

        int GetLegumeRemovalN(Field field);

        List<Field> RecalcCropsSoilTestMessagesByFarm(List<Field> fields, int FarmRegionId);

        Field RecalcCropsSoilTestMessagesByField(Field field, int farmRegionId);

        bool DisplayMessages(Field field);
    }

    public class ChemicalBalanceMessage : IChemicalBalanceMessage
    {
        //private readonly UserData _ud;
        private readonly IAgriConfigurationRepository _sd;

        private readonly ICalculateCropRequirementRemoval _calculateCropRequirementRemoval;

        public ChemicalBalances chemicalBalances = new ChemicalBalances();

        public ChemicalBalanceMessage(IAgriConfigurationRepository sd,
            ICalculateCropRequirementRemoval calculateCropRequirementRemoval)
        {
            _sd = sd;
            _calculateCropRequirementRemoval = calculateCropRequirementRemoval;
        }

        public List<BalanceMessages> DetermineBalanceMessages(Field field, int farmRegionId, string year)
        {
            List<BalanceMessages> messages = new List<BalanceMessages>();
            bool legume = false;
            string message = string.Empty;
            BalanceMessages bm = new BalanceMessages();

            //get soil test values
            ConversionFactor cf = _sd.GetConversionFactor();

            //get the chemical balances
            ChemicalBalances cb = GetChemicalBalances(field, farmRegionId, year);

            //determine if a legume is included in the crops
            var fieldCrops = field.Crops;

            if (fieldCrops.Count > 0)
            {
                foreach (var _crop in fieldCrops)
                {
                    Crop crop = _sd.GetCrop(Convert.ToInt16(_crop.cropId));
                    if (crop.NitrogenRecommendationId == 1) // no nitrogen need to be added
                        legume = true;
                }

                if (legume)
                {
                    // get sum of agronomic N for manure, fertilizer and other for field
                    // sum the above number with the crop removal N
                    // use the resulting number to determine message
                    long LegumeAgronomicN = GetLegumeAgronomicN(field);
                    int LegumeRemovalN = GetLegumeRemovalN(field);
                    bm = _sd.GetMessageByChemicalBalance("AgrN", LegumeAgronomicN + LegumeRemovalN, legume);
                    if (bm != null)
                        messages.Add(bm);
                }
                else
                {
                    bm = _sd.GetMessageByChemicalBalance("AgrN", chemicalBalances.balance_AgrN, legume);
                    if (bm != null)
                        messages.Add(bm);
                }

                bm = _sd.GetMessageByChemicalBalance("AgrP2O5", chemicalBalances.balance_AgrP2O5, legume);
                if (bm != null)
                    messages.Add(bm);

                bm = _sd.GetMessageByChemicalBalance("AgrK2O", chemicalBalances.balance_AgrK2O, legume);
                if (bm != null)
                    messages.Add(bm);

                bm = _sd.GetMessageByChemicalBalance("CropN", chemicalBalances.balance_CropN, legume);
                if (bm != null)
                    messages.Add(bm);

                bm = _sd.GetMessageByChemicalBalance("CropP2O5", chemicalBalances.balance_CropP2O5, legume);
                if (bm != null)
                    messages.Add(bm);

                bm = _sd.GetMessageByChemicalBalance("CropK2O", chemicalBalances.balance_CropK2O, legume);
                if (!string.IsNullOrEmpty(bm?.Chemical))
                    messages.Add(bm);

                bm = _sd.GetMessageByChemicalBalance("AgrP2O5CropP2O5", chemicalBalances.balance_AgrP2O5, chemicalBalances.balance_CropP2O5, "CropP2O5");
                if (bm != null)
                    messages.Add(bm);
            }
            return messages;
        }

        public ChemicalBalances GetChemicalBalances(Field field, int farmRegionId, string year)
        {
            chemicalBalances.balance_AgrN = 0;
            chemicalBalances.balance_AgrP2O5 = 0;
            chemicalBalances.balance_AgrK2O = 0;
            chemicalBalances.balance_CropN = 0;
            chemicalBalances.balance_CropP2O5 = 0;
            chemicalBalances.balance_CropK2O = 0;

            //List<FieldCrop> crps = _ud.GetFieldCrops(fldName);
            foreach (var c in field.Crops)
            {
                chemicalBalances.balance_AgrN -= Convert.ToInt64(c.reqN);
                chemicalBalances.balance_AgrP2O5 -= Convert.ToInt64(c.reqP2o5);
                chemicalBalances.balance_AgrK2O -= Convert.ToInt64(c.reqK2o);
                chemicalBalances.balance_CropN -= Convert.ToInt64(c.remN);
                chemicalBalances.balance_CropP2O5 -= Convert.ToInt64(c.remP2o5);
                chemicalBalances.balance_CropK2O -= Convert.ToInt64(c.remK2o);
            }

            if (field.HasNutrients)
            {
                //List<NutrientManure> manures = _ud.GetFieldNutrientsManures(fldName);
                foreach (var m in field.Nutrients.nutrientManures)
                {
                    chemicalBalances.balance_AgrN += Convert.ToInt64(m.yrN);
                    chemicalBalances.balance_AgrP2O5 += Convert.ToInt64(m.yrP2o5);
                    chemicalBalances.balance_AgrK2O += Convert.ToInt64(m.yrK2o);
                    chemicalBalances.balance_CropN += Convert.ToInt64(m.ltN);
                    chemicalBalances.balance_CropP2O5 += Convert.ToInt64(m.ltP2o5);
                    chemicalBalances.balance_CropK2O += Convert.ToInt64(m.ltK2o);
                }

                //List<NutrientFertilizer> fertilizers = _ud.GetFieldNutrientsFertilizers(fldName);
                foreach (var f in field.Nutrients.nutrientFertilizers)
                {
                    chemicalBalances.balance_AgrN += Convert.ToInt64(f.fertN);
                    chemicalBalances.balance_AgrP2O5 += Convert.ToInt64(f.fertP2o5);
                    chemicalBalances.balance_AgrK2O += Convert.ToInt64(f.fertK2o);
                    chemicalBalances.balance_CropN += Convert.ToInt64(f.fertN);
                    chemicalBalances.balance_CropP2O5 += Convert.ToInt64(f.fertP2o5);
                    chemicalBalances.balance_CropK2O += Convert.ToInt64(f.fertK2o);
                }

                //List<NutrientOther> others = _ud.GetFieldNutrientsOthers(fldName);
                foreach (var m in field.Nutrients.nutrientOthers)
                {
                    chemicalBalances.balance_AgrN += Convert.ToInt64(m.ltN);
                    chemicalBalances.balance_AgrP2O5 += Convert.ToInt64(m.ltP2o5);
                    chemicalBalances.balance_AgrK2O += Convert.ToInt64(m.ltK);
                    chemicalBalances.balance_CropN += Convert.ToInt64(m.yrN);
                    chemicalBalances.balance_CropP2O5 += Convert.ToInt64(m.yrP2o5);
                    chemicalBalances.balance_CropK2O += Convert.ToInt64(m.yrK);
                }
            }

            // include the Nitrogren credit as a result of adding manure in previous years
            // lookup default Nitrogen credit.
            //Field field = _ud.GetFieldDetails(fldName);
            if (field.Crops != null)
            {
                if (field.PreviousYearManureApplicationNitrogenCredit != null && field.Crops.Count() > 0)
                    chemicalBalances.balance_AgrN += Convert.ToInt32(field.PreviousYearManureApplicationNitrogenCredit);
                else
                    // accomodate previous version of farm data - lookup default Nitrogen credit.
                    chemicalBalances.balance_AgrN += CalcPrevYearManureApplDefault(field);
                if (field.SoilTest != null)
                {
                    if (_sd.IsNitrateCreditApplicable(farmRegionId, field.SoilTest.sampleDate, Convert.ToInt16(year)))
                    {
                        if (field.SoilTestNitrateOverrideNitrogenCredit != null && field.Crops.Count() > 0)
                            chemicalBalances.balance_AgrN += Convert.ToInt32(Math.Round(Convert.ToDecimal(field.SoilTestNitrateOverrideNitrogenCredit)));
                        else
                            // accomodate previous version of farm data - lookup default Nitrogen credit.
                            chemicalBalances.balance_AgrN += Convert.ToInt32(Math.Round(CalcSoitTestNitrateDefault(field)));
                    }
                }
            }

            return chemicalBalances;
        }

        // This routine will typically be triggered after soil tests for a particular field have been updated
        // This routine will recalculate for all crops in a field the nutrients all that are dependant on the soil tests
        public Field RecalcCropsSoilTestMessagesByField(Field field, int farmRegionId)
        {
            //iterate through the crops and update the crop requirements
            var fieldResult = field;
            var fieldCrops = fieldResult.Crops;

            if (fieldCrops.Count > 0)
            {
                foreach (var crop in fieldCrops)
                {
                    CropType crpTyp = new CropType();
                    if (crop.cropId != null)
                    {
                        Crop cp = _sd.GetCrop(Convert.ToInt32(crop.cropId));
                        crpTyp = _sd.GetCropType(cp.CropTypeId);
                    }
                    else
                    {
                        crpTyp.ModifyNitrogen = false;
                    }

                    var crr = _calculateCropRequirementRemoval.GetCropRequirementRemoval(
                                Convert.ToInt16(crop.cropId),
                                crop.yield,
                                crop.crudeProtien,
                                crop.coverCropHarvested,
                                0,
                                farmRegionId,
                                field);

                    if (!crpTyp.ModifyNitrogen)
                    {
                        crop.reqN = crr.N_Requirement;
                    }
                    crop.reqP2o5 = crr.P2O5_Requirement;
                    crop.reqK2o = crr.K2O_Requirement;
                    crop.remN = crr.N_Removal;
                    crop.remP2o5 = crr.P2O5_Removal;
                    crop.remK2o = crr.K2O_Removal;
                }
            }

            return fieldResult;
        }

        public List<Field> RecalcCropsSoilTestMessagesByFarm(List<Field> fields, int FarmRegionId)
        {
            var fieldsResult = fields;
            foreach (Field field in fieldsResult)
            {
                RecalcCropsSoilTestMessagesByField(field, FarmRegionId);
            }
            return fieldsResult;
        }

        public long GetLegumeAgronomicN(Field field)
        {
            long LegumeAgronomicN = 0;

            if (field.HasNutrients)
            {
                //List<NutrientManure> manures = _ud.GetFieldNutrientsManures(fldName)
                var manures = field.Nutrients.nutrientManures;
                foreach (var m in manures)
                {
                    LegumeAgronomicN += Convert.ToInt64(m.yrN);
                }

                //List<NutrientFertilizer> fertilizers = _ud.GetFieldNutrientsFertilizers(fldName);
                var fertilizers = field.Nutrients.nutrientFertilizers;
                foreach (var f in fertilizers)
                {
                    LegumeAgronomicN += Convert.ToInt64(f.fertN);
                }

                //List<NutrientOther> others = _ud.GetFieldNutrientsOthers(fldName);
                var others = field.Nutrients.nutrientOthers;
                foreach (var m in others)
                {
                    LegumeAgronomicN += Convert.ToInt64(m.yrN);
                }
            }

            //Field field = _ud.GetFieldDetails(fldName);

            if (field.PreviousYearManureApplicationNitrogenCredit != null)
                LegumeAgronomicN += Convert.ToInt64(field.PreviousYearManureApplicationNitrogenCredit);
            else
                LegumeAgronomicN += Convert.ToInt64(CalcPrevYearManureApplDefault(field));

            if (field.SoilTestNitrateOverrideNitrogenCredit != null)
                LegumeAgronomicN += Convert.ToInt64(field.SoilTestNitrateOverrideNitrogenCredit);
            else
                LegumeAgronomicN += Convert.ToInt64(CalcSoitTestNitrateDefault(field));

            return LegumeAgronomicN;
        }

        public int GetLegumeRemovalN(Field field)
        {
            int LegumeRemovalN = 0;

            foreach (var c in field.Crops)
            {
                LegumeRemovalN -= Convert.ToInt16(c.remN);
            }

            return LegumeRemovalN;
        }

        private int prevYearManureDefaultLookup(string prevYearManureAppl_volcatcd, string prevManureApplicationYearsFrequency)
        {
            List<PreviousYearManureApplicationNitrogenDefault> defaultNitrogen = new List<PreviousYearManureApplicationNitrogenDefault>();
            defaultNitrogen = _sd.GetPrevYearManureNitrogenCreditDefaults();
            // loop through to find the default nitrogen credit
            foreach (var item in defaultNitrogen)
                if (item.PreviousYearManureAplicationFrequency == prevManureApplicationYearsFrequency)
                    // refactor - check to make sure it can handle additional volCatCd
                    return item.DefaultNitrogenCredit[Convert.ToInt16(prevYearManureAppl_volcatcd)];
            return 0;
        }

        public int CalcPrevYearManureApplDefault(Field field)
        {
            if (field != null)
            {
                string prevYearManureApplFrequency = field.PreviousYearManureApplicationFrequency;
                int largestPrevYearManureVolumeCategory = 0;
                if (field.Crops != null)
                {
                    if (field.Crops.Count() > 0)
                    {
                        foreach (FieldCrop crop in field.Crops)
                            if (crop.prevYearManureAppl_volCatCd > largestPrevYearManureVolumeCategory)
                                largestPrevYearManureVolumeCategory = crop.prevYearManureAppl_volCatCd;
                    }
                    return prevYearManureDefaultLookup(largestPrevYearManureVolumeCategory.ToString(), prevYearManureApplFrequency);
                }
            }
            return 0;  // no Nitrogen credit as there are no crops
        }

        public decimal CalcSoitTestNitrateDefault(Field field)
        {
            if (field != null)
            {
                if (field.Crops != null)
                {
                    if (field.Crops.Count() > 0 && field.SoilTest != null)
                    {
                        return field.SoilTest.valNO3H * _sd.GetSoilTestNitratePPMToPoundPerAcreConversionFactor();
                    }
                }
            }
            return 0;  // no Nitrogen credit as there are no crops
        }

        public bool DisplayMessages(Field field)
        {
            //display balance messages when at least one Crop has been added
            if (field.Crops.Count > 0)
            {
                foreach (var crp in field.Crops)
                {
                    Crop cp = _sd.GetCrop(Convert.ToInt32(crp.cropId));
                    if (cp.CropTypeId != 2)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
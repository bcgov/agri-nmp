﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agri.Interfaces;
using Agri.Models;
using Agri.Models.Calculate;
using Agri.Models.Farm;

namespace Agri.CalculateService
{
    public class ManureApplicationCalculator : IManureApplicationCalculator
    {
        private IManureUnitConversionCalculator _manureUnitConversionCalculator;

        public ManureApplicationCalculator(IManureUnitConversionCalculator manureUnitConversionCalculator)
        {
            _manureUnitConversionCalculator = manureUnitConversionCalculator;
        }

        public AppliedManure GetAppliedManure(YearData yearData, FarmManure farmManure)
        {
            AppliedManure appliedManure;
            if (farmManure.stored_imported == NutrientAnalysisTypes.Stored)
            {
                //Stored Manure
                appliedManure = GetAppliedStoredManure(yearData, farmManure.managedManureId);
            }
            else
            {
                appliedManure = GetAppliedImportedManure(yearData, farmManure.managedManureId);
            }

            return appliedManure;
        }

        public AppliedStoredManure GetAppliedStoredManure(YearData yearData, string managedManureId)
        {
            var manureStorageSystemId = yearData.GetManureStorageSystemId(managedManureId);
            var manureStorageSystem = yearData.ManureStorageSystems.SingleOrDefault(mss => mss.Id == manureStorageSystemId);
            //var managedManureIds = manureStorageSystem.MaterialsIncludedInSystem.Select(m => m.ManureId).ToList();
            //var fieldsAppliedWithStoredManure = yearData.GetFieldsAppliedWithManure(managedManureIds);
            //var farmManureIds = yearData.GetFarmManureIds(managedManureIds);

            //var fieldAppliedManures = new List<FieldAppliedManure>();
            //foreach (var field in fieldsAppliedWithStoredManure)
            //{
            //    var nutrientManures = field.nutrients.nutrientManures
            //        .Where(nm => farmManureIds.Any(fm => fm == Convert.ToInt32(nm.manureId)))
            //        .ToList();

            //    foreach (var nutrientManure in nutrientManures)
            //    {
            //        var fieldAppliedManure = new FieldAppliedManure();
            //        if (manureStorageSystem.ManureMaterialType == ManureMaterialType.Liquid)
            //        {
            //            var convertedRate = _manureUnitConversionCalculator
            //                .GetLiquidUSGallonsPerAcreApplicationRate(nutrientManure.rate,
            //                    (ApplicationRateUnits) Convert.ToInt32(nutrientManure.unitId));

            //            fieldAppliedManure.USGallonsApplied =
            //                field.area * convertedRate;
            //        }
            //        else
            //        {
            //            var farmManure = yearData.farmManures
            //                .Single(fm => fm.id == Convert.ToInt32(nutrientManure.manureId));

            //            decimal convertedRate;
            //            if (string.IsNullOrWhiteSpace(farmManure.moisture))
            //            {
            //                convertedRate = _manureUnitConversionCalculator
            //                    .GetSolidsTonsPerAcreApplicationRate(farmManure.manureId, nutrientManure.rate,
            //                        (ApplicationRateUnits) Convert.ToInt32(nutrientManure.unitId));
            //            }
            //            else
            //            {
            //                convertedRate = _manureUnitConversionCalculator
            //                    .GetSolidsTonsPerAcreApplicationRate(Convert.ToDecimal(farmManure.moisture),
            //                        nutrientManure.rate,
            //                        (ApplicationRateUnits) Convert.ToInt32(nutrientManure.unitId));
            //            }

            //            fieldAppliedManure.TonsApplied = field.area * convertedRate;
            //        }

            //        fieldAppliedManures.Add(fieldAppliedManure);
            //    }
            //}

            //var appliedStoredManure = new AppliedStoredManure(fieldAppliedManures, manureStorageSystem);
            var appliedStoredManure = GetAppliedManureFromStorageSystem(yearData, manureStorageSystem);

            return appliedStoredManure;
        }

        public AppliedStoredManure GetAppliedManureFromStorageSystem(YearData yearData, ManureStorageSystem manureStorageSystem)
        {
            var managedManureIds = manureStorageSystem.MaterialsIncludedInSystem.Select(m => m.ManureId).ToList();
            var fieldsAppliedWithStoredManure = yearData.GetFieldsAppliedWithManure(managedManureIds);
            var farmManureIds = yearData.GetFarmManureIds(managedManureIds);

            var fieldAppliedManures = new List<FieldAppliedManure>();
            foreach (var field in fieldsAppliedWithStoredManure)
            {
                var nutrientManures = field.nutrients.nutrientManures
                    .Where(nm => farmManureIds.Any(fm => fm == Convert.ToInt32(nm.manureId)))
                    .ToList();

                foreach (var nutrientManure in nutrientManures)
                {
                    var fieldAppliedManure = new FieldAppliedManure();
                    if (manureStorageSystem.ManureMaterialType == ManureMaterialType.Liquid)
                    {
                        var convertedRate = _manureUnitConversionCalculator
                            .GetLiquidUSGallonsPerAcreApplicationRate(nutrientManure.rate,
                                (ApplicationRateUnits)Convert.ToInt32(nutrientManure.unitId));

                        fieldAppliedManure.USGallonsApplied =
                            field.area * convertedRate;
                    }
                    else
                    {
                        var farmManure = yearData.farmManures
                            .Single(fm => fm.id == Convert.ToInt32(nutrientManure.manureId));

                        decimal convertedRate;
                        if (string.IsNullOrWhiteSpace(farmManure.moisture))
                        {
                            convertedRate = _manureUnitConversionCalculator
                                .GetSolidsTonsPerAcreApplicationRate(farmManure.manureId, nutrientManure.rate,
                                    (ApplicationRateUnits)Convert.ToInt32(nutrientManure.unitId));
                        }
                        else
                        {
                            convertedRate = _manureUnitConversionCalculator
                                .GetSolidsTonsPerAcreApplicationRate(Convert.ToDecimal(farmManure.moisture),
                                    nutrientManure.rate,
                                    (ApplicationRateUnits)Convert.ToInt32(nutrientManure.unitId));
                        }

                        fieldAppliedManure.TonsApplied = field.area * convertedRate;
                    }

                    fieldAppliedManures.Add(fieldAppliedManure);
                }
            }

            var appliedStoredManure = new AppliedStoredManure(fieldAppliedManures, manureStorageSystem);

            return appliedStoredManure;
        }



        public AppliedImportedManure GetAppliedImportedManure(YearData yearData, string managedManureId)
        {
            var fieldsAppliedWithImportedManure = yearData.GetFieldsAppliedWithManure(managedManureId);
            var farmManureIds = yearData.GetFarmManureIds(managedManureId);
            var importedManureId = yearData.GetImportedManureId(managedManureId);
            var importedManure = yearData.ImportedManures.SingleOrDefault(mss => mss.Id == importedManureId);

            var fieldAppliedManures = new List<FieldAppliedManure>();
            foreach (var field in fieldsAppliedWithImportedManure)
            {
                var nutrientManures = field.nutrients.nutrientManures
                    .Where(nm => farmManureIds.Any(fm => fm == Convert.ToInt32(nm.manureId)))
                    .ToList();

                foreach (var nutrientManure in nutrientManures)
                {
                    var fieldAppliedManure = new FieldAppliedManure();
                    if (importedManure.ManureType == ManureMaterialType.Liquid)
                    {
                        var convertedRate = _manureUnitConversionCalculator
                            .GetLiquidUSGallonsPerAcreApplicationRate(nutrientManure.rate,
                                (ApplicationRateUnits)Convert.ToInt32(nutrientManure.unitId));

                        fieldAppliedManure.USGallonsApplied =
                            field.area * convertedRate;
                    }
                    else
                    {
                        var convertedRate = _manureUnitConversionCalculator
                            .GetSolidsTonsPerAcreApplicationRate(importedManure.Moisture.Value, nutrientManure.rate,
                                (ApplicationRateUnits) Convert.ToInt32(nutrientManure.unitId));

                        fieldAppliedManure.TonsApplied = field.area * convertedRate;
                    }

                    fieldAppliedManures.Add(fieldAppliedManure);
                }
            }

            var appliedImportedManure = new AppliedImportedManure(fieldAppliedManures, importedManure);

            return appliedImportedManure;
        }
    }
}

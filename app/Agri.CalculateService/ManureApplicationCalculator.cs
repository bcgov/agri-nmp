using System;
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

        public AppliedStoredManure GetAppliedStoredManure(YearData yearData, int manureStorageSystemId)
        {
            var fieldsAppliedWithStoredManure = yearData.GetFieldsAppliedWithStoredManure(manureStorageSystemId);
            var farmManureIds = yearData.GetFarmManureIdsForStorageSystem(manureStorageSystemId);
            var manureStorageSystem = yearData.ManureStorageSystems.SingleOrDefault(mss => mss.Id == manureStorageSystemId);

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
                            .GetUSGallonsVolume(ManureMaterialType.Liquid,
                            nutrientManure.rate, 
                            (AnnualAmountUnits) Convert.ToInt32(nutrientManure.unitId));

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
                                    (ApplicationRateUnits) Convert.ToInt32(nutrientManure.unitId));
                        }
                        else
                        {
                            convertedRate = _manureUnitConversionCalculator
                                .GetSolidsTonsPerAcreApplicationRate(Convert.ToDecimal(farmManure.moisture), nutrientManure.rate,
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

        public AppliedImportedManure GetAppliedImportedManure(YearData yearData, int importedManureId)
        {
            var fieldsAppliedWithImportedManure = yearData.GetFieldsAppliedWithUnstorableImportedManure(importedManureId);
            var farmManureIds = yearData.GetFarmManureIdsForImportedManure(importedManureId);
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
                            .GetUSGallonsVolume(ManureMaterialType.Liquid,
                            nutrientManure.rate,
                            (AnnualAmountUnits)Convert.ToInt32(nutrientManure.unitId));

                        fieldAppliedManure.USGallonsApplied =
                            field.area * convertedRate;
                    }
                    else
                    {
                        var convertedRate = _manureUnitConversionCalculator
                            .GetSolidsTonsPerAcreApplicationRate(importedManure.Moisture.Value, nutrientManure.rate,
                                (ApplicationRateUnits)Convert.ToInt32(nutrientManure.unitId));

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

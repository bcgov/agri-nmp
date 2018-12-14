using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using Newtonsoft.Json;

namespace Agri.Models.Farm
{
    public class YearData
    {
        public string year { get; set; }
        public List<Field> fields { get; set; }
        public List<FarmManure> farmManures { get; set; }
        public List<GeneratedManure> GeneratedManures { get; set; }
        public List<ImportedManure> ImportedManures { get; set; }
        public List<ManureStorageSystem> ManureStorageSystems { get; set; }

        public int? GetManureStorageSystemId(string managedManureId)
        {
            var manureStorageSystemId = ManureStorageSystems.SingleOrDefault(mss =>
                mss.MaterialsIncludedInSystem.Any(m => m.ManureId == managedManureId))?.Id;
            return manureStorageSystemId;
        }

        public int? GetImportedManureId(string managedManureId)
        {
            var importedManureId =
                ImportedManures.SingleOrDefault(im => im.ManureId == managedManureId).Id;
            return importedManureId;
        }

        public List<NutrientManure> GetStoredManureAllocated(string managedManureId)
        {
            var farmManureIds = GetFarmManureIdsForStorageSystem(managedManureId);

            var appliedAsNutrients = GetNutrientManuresFromFields(farmManureIds);

            return appliedAsNutrients;
        }

        public List<int> GetFarmManureIdsForStorageSystem(string managedManureId)
        {
            var farmManureIds = farmManures
                .Where(fm => fm.stored_imported == NutrientAnalysisTypes.Stored && fm.managedManureId == managedManureId)
                .Select(fm => fm.id).ToList();

            return farmManureIds;
        }

        public List<NutrientManure> GetUnstorableImportedManureAllocated(string managedManureId)
        {
            var farmManureIds = GetFarmManureIdsForImportedManure(managedManureId);

            var appliedAsNutrients = GetNutrientManuresFromFields(farmManureIds);

            return appliedAsNutrients;
        }

        public List<int> GetFarmManureIdsForImportedManure(string managedManureId)
        {
            var farmManureIds = farmManures.Where(fm =>
                    fm.stored_imported == NutrientAnalysisTypes.Imported && fm.managedManureId == managedManureId)
                .Select(fm => fm.id).ToList();

            return farmManureIds;
        }

        public List<NutrientManure> GetNutrientManuresFromFields(List<int> farmManureIds)
        {
            return fields
                .SelectMany(f => f.nutrients.nutrientManures)
                .Where(nm => farmManureIds.Any(fm => fm == Convert.ToInt32(nm.manureId)))
                .ToList();
        }


        public List<Field> GetFieldsAppliedWithStoredManure(string managedManureId)
        {
            var farmManureIds = GetFarmManureIdsForStorageSystem(managedManureId);

            var appliedFields = GetFieldsAppliedWithFarmManure(farmManureIds);

            return appliedFields;
        }

        public List<Field> GetFieldsAppliedWithUnstorableImportedManure(string managedManureId)
        {
            var farmManureIds = GetFarmManureIdsForImportedManure(managedManureId);

            var appliedFields = GetFieldsAppliedWithFarmManure(farmManureIds);

            return appliedFields;
        }

        public List<Field> GetFieldsAppliedWithFarmManure(List<int> farmManureIds)
        {
            return fields
                .Where(f => f.nutrients != null && f.nutrients.nutrientManures
                                .Any(nm => farmManureIds
                                            .Any(fm => fm == Convert.ToInt32(nm.manureId)))).ToList();
        }
    }
}
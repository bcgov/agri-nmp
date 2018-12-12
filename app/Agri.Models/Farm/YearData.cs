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


        public List<NutrientManure> GetStoredManureAllocated(int storageSystemId)
        {
            var farmManureIds = GetFarmManureIdsForStorageSystem(storageSystemId);

            var appliedAsNutrients = GetNutrientManuresFromFields(farmManureIds);

            return appliedAsNutrients;
        }

        public List<int> GetFarmManureIdsForStorageSystem(int storageSystemId)
        {
            var farmManureIds = farmManures.Where(fm =>
                    fm.sourceOfMaterialStorageSystemId.HasValue && fm.sourceOfMaterialStorageSystemId == storageSystemId)
                .Select(fm => fm.id).ToList();

            return farmManureIds;
        }

        public List<NutrientManure> GetUnstorableImportedManureAllocated(int importedManureId)
        {
            var farmManureIds = GetFarmManureIdsForImportedManure(importedManureId);

            var appliedAsNutrients = GetNutrientManuresFromFields(farmManureIds);

            return appliedAsNutrients;
        }

        public List<int> GetFarmManureIdsForImportedManure(int importedManureId)
        {
            var farmManureIds = farmManures.Where(fm =>
                    fm.sourceOfMaterialImportedManureId.HasValue && fm.sourceOfMaterialStorageSystemId == importedManureId)
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


        public List<Field> GetFieldsAppliedWithStoredManure(int storageSystemId)
        {
            var farmManureIds = GetFarmManureIdsForStorageSystem(storageSystemId);

            var appliedFields = GetFieldsAppliedWithFarmManure(farmManureIds);

            return appliedFields;
        }

        public List<Field> GetFieldsAppliedWithUnstorableImportedManure(int importedManureId)
        {
            var farmManureIds = GetFarmManureIdsForImportedManure(importedManureId);

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
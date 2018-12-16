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


        public List<int> GetFarmManureIds(List<string> managedManureIds)
        {
            var farmManureIds = farmManures
                .Where(fm => managedManureIds.Any(mm => mm == fm.managedManureId))
                .Select(fm => fm.id).ToList();

            return farmManureIds;
        }

        public List<int> GetFarmManureIds(string managedManureId)
        {
            var farmManureIds = GetFarmManureIds(new List<string> {managedManureId});

            return farmManureIds;
        }
        
        public List<NutrientManure> GetNutrientManuresFromFields(List<int> farmManureIds)
        {
            return fields
                .SelectMany(f => f.nutrients.nutrientManures)
                .Where(nm => farmManureIds.Any(fm => fm == Convert.ToInt32(nm.manureId)))
                .ToList();
        }
        
        public List<Field> GetFieldsAppliedWithManure(List<string> managedManureIds)
        {
            var farmManureIds = GetFarmManureIds(managedManureIds);

            var appliedFields = GetFieldsAppliedWithFarmManure(farmManureIds);

            return appliedFields;
        }
        
        public List<Field> GetFieldsAppliedWithManure(string managedManureId)
        {
            var farmManureIds = GetFarmManureIds(managedManureId);

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
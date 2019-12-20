using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using Newtonsoft.Json;

namespace Agri.Models.Farm
{
    public class YearData
    {
        public YearData()
        {
            Fields = new List<Field>();
            RancherFields = new List<RancherField>();
            Animals = new List<Animal>();
            FarmManures = new List<FarmManure>();
            GeneratedManures = new List<GeneratedManure>();
            ImportedManures = new List<ImportedManure>();
            SeparatedSolidManures = new List<SeparatedSolidManure>();
            ManureStorageSystems = new List<ManureStorageSystem>();
        }

        public string Year { get; set; }
        public List<Field> Fields { get; set; }
        public List<RancherField> RancherFields { get; set; }
        public List<Animal> Animals { get; set; }
        public List<FarmManure> FarmManures { get; set; }
        public List<GeneratedManure> GeneratedManures { get; set; }
        public List<ImportedManure> ImportedManures { get; set; }
        public List<SeparatedSolidManure> SeparatedSolidManures { get; set; }
        public List<ManureStorageSystem> ManureStorageSystems { get; set; }

        public List<int> GetFarmManureIds(ManureStorageSystem manureStorageSystem)
        {
            if (manureStorageSystem == null)
            {
                return new List<int>();
            }
            var farmManureIds = FarmManures
                .Where(fm => fm.sourceOfMaterialStoredSystemId == manureStorageSystem.Id)
                .Select(fm => fm.id).ToList();

            return farmManureIds;
        }

        public List<int> GetFarmManureIds(ImportedManure importedManure)
        {
            if (importedManure == null)
            {
                return new List<int>();
            }
            var farmManureIds = FarmManures
                .Where(fm => fm.sourceOfMaterialImportedManureId == importedManure.Id)
                .Select(fm => fm.id).ToList();

            return farmManureIds;
        }

        public List<NutrientManure> GetNutrientManuresFromFields(List<int> farmManureIds)
        {
            if (Fields.Any(f => f.nutrients != null))
            {
                return Fields
                    .SelectMany(f => f.nutrients.nutrientManures)
                    .Where(nm => farmManureIds.Any(fm => fm == Convert.ToInt32(nm.manureId)))
                    .ToList();
            }
            return new List<NutrientManure>();
        }

        public List<Field> GetFieldsAppliedWithManure(ManureStorageSystem manureStorageSystem)
        {
            var farmManureIds = GetFarmManureIds(manureStorageSystem);

            var appliedFields = GetFieldsAppliedWithFarmManure(farmManureIds);

            return appliedFields;
        }

        public List<Field> GetFieldsAppliedWithManure(ImportedManure importedManure)
        {
            var farmManuresWithImported = GetFarmManureIds(importedManure);
            var fields = GetFieldsAppliedWithFarmManure(farmManuresWithImported);

            return fields;
        }

        public List<Field> GetFieldsAppliedWithManure(FarmManure farmManure)
        {
            var appliedFields = GetFieldsAppliedWithFarmManure(farmManure != null ? new List<int> { farmManure.id } : new List<int>());

            return appliedFields;
        }

        public List<Field> GetFieldsAppliedWithFarmManure(List<int> farmManureIds)
        {
            return Fields
                .Where(f => f.nutrients != null && f.nutrients.nutrientManures
                                .Any(nm => farmManureIds
                                            .Any(fm => fm == Convert.ToInt32(nm.manureId)))).ToList();
        }
    }
}
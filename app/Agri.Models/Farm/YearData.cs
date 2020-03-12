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
            FarmAnimals = new List<FarmAnimal>();
            Fields = new List<Field>();
            FarmManures = new List<FarmManure>();
            GeneratedManures = new List<GeneratedManure>();
            ImportedManures = new List<ImportedManure>();
            SeparatedSolidManures = new List<SeparatedSolidManure>();
            ManureStorageSystems = new List<ManureStorageSystem>();
        }

        public string Year { get; set; }
        public List<Field> Fields { get; set; }
        public List<FarmAnimal> FarmAnimals { get; set; }
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
                .Where(fm => fm.SourceOfMaterialStoredSystemId == manureStorageSystem.Id)
                .Select(fm => fm.Id).ToList();

            return farmManureIds;
        }

        public List<int> GetFarmManureIds(ImportedManure importedManure)
        {
            if (importedManure == null)
            {
                return new List<int>();
            }
            var farmManureIds = FarmManures
                .Where(fm => fm.SourceOfMaterialImportedManureId == importedManure.Id)
                .Select(fm => fm.Id).ToList();

            return farmManureIds;
        }

        public List<NutrientManure> GetNutrientManuresFromFields(List<int> farmManureIds)
        {
            if (Fields.Any(f => f.Nutrients != null))
            {
                return Fields
                    .SelectMany(f => f.Nutrients.nutrientManures)
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

        public List<Field> GetFieldsAppliedWithManure(FarmAnimal farmAnimal)
        {
            var fields = GetFieldsAppliedWithFarmManure(new List<int> { farmAnimal.Id.GetValueOrDefault(0) });

            return fields;
        }

        public List<Field> GetFieldsAppliedWithManure(FarmManure farmManure)
        {
            var appliedFields = GetFieldsAppliedWithFarmManure(farmManure != null ? new List<int> { farmManure.Id } : new List<int>());

            return appliedFields;
        }

        public List<Field> GetFieldsAppliedWithFarmManure(List<int> farmManureIds)
        {
            return Fields
                .Where(f => f.Nutrients != null && f.Nutrients.nutrientManures
                                .Any(nm => farmManureIds
                                            .Any(fm => fm == Convert.ToInt32(nm.manureId)))).ToList();
        }
    }
}
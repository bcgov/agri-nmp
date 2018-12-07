using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agri.Models.Configuration;
using Newtonsoft.Json;

namespace Agri.Models.Farm
{
    public class ManureStorageSystem
    {
        public ManureStorageSystem()
        {
            ManureStorageStructures = new List<ManureStorageStructure>();
            GeneratedManuresIncludedInSystem = new List<GeneratedManure>();
            ImportedManuresIncludedInSystem = new List<ImportedManure>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public ManureMaterialType ManureMaterialType { get; set; }
        public List<GeneratedManure> GeneratedManuresIncludedInSystem { get; set; }
        public List<ImportedManure> ImportedManuresIncludedInSystem { get; set; }
        [JsonIgnore]
        public List<ManagedManure> MaterialsIncludedInSystem
        {
            get
            {
                var manures = new List<ManagedManure>();
                if (GeneratedManuresIncludedInSystem.Any())
                {
                    manures.AddRange(GeneratedManuresIncludedInSystem);
                }

                if (ImportedManuresIncludedInSystem.Any())
                {
                    manures.AddRange(ImportedManuresIncludedInSystem);
                }

                return manures.ToList();
            }
        }
        public bool GetsRunoffFromRoofsOrYards { get; set; }
        public int? RunoffAreaSquareFeet { get; set; }
        public List<ManureStorageStructure> ManureStorageStructures { get; }

        public void AddUpdateManureStorageStructure(ManureStorageStructure manureStorageStructure)
        {
            var savedStructure = ManureStorageStructures.SingleOrDefault(mss => mss.Id == manureStorageStructure.Id);

            if (savedStructure == null)
            {
                manureStorageStructure.Id = ManureStorageStructures.Any()
                    ? ManureStorageStructures.Select(mss => mss.Id).Max() + 1
                    : 0;

                ManureStorageStructures.Add(manureStorageStructure);
            }
            else
            {
                savedStructure.Name = manureStorageStructure.Name;
                savedStructure.UncoveredAreaSquareFeet = manureStorageStructure.UncoveredAreaSquareFeet;
            }
        }

        public void UpdateManureStorageStructure(ManureStorageStructure manureStorageStructure)
        {
            if (manureStorageStructure.Id > 0)
            {
                var savedStructure = ManureStorageStructures.Single(mss => mss.Id == manureStorageStructure.Id);
                savedStructure.Name = manureStorageStructure.Name;
                savedStructure.UncoveredAreaSquareFeet = manureStorageStructure.UncoveredAreaSquareFeet;
            }
        }

        public ManureStorageStructure GetManureStorageStructure(int id)
        {
            return ManureStorageStructures.Single(mss => mss.Id == id);
        }
    }
}

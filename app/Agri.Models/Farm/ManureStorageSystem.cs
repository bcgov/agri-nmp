using System;
using System.Collections.Generic;
using System.Text;
using Agri.Models.Configuration;

namespace Agri.Models.Farm
{
    public class ManureStorageSystem
    {
        public ManureStorageSystem()
        {
            ManureStorageStructures = new List<ManureStorageStructure>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public ManureMaterialType ManureMaterialType { get; set; }
        public List<GeneratedManure> MaterialsIncludedInSystem { get; set; }
        public bool GetsRunoffFromRoofsOrYards { get; set; }
        public bool IncludeRunoff => RooftopsAreaSquareFeetIncludedInRunoff > 0;
        public int RooftopsAreaSquareFeetIncludedInRunoff { get; set; }
        public List<ManureStorageStructure> ManureStorageStructures { get; set; }
    }
}

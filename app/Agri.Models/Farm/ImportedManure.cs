using Agri.Models.Configuration;
using System.Collections.Generic;

namespace Agri.Models.Farm
{
    public class ImportedManure : ManagedManure
    {
        public string MaterialName { get; set; }
        public ManureMaterialType ManureType { get; set; }
        public string ManureTypeName { get; set; }
        public decimal AnnualAmount { get; set; }
        public string Units { get; set; }
        public bool IsLandAppliedBeforeStorage { get; set; }
        public override string ManureId => $"Imported{Id ?? 0}";
    }
}

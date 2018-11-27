using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri.Models.Farm
{
    public class ImportedManure
    {
        public int? id { get; set; }
        public string MaterialName { get; set; }
        public ManureMaterialType ManureType { get; set; }
        public string ManureTypeName { get; set; }
        public string AnnualAmount { get; set; }
        public string Units { get; set; }
        public bool IsLandAppliedBeforeStorage { get; set; }
        public bool AssignedToStoredSystem { get; set; }
    }
}

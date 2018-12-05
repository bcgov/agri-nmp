using Agri.Models.Configuration;
using System.Collections.Generic;

namespace Agri.Models.Farm
{
    public class ImportedManure
    {
        public int? Id { get; set; }
        public string MaterialName { get; set; }
        public ManureMaterialType ManureType { get; set; }
        public string ManureTypeName { get; set; }
        public decimal AnnualAmount { get; set; }
        public string AnnualAmountDisplayVolume { get; set; }
        public string AnnualAmountDisplayWeight { get; set; }
        public AnnualAmountUnits Units { get; set; }
        public decimal? Moisture { get; set; }
        public decimal StandardSolidMoisture { get; set; }
        public bool IsLandAppliedBeforeStorage { get; set; }
        public bool AssignedToStoredSystem { get; set; }
    }
}

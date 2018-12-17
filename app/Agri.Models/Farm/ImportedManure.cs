using Agri.Models.Configuration;
using System.Collections.Generic;

namespace Agri.Models.Farm
{
    public class ImportedManure : ManagedManure
    {
        public string MaterialName { get; set; }
        public string ManureTypeName { get; set; }
        public decimal AnnualAmount { get; set; }
        public decimal AnnualAmountUSGallonsVolume { get; set; }
        public decimal AnnualAmountCubicYardsVolume { get; set; }
        public decimal AnnualAmountCubicMetersVolume { get; set; }
        public decimal AnnualAmountTonsWeight { get; set; }
        public string AnnualAmountDisplayVolume
        {
            get
            {
                if (ManureType == ManureMaterialType.Solid)
                {
                    return $"{string.Format("{0:n0}", AnnualAmountCubicYardsVolume)} yards³ ({string.Format("{0:n0}", AnnualAmountCubicMetersVolume)} m³)";
                }
                else
                {
                    return $"{string.Format("{0:n0}", AnnualAmountUSGallonsVolume)} U.S. gallons";
                }
            }
        }

        public string AnnualAmountDisplayWeight => 
            ManureType == ManureMaterialType.Solid ? $"{string.Format("{0:n0}", AnnualAmountTonsWeight)} tons" : "-";
        public AnnualAmountUnits Units { get; set; }
        public decimal? Moisture { get; set; }
        public decimal StandardSolidMoisture { get; set; }
        public bool IsMaterialStored { get; set; }
        public override string ManureId => $"Imported{Id ?? 0}";
        public override string ManagedManureName => MaterialName;
    }
}

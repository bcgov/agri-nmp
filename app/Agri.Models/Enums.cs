using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Agri.Models
{
    public enum ManureMaterialType
    {
        Liquid = 1,
        Solid = 2
    }

    public enum AnnualAmountUnits
    {
        [Description("yards³")]
        Yards,
        tons,
        [Description("US gallons")]
        USGallons,
        [Description("Imp. gallons")]
        ImperialGallons,
        [Description("m³")]
        CubicMeters
    }
}
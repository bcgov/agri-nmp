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
        Yards = 1,
        tons = 2,
        [Description("US gallons")]
        USGallons = 3,
        [Description("Imp. gallons")]
        ImperialGallons = 4,
        [Description("m³")]
        CubicMeters = 5,
        tonnes = 6
    }

    public enum NutrientAnalysisTypes
    {
        Stored=1,
        Imported=2
    }
}
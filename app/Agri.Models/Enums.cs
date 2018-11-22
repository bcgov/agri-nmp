using System.ComponentModel.DataAnnotations;

namespace Agri.Models
{
    public enum ManureMaterialType
    {
        Liquid = 1,
        Solid = 2,
        [Display(Name = "Solid Liquid Separation")]
        SolidLiquidSeparated = 3
    }
}
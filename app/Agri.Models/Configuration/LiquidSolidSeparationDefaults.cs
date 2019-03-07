using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class LiquidSolidSeparationDefault : Versionable
    {
        [Key]
        public int Id { get; set; }
        public int PercentOfLiquidSeparation { get; set; }
    }
}

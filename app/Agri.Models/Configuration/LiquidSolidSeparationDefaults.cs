using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class LiquidSolidSeparationDefault : ConfigurationBase
    {
        [Key]
        public int Id { get; set; }
        public int PercentOfLiquidSeparation { get; set; }
    }
}

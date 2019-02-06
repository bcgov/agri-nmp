using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class AmmoniaRetention //: ConfigurationBase
    {
        [Key]
        public int SeasonApplicationId { get; set; }
        [Key]
        public int DryMatter { get; set; }
        public decimal? Value { get; set; }
    }
}
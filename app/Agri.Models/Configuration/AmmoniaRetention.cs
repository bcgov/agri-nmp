using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class AmmoniaRetention
    {
        [Key]
        public int SeasonApplicationId { get; set; }
        [Key]
        public int DryMatter { get; set; }
        [Key]
        public int VersionId { get; set; }
        public decimal? Value { get; set; }

        public Version Version { get; set; }
    }
}
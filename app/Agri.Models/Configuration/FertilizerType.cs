using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class FertilizerType : ConfigurationBase
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string DryLiquid { get; set; }
        public bool Custom { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class NutrientIcon : ConfigurationBase
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Definition { get; set; }
    }
}
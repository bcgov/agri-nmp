using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class NutrientIcon
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Definition { get; set; }
    }
}
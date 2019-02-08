using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class HarvestUnit : ConfigurationBase
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class Yield : ConfigurationBase
    {
        [Key]
        public int Id { get; set; }
        public string YieldDesc { get; set; }
    }
}
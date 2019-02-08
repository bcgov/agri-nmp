using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class SoilTestPotassiumRange : ConfigurationBase
    {
        [Key]
        public int UpperLimit { get; set; }
        public string Rating { get; set; }
    }
}
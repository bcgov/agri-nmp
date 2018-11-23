using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class SoilTestPotassiumRange
    {
        [Key]
        public int UpperLimit { get; set; }
        public string Rating { get; set; }
    }
}
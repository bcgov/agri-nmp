using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class SoilTestPotassiumRange : Versionable
    {
        [Key]
        public int UpperLimit { get; set; }
        public string Rating { get; set; }
    }
}
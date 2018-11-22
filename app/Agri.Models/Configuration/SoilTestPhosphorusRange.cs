using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class SoilTestPhosphorusRange
    {
        [Key]
        public int UpperLimit { get; set; }
        public string Rating { get; set; }

    }
}
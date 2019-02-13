using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agri.Models.Configuration
{
    public class SoilTestRange : ConfigurationBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int LowerLimit { get; set; }
        public int UpperLimit { get; set; }
        public string Rating { get; set; }
    }
}
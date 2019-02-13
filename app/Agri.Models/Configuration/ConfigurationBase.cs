using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class ConfigurationBase
    {
        [Key]
        public int StaticDataVersionId { get; set; }
        public StaticDataVersion Version { get; set; }
    }
}

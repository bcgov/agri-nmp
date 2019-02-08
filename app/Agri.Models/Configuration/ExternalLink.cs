using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class ExternalLink : ConfigurationBase
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
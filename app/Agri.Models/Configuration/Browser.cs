using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class Browser : ConfigurationBase
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string MinVersion { get; set; }
    }
}
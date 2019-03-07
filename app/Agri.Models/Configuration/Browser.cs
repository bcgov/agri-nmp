using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class Browser
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string MinVersion { get; set; }
    }
}
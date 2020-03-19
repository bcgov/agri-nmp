using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class Depth
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Value { get; set; }
    }
}
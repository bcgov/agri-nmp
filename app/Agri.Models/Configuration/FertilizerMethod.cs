using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class FertilizerMethod : Versionable
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
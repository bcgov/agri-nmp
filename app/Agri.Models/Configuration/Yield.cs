using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class Yield : Versionable
    {
        [Key]
        public int Id { get; set; }
        public string YieldDesc { get; set; }
    }
}
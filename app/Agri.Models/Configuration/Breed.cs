using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class Breed : ConfigurationBase
    {
        [Key]
        public int Id { get; set; }
        public string BreedName { get; set; }
        public int AnimalId { get; set; }
        public decimal BreedManureFactor { get; set; }
        public Animal Animal { get; set; }
    }
}

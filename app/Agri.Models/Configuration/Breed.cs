using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Agri.Models.Configuration
{
    public class Breed : Versionable
    {
        [Key]
        public int Id { get; set; }

        public string BreedName { get; set; }
        public int AnimalId { get; set; }
        public decimal BreedManureFactor { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public Animal Animal { get; set; }
    }
}
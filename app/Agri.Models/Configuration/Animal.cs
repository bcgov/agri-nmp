using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class Animal : ConfigurationBase
    {
        public Animal()
        {
            AnimalSubTypes = new List<AnimalSubType>();
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string UseSortOrder { get; set; }
        public List<Breed> Breeds { get; set; }
        public List<AnimalSubType> AnimalSubTypes { get; set; }
    }
}

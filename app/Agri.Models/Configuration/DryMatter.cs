using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class DryMatter : ConfigurationBase
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Manure> Manures { get; set; }
    }
}
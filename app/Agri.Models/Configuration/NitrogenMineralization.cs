using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class NitrogenMineralization : ConfigurationBase
    {
        public NitrogenMineralization()
        {
            Manures = new List<Manure>();
        }
        [Key]
        public int Id { get; set; }
        [Key]
        public int LocationId { get; set; }
        public string Name { get; set; }
        public decimal FirstYearValue { get; set; }
        public decimal LongTermValue { get; set; }

        public Location Location { get; set; }
        public List<Manure> Manures { get; set; }
    }
}
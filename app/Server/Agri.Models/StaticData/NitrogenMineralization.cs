using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agri.Models.StaticData
{
    public class NitrogenMineralization
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

        public List<Manure> Manures { get; set; }
    }
}
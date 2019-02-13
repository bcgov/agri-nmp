using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class NitrogenMineralization : Versionable
    {
        [Key]
        public int Id { get; set; }
        [Key]
        public int LocationId { get; set; }
        public string Name { get; set; }
        public decimal FirstYearValue { get; set; }
        public decimal LongTermValue { get; set; }

        public Location Location { get; set; }
    }
}
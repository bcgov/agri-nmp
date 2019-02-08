using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class SeasonApplication : ConfigurationBase
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Season { get; set; }
        public string ApplicationMethod { get; set; }
        public decimal DryMatterLessThan1Percent { get; set; }
        public decimal DryMatter1To5Percent { get; set; }
        public decimal DryMatter5To10Percent { get; set; }
        public decimal DryMatterGreaterThan10Percent { get; set; }
        public string PoultrySolid { get; set; }
        public string Compost { get; set; }
        public int SortNum { get; set; }
        public string ManureType { get; set; }
    }
}
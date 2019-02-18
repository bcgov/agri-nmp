using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class PreviousCropType : Versionable
    {
        [Key]
        public int Id { get; set; }
        public int PreviousCropCode { get; set; }
        public string Name { get; set; }
        public int NitrogenCreditMetric { get; set; }  //Not being used
        public int NitrogenCreditImperial { get; set; }
    }
}
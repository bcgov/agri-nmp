using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class NitrateCreditSampleDate : ConfigurationBase
    {
        [Key]
        public int Id { get; set; }
        public string Location { get; set; }
        public string FromDateMonth { get; set; }
        public string ToDateMonth { get; set; }
    }
}

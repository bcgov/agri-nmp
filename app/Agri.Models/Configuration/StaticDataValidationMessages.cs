using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class StaticDataValidationMessages
    {
        [Key]
        public string Child { get; set; }
        public string Parent { get; set; }
        public string LinkData { get; set; }        
    }
}

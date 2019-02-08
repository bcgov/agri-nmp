using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class ManureImportedDefault : ConfigurationBase
    {
        [Key]
        public int Id { get; set; }
        public decimal DefaultSolidMoisture { get; set; }
    }
}

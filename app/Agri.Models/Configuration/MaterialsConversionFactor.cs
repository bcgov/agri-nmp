using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class MaterialsConversionFactor : ConfigurationBase
    {
        [Key]
        public int Id { get; set; }
        public AnnualAmountUnits InputUnit { get; set; }
        public string InputUnitName { get; set; }
    }
}

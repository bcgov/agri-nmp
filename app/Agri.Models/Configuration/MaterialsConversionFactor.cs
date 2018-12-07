using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri.Models.Configuration
{
    public class MaterialsConversionFactor
    {
        public int Id { get; set; }
        public AnnualAmountUnits InputUnit { get; set; }
        public string InputUnitName { get; set; }
    }
}

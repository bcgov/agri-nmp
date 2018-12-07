using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri.Models
{
    public class SolidMaterialApplicationTonPerAcreRateConversion
    {
        public int Id { get; set; }
        public ApplicationRateUnits ApplicationRateUnits { get; set; }
        public string ApplicationRateUnitName { get; set; }
        public string TonsPerAcreConversion { get; set; }
    }
}

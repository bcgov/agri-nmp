using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri.Models.Configuration
{
    public class ApplicationPerAcreConversion
    {
        public int Id { get; set; }
        public ApplicationRateUnits ApplicationRateUnit { get; set; }
        public string ApplicationRateUnitName { get; set; }
    }
}

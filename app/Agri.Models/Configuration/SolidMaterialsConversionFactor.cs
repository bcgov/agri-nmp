using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri.Models.Configuration
{
    public class SolidMaterialsConversionFactor : MaterialsConversionFactor
    {
        public string CubicYardsOutput { get; set; }
        public string CubicMetersOutput { get; set; }
        public string MetricTonsOutput { get; set; }
    }
}

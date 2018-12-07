using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri.Models.Configuration
{
    public class SolidMaterialsConversionFactor : MaterialsConversionFactor
    {
        public decimal CubicYardsOutput { get; set; }
        public decimal CubicMetersOutput { get; set; }
        public decimal MetricTonsOutput { get; set; }
    }
}

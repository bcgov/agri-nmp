using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri.Models.Farm
{
    public class SeparatedSolidManure : ManagedManure
    {
        public string Name { get; set; }
        public override ManureMaterialType ManureType => ManureMaterialType.Solid;
        public decimal AnnualAmountTonsWeight { get; set; }
        public override string ManureId => $"SeparatedSolid{Id ?? 0}";
        public override string ManagedManureName => Name;
    }
}

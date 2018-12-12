using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri.Models.Farm
{
    public interface IAppliableManure
    {
        ApplicationRateUnits ApplicationRateUnit { get; set; }
        int AmountApplied { get; set; }
    }
}

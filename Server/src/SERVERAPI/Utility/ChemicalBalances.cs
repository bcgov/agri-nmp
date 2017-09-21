using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERVERAPI.Utility
{
    public class ChemicalBalances
    {
        public int balance_AgrN { get; set; }
        public int balance_AgrP2O5 { get; set; }
        public int balance_AgrK2O { get; set; }
        public int balance_CropN { get; set; }
        public int balance_CropP2O5 { get; set; }
        public int balance_CropK2O { get; set; }
    }
}

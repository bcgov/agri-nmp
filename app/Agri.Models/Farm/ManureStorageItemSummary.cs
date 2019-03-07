using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri.Models.Farm
{
    public class ManureStorageItemSummary
    {
        private AnnualAmountUnits _annualAmountUnit;
        public ManureStorageItemSummary(ManagedManure managedManure, decimal itemTotalAnnualStored, AnnualAmountUnits annualAmountUnit)
        {
            ManagedManure = managedManure;
            ItemTotalAnnualStored = itemTotalAnnualStored;
            _annualAmountUnit = annualAmountUnit;
        }
        public ManagedManure ManagedManure { get; private set; }

        public decimal ItemTotalAnnualStored { get; private set; }

        public string AnnualAmountUnit => EnumHelper<AnnualAmountUnits>.GetDisplayValue(_annualAmountUnit);

        public decimal GetPercentageOfTotalStorageGeneratedManure(decimal annualTotalStoredGeneratedManure)
        {
            var percentOfTotalAnnualAmount = annualTotalStoredGeneratedManure > 0
                ? (ItemTotalAnnualStored / annualTotalStoredGeneratedManure * 100)
                : 0;
            return percentOfTotalAnnualAmount;
        }
    }
}

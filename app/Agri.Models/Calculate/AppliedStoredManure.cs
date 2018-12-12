using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agri.Models.Farm;

namespace Agri.Models.Calculate
{
    public class AppliedStoredManure : AppliedManure
    {
        public AppliedStoredManure(List<FieldAppliedManure> fieldAppliedManures,
            ManureStorageSystem manureStorageSystem) :
            base(fieldAppliedManures)
        {
            ManureStorageSystem = manureStorageSystem;
        }

        public ManureStorageSystem ManureStorageSystem { get; private set; }

        public override ManureMaterialType? ManureMaterialType => ManureStorageSystem?.ManureMaterialType;

        public override string SourceName => ManureStorageSystem.Name;

        public override decimal TotalAnnualManureToApply => ManureStorageSystem.AnnualTotalAmountofManureInStorage;

    }
}

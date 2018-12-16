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

        public List<string> ListUnallocatedMaterialAsPercentOfTotalStored
        {
            get
            {
                var result = new List<string>();
                foreach (var storedMaterial in ManureStorageSystem.ManureStorageItemSummaries)
                {
                    var percentOfTotalStored =
                        storedMaterial.GetPercentageOfTotalStorageGeneratedManure(ManureStorageSystem
                            .AnnualTotalAmountofManureInStorage);

                    var unallocated =
                        $"Storage System: {ManureStorageSystem.Name}, Material: {storedMaterial.ManagedManure.ManagedManureName} ";
                    unallocated += $"Unallocated {storedMaterial.ItemTotalAnnualStored} {storedMaterial.AnnualAmountUnit} - {percentOfTotalStored}% of Total Stored";
                    result.Add(unallocated);
                }

                return result;
            }
        }
    }
}

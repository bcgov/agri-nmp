using System.Collections.Generic;
using System.Linq;
using Agri.Models.Farm;

namespace SERVERAPI.ViewModels
{
    public class ManureStorageViewModel
    {
        public List<GeneratedManure> GeneratedManures { get; set; }
        public List<ManureStorageSystem> ManureStorageSystems { get; set; }

        public string ExplainMaterialsNeedingStorageMessage { get; set; }

        public bool DisableAddStorageSystemButton
        {
            get { return GeneratedManures.All(gm => gm.AssignedToStoredSystem); }
        }
    }
}

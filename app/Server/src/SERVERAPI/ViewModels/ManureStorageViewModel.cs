using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SERVERAPI.Models;

namespace SERVERAPI.ViewModels
{
    public class ManureStorageViewModel
    {
        public List<GeneratedManure> GeneratedManures { get; set; }
        public List<ManureStorageSystem> ManureStorageSystems { get; set; }

        public bool DisableAddButtons
        {
            get { return GeneratedManures.All(gm => gm.AssignedToStoredSystem); }
        }
    }
}

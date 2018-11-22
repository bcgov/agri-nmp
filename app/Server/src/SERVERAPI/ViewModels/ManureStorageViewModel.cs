using System.Collections.Generic;
using Agri.Models.Farm;

namespace SERVERAPI.ViewModels
{
    public class ManureStorageViewModel
    {
        public List<GeneratedManure> GeneratedManures { get; set; }
        public List<ManureStorageSystem> ManureStorageSystems { get; set; }
    }
}

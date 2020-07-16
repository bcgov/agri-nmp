using System.Collections.Generic;
using System.Linq;
using Agri.Models.Calculate;
using Agri.Models.Farm;

namespace SERVERAPI.ViewModels
{
    public class ReportViewModel
    {
        public string Body { get; set; }
        public string RepeatFooter { get; set; }
        public bool UnsavedData { get; set; }
        public string Url { get; set; }
        public string NoCropsMsg { get; set; }
        public string DownloadMsg { get; set; }
        public string LoadMsg { get; set; }
        public string MaterialsNotStoredMessage { get; set; }
        public List<GeneratedManure> GeneratedManures { get; set; }
        public List<ImportedManure> ImportedManures { get; set; }

        public List<ManagedManure> StorableManures
        {
            get
            {
                var manures = new List<ManagedManure>();
                if (GeneratedManures.Any())
                {
                    manures.AddRange(GeneratedManures);
                }

                if (ImportedManures.Any())
                {
                    manures.AddRange(ImportedManures.Where(im => im.IsMaterialStored));
                }

                return manures;
            }
        }

        public List<ManagedManure> UnallocatedManures => StorableManures.Where(mm => !mm.AssignedToStoredSystem).ToList();

        public List<AppliedManure> RemainingManures { get; set; }
        public List<AppliedManure> OverUtilizedManures { get; set; }
        public string MaterialsRemainingMessage { get; set; }
        public string OverUtilizedManuresMessage { get; set; }
    }
}
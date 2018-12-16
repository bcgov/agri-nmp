using System.Collections.Generic;
using System.Linq;
using Agri.Models.Calculate;
using Agri.Models.Farm;

namespace SERVERAPI.ViewModels
{
    public class ReportViewModel
    {
        public bool fields { get; set; }
        public bool nutrientSources { get; set; }
        public bool nutrientApplicationSchedule { get; set; }
        public bool nutrientSourceAnalysis { get; set; }
        public bool soilTestSummary { get; set; }
        public bool recordKeepingSheets { get; set; }
        public bool unsavedData { get; set; }
        public string url { get; set; }
        public string noCropsMsg { get; set; }
        public string downloadMsg { get; set; }
        public string loadMsg { get; set; }
        public string materialsNotStoredMessage { get; set; }
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
        public List<string> UnallocatedManureNames => StorableManures.Where(mm => !mm.AssignedToStoredSystem).Select(mm => mm.ManagedManureName).ToList();

        public List<AppliedManure> AppliedManures { get; set; }
    }
}
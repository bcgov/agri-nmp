using Agri.Data;
using Agri.Models.Farm;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Models.Impl;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERVERAPI.ViewComponents
{
    public class Compost : ViewComponent
    {
        private UserData _ud;
        private IAgriConfigurationRepository _sd;

        public Compost(UserData ud, IAgriConfigurationRepository sd)
        {
            _ud = ud;
            _sd = sd;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await GetCompostAsync());
        }

        private Task<CompostViewModel> GetCompostAsync()
        {
            CompostViewModel fvm = new CompostViewModel();
            fvm.composts = new List<FarmManure>();

            fvm.GeneratedManures = _ud.GetGeneratedManures();
            fvm.ImportedManures = _ud.GetImportedManures();
            fvm.StorageSystems = _ud.GetStorageSystems();

            fvm.compostMsg = _sd.GetUserPrompt("compostmessage");
            fvm.ExplainMaterialsNeedingNutrientAnalysisMessage = _sd.GetUserPrompt("nutrientAnalysisForMaterialsMessage");

            List<FarmManure> compostList = _ud.GetFarmManures();

            foreach (var f in compostList)
            {
                fvm.composts.Add(f);
            }

            return Task.FromResult(fvm);
        }
    }

    public class CompostViewModel
    {
        public List<FarmManure> composts { get; set; }
        public string compostMsg { get; set; }
        public string ExplainMaterialsNeedingNutrientAnalysisMessage { get; set; }
        public List<ManureStorageSystem> StorageSystems { get; set; }
        public List<GeneratedManure> GeneratedManures { get; set; }
        public List<ImportedManure> ImportedManures { get; set; }

        public List<ManagedManure> ImportedMaterialsNotStored
        {
            get
            {
                var manures = new List<ManagedManure>();
                if (ImportedManures.Any())
                {
                    manures.AddRange(ImportedManures.Where(im => !im.IsMaterialStored && !im.AssignedWithNutrientAnalysis));
                }

                return manures;
            }
        }

        public List<string> UnallocatedStorageAndImportedManureNames => (ImportedMaterialsNotStored.Select(mm => mm.ManagedManureName)
            .Concat(StorageSystems.Where(ss => !ss.AssignedWithNutrientAnalysis).Select(ss => ss.Name))).ToList();
    }
}
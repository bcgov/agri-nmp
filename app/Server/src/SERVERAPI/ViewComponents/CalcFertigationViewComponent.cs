using Agri.Data;
using Agri.Models.Configuration;
using Agri.Models.Farm;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace SERVERAPI.ViewComponents
{
    public class CalcFertigationViewComponent : ViewComponent
    {
        private IAgriConfigurationRepository _sd;
        private Models.Impl.UserData _ud;

        public CalcFertigationViewComponent(IAgriConfigurationRepository sd, Models.Impl.UserData ud)
        {
            _sd = sd;
            _ud = ud;
        }

        public async Task<IViewComponentResult> InvokeAsync(string fldName)
        {
            return View(await GetFertigationAsync(fldName));
        }

        private Task<CalcFertigationViewModel> GetFertigationAsync(string fldName)
        {
            CalcFertigationViewModel fgvm = new CalcFertigationViewModel();
            fgvm.fldFertilizers = new List<DisplayNutrientFertigation>();

            List<NutrientFertilizer> fldFertilizers = _ud.GetFieldNutrientsFertilizers(fldName);
            foreach (var f in fldFertilizers.Where(f => f.isFertigation))
            {
                DisplayNutrientFertigation dm = new DisplayNutrientFertigation();

                FertilizerType ft = _sd.GetFertilizerType(f.fertilizerTypeId.ToString());

                string fertilizerName;
                if (ft.Custom)
                {
                    fertilizerName = ft.DryLiquid == "dry" ? "Custom (Dry) " : "Custom (Liquid) ";
                    fertilizerName = fertilizerName + f.customN.ToString() + "-" + f.customP2o5.ToString() + "-" + f.customK2o.ToString();
                }
                else
                {
                    Fertilizer ff = _sd.GetFertilizer(f.fertilizerId.ToString());
                    fertilizerName = ff.Name;
                }

                dm.fldName = fldName;
                dm.fertilizerId = f.id;
                dm.fertilizerName = fertilizerName;
                dm.valN = f.fertN.ToString("G29");
                dm.valP = f.fertP2o5.ToString("G29");
                dm.valK = f.fertK2o.ToString("G29");
                dm.isFertigation = true;
                dm.eventsPerSeason = f.eventsPerSeason;

                fgvm.fldFertilizers.Add(dm);
            }

            return Task.FromResult(fgvm);
        }
    }
    public class CalcFertigationViewModel
{
    public List<DisplayNutrientFertigation> fldFertilizers { get; set; }
}

public class DisplayNutrientFertigation
{
    public string fldName { get; set; }
    public int fertilizerId { get; set; }
    public string fertilizerName { get; set; }
    public string valN { get; set; }
    public string valP { get; set; }
    public string valK { get; set; }
    public bool isFertigation { get; set; }
    public int eventsPerSeason { get; set; }
}
}
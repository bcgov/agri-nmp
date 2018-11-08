using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Controllers;
using SERVERAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agri.Models.Farm;
using static SERVERAPI.Models.StaticData;

namespace SERVERAPI.ViewComponents
{
    public class CalcFertilizer : ViewComponent
    {
        private Models.Impl.StaticData _sd;
        private Models.Impl.UserData _ud;

        public CalcFertilizer(Models.Impl.StaticData sd, Models.Impl.UserData ud)
        {
            _sd = sd;
            _ud = ud;
        }


        public async Task<IViewComponentResult> InvokeAsync(string fldName)
        {
            return View(await GetFertilizerAsync(fldName));
        }

        private Task<CalcFertilizerViewModel> GetFertilizerAsync(string fldName)
        {
            string fertilizerName = string.Empty;
            CalcFertilizerViewModel fvm = new CalcFertilizerViewModel();
            fvm.fldFertilizers = new List<DisplayNutrientFertilizer>();

            List<NutrientFertilizer> fldFertilizers = _ud.GetFieldNutrientsFertilizers(fldName);
            foreach (var f in fldFertilizers)
            {
                DisplayNutrientFertilizer dm = new DisplayNutrientFertilizer();

                FertilizerType ft = _sd.GetFertilizerType(f.fertilizerTypeId.ToString());

                if(ft.custom)
                {
                    fertilizerName = ft.dry_liquid == "dry" ? "Custom (Dry) " : "Custom (Liquid) ";
                    fertilizerName = fertilizerName + f.customN.ToString() + "-" + f.customP2o5.ToString() + "-" + f.customK2o.ToString();
                }
                else
                {
                    Fertilizer ff = _sd.GetFertilizer(f.fertilizerId.ToString());
                    fertilizerName = ff.name;
                }

                dm.fldName = fldName;
                dm.fertilizerId = f.id;
                dm.fertilizerName = fertilizerName;
                dm.valN = f.fertN.ToString();
                dm.valP = f.fertP2o5.ToString();
                dm.valK = f.fertK2o.ToString();

                fvm.fldFertilizers.Add(dm);
            }

            return Task.FromResult(fvm);
        }
    }
    public class CalcFertilizerViewModel
    {
        public List<DisplayNutrientFertilizer> fldFertilizers { get; set; }
    }
    public class DisplayNutrientFertilizer
    {
        public string fldName { get; set; }
        public int fertilizerId { get; set; }
        public string fertilizerName { get; set; }
        public string valN { get; set; }
        public string valP { get; set; }
        public string valK { get; set; }
    }
}

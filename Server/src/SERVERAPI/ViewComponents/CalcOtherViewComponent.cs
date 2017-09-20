using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Controllers;
using SERVERAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERVERAPI.ViewComponents
{
    public class CalcOther : ViewComponent
    {
        private Models.Impl.StaticData _sd;
        private Models.Impl.UserData _ud;

        public CalcOther(Models.Impl.StaticData sd, Models.Impl.UserData ud)
        {
            _sd = sd;
            _ud = ud;
        }

        public async Task<IViewComponentResult> InvokeAsync(string fldName)
        {
            return View(await GetOtherAsync(fldName));
        }

        private Task<CalcOtherViewModel> GetOtherAsync(string fldName)
        {
            CalcOtherViewModel ovm = new CalcOtherViewModel();
            ovm.fldName = fldName;

            ovm.others = new List<DisplayNutrientOther>();

            List<NutrientOther> fldOthers = _ud.GetFieldNutrientsOthers(fldName);
            foreach (var m in fldOthers)
            {
                DisplayNutrientOther no = new DisplayNutrientOther()
                {
                    otherId = m.id,
                    description = m.description,
                    nitrogen = m.nitrogen,
                    phospherous = m.phospherous,
                    potassium = m.potassium
                };
                ovm.others.Add(no);
            }

            return Task.FromResult(ovm);
        }
    }
    public class CalcOtherViewModel
    {
        public string fldName { get; set; }
        public List<DisplayNutrientOther> others { get; set; }
    }
    public class DisplayNutrientOther
    {
        public int otherId { get; set; }
        public string description { get; set; }
        public decimal nitrogen { get; set; }
        public decimal phospherous { get; set; }
        public decimal potassium { get; set; }
    }
}
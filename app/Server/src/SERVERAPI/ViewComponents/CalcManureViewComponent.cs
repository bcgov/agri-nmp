using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Controllers;
using SERVERAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agri.Models.Farm;

namespace SERVERAPI.ViewComponents
{
    public class CalcManure : ViewComponent
    {
        private Models.Impl.StaticData _sd;
        private Models.Impl.UserData _ud;

        public CalcManure(Models.Impl.StaticData sd, Models.Impl.UserData ud)
        {
            _sd = sd;
            _ud = ud;
        }

        public async Task<IViewComponentResult> InvokeAsync(string fldName)
        {
            return View(await GetManureAsync(fldName));
        }

        private Task<CalcManureViewModel> GetManureAsync(string fldName)
        {
            CalcManureViewModel mvm = new CalcManureViewModel();
            mvm.manures = new List<DisplayNutrientManure>();

            List<NutrientManure> fldManures = _ud.GetFieldNutrientsManures(fldName);
            foreach (var m in fldManures)
            {
                DisplayNutrientManure dm = new DisplayNutrientManure()
                {
                    fldName = fldName,
                    manId = m.id,
                    matType = _ud.GetFarmManure(Convert.ToInt32(m.manureId)).name,
                    applType = _sd.GetApplication(m.applicationId).name,
                    rate = m.rate.ToString() + " " + _sd.GetUnit(m.unitId).name,
                    yrN = m.yrN.ToString("G29"),
                    yrP = m.yrP2o5.ToString("G29"),
                    yrK = m.yrK2o.ToString("G29"),
                    ltN = m.ltN.ToString("G29"),
                    ltP = m.ltP2o5.ToString("G29"),
                    ltK = m.ltK2o.ToString("G29"),
                };
                mvm.manures.Add(dm);
            }

            return Task.FromResult(mvm);
        }
    }
    public class CalcManureViewModel
    {
        public List<DisplayNutrientManure> manures { get; set; }
    }
    public class DisplayNutrientManure
    {
        public string fldName { get; set; }
        public int manId { get; set; }
        public string matType { get; set; }
        public string applType { get; set; }
        public string rate { get; set; }
        public string yrN { get; set; }
        public string yrP { get; set; }
        public string yrK { get; set; }
        public string ltN { get; set; }
        public string ltP { get; set; }
        public string ltK { get; set; }
    }
}
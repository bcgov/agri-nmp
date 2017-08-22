using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Controllers;
using SERVERAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERVERAPI.ViewComponents
{
    public class CalcManure : ViewComponent
    {
        private Models.Impl.StaticData _sd;

        public CalcManure(Models.Impl.StaticData sd)
        {
            _sd = sd;
        }

        public async Task<IViewComponentResult> InvokeAsync(string fldName)
        {
            return View(await GetManureAsync(fldName));
        }

        private Task<CalcManureViewModel> GetManureAsync(string fldName)
        {
            CalcManureViewModel mvm = new CalcManureViewModel();
            mvm.manures = new List<DisplayNutrientManure>();

            var farmData = HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");

            if (farmData.years != null)
            {
                YearData yd = farmData.years.FirstOrDefault(y => y.year == farmData.farmDetails.year);
                if (yd.fields == null)
                {
                    yd.fields = new List<Field>();
                }
                Field fld = yd.fields.FirstOrDefault(f => f.fieldName == fldName);
                if (fld == null)
                {
                    fld = new Field();
                }
                if (fld.nutrients == null)
                {
                    fld.nutrients = new Nutrients();
                }
                List<NutrientManure> fldManures = fld.nutrients.nutrientManures;
                if (fldManures == null)
                {
                    fldManures = new List<NutrientManure>();
                }

                foreach (var m in fldManures)
                {
                    DisplayNutrientManure dm = new DisplayNutrientManure()
                    {
                        fldName = fldName,
                        manId = m.id,
                        matType = _sd.GetManure(m.manureId).name,
                        applType = _sd.GetApplication(m.applicationId).name,
                        rate = m.rate.ToString() + " " + _sd.GetUnit(m.unitId).name,
                        yrN = m.yrN.ToString(),
                        yrP = m.yrP2o5.ToString(),
                        yrK = m.yrK2o.ToString(),
                        ltN = m.ltN.ToString(),
                        ltP = m.ltP2o5.ToString(),
                        ltK = m.ltK2o.ToString(),
                    };
                    mvm.manures.Add(dm);
                }
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
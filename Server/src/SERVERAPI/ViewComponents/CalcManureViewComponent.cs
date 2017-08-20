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
        public async Task<IViewComponentResult> InvokeAsync(string fldName)
        {
            return View(await GetManureAsync(fldName));
        }

        private Task<CalcManureViewModel> GetManureAsync(string fldName)
        {
            Models.Impl.StaticData sd = new Models.Impl.StaticData();
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
                if(fld == null)
                {
                    fld = new Field();
                }
                if (fld.nutrients == null)
                {
                    fld.nutrients = new Nutrients();
                }
                List<NutrientManure> fldManures = fld.nutrients.nutrientManures;
                if(fldManures == null)
                {
                    fldManures = new List<NutrientManure>();
                }

                foreach (var m in fldManures)
                {
                    DisplayNutrientManure dm = new DisplayNutrientManure()
                    {
                        matType = sd.GetManure(HttpContext, m.manureId).name,
                        applType = sd.GetApplication(HttpContext, m.applicationId).name,
                        rate = m.rate.ToString()
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
        public string matType { get; set; }
        public string applType { get; set; }
        public string rate { get; set; }
    }
}

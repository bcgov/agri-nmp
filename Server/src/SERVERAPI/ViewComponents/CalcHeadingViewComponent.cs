using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERVERAPI.ViewComponents
{
    public class CalcHeading : ViewComponent
    {
        private Models.Impl.StaticData _sd;
        private Models.Impl.UserData _ud;

        public CalcHeading(Models.Impl.StaticData sd, Models.Impl.UserData ud)
        {
            _sd = sd;
            _ud = ud;
        }
        public async Task<IViewComponentResult> InvokeAsync(string fldName)
        {
            return View(await GetHeadingAsync(fldName));
        }

        private Task<CalcHeadingViewModel> GetHeadingAsync(string fldName)
        {
            CalcHeadingViewModel cvm = new CalcHeadingViewModel();
            cvm.headingReqd = false;

            List<FieldCrop> crps = _ud.GetFieldCrops(fldName);
            if (crps.Count() > 0)
                cvm.headingReqd = true;

            List<NutrientManure> manures = _ud.GetFieldNutrientsManures(fldName);
            if (manures.Count() > 0)
                cvm.headingReqd = true;

            return Task.FromResult(cvm);
        }
    }
    public class CalcHeadingViewModel
    {
        public bool headingReqd { get; set; }
    }
}

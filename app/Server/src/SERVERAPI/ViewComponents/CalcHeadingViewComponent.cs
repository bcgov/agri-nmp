﻿using Agri.Data;
using Agri.Models.Farm;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERVERAPI.ViewComponents
{
    public class CalcHeading : ViewComponent
    {
        private IAgriConfigurationRepository _sd;
        private Models.Impl.UserData _ud;

        public CalcHeading(IAgriConfigurationRepository sd, Models.Impl.UserData ud)
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

            List<NutrientFertilizer> fertilizers = _ud.GetFieldNutrientsFertilizers(fldName);
            if (fertilizers.Count() > 0)
                cvm.headingReqd = true;

            List<NutrientOther> others = _ud.GetFieldNutrientsOthers(fldName);
            if (others.Count() > 0)
                cvm.headingReqd = true;

            cvm.unsavedData = _ud.FarmData().unsaved;

            return Task.FromResult(cvm);
        }
    }

    public class CalcHeadingViewModel
    {
        public bool headingReqd { get; set; }
        public string urlAgri { get; set; }
        public string urlCrop { get; set; }
        public bool unsavedData { get; set; }
    }
}
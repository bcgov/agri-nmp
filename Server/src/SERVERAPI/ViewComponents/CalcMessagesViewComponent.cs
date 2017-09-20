using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERVERAPI.ViewComponents
{
    public class CalcMessages : ViewComponent
    {
        private Models.Impl.StaticData _sd;
        private Models.Impl.UserData _ud;

        public CalcMessages(Models.Impl.StaticData sd, Models.Impl.UserData ud)
        {
            _sd = sd;
            _ud = ud;
        }

        public async Task<IViewComponentResult> InvokeAsync(string fldName)
        {
            return View(await GetMessagesAsync(fldName));
        }

        private Task<CalcOtherViewModel> GetMessagesAsync(string fldName)
        {
            CalcOtherViewModel ovm = new CalcOtherViewModel();
            ovm.fldName = fldName;

            ovm.others = new List<DisplayNutrientOther>();

            return Task.FromResult(ovm);
        }
    }
}

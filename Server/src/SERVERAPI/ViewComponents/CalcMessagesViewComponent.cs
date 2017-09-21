using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Utility;
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
        public IHostingEnvironment _env { get; set; }

        public CalcMessages(Models.Impl.StaticData sd, Models.Impl.UserData ud, IHostingEnvironment env)
        {
            _sd = sd;
            _ud = ud;
            _env = env;
        }

        public async Task<IViewComponentResult> InvokeAsync(string fldName)
        {
            return View(await GetMessagesAsync(fldName));
        }

        private Task<CalcMessagesViewModel> GetMessagesAsync(string fldName)
        {
            CalcMessagesViewModel cvm = new CalcMessagesViewModel();

            ChemicalBalanceMessage cbm = new ChemicalBalanceMessage(_ud, _sd);
            cvm.messages = null;

            cvm.messages = cbm.DetermineBalanceMessages(fldName);

            return Task.FromResult(cvm);
        }
    }
    public class CalcMessagesViewModel
    {
        public List<string> messages { get; set; }
    }
}

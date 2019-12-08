using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Agri.Interfaces;
using Agri.Models.Calculate;
using Agri.CalculateService;
using SERVERAPI.Models.Impl;

namespace SERVERAPI.ViewComponents
{
    public class CalcMessages : ViewComponent
    {
        private readonly IAgriConfigurationRepository _sd;
        private readonly UserData _ud;
        private readonly IHostingEnvironment _env;
        private readonly IChemicalBalanceMessage _chemicalBalanceMessage;

        public CalcMessages(IAgriConfigurationRepository sd,
            UserData ud,
            IHostingEnvironment env,
            IChemicalBalanceMessage chemicalBalanceMessage)
        {
            _sd = sd;
            _ud = ud;
            _env = env;
            _chemicalBalanceMessage = chemicalBalanceMessage;
        }

        public async Task<IViewComponentResult> InvokeAsync(string fldName)
        {
            return View(await GetMessagesAsync(fldName));
        }

        private Task<CalcMessagesViewModel> GetMessagesAsync(string fldName)
        {
            CalcMessagesViewModel cvm = new CalcMessagesViewModel();

            cvm.messages = null;

            var field = _ud.GetFieldDetails(fldName);
            cvm.messages = _chemicalBalanceMessage.DetermineBalanceMessages(field, _ud.FarmDetails().FarmRegion.Value, _ud.FarmDetails().Year);
            cvm.displayMsgs = _chemicalBalanceMessage.DisplayMessages(field);

            return Task.FromResult(cvm);
        }
    }

    public class CalcMessagesViewModel
    {
        public List<BalanceMessages> messages { get; set; }

        public bool displayMsgs { get; set; }
    }
}
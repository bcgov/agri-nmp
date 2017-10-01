using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SERVERAPI.Utility;

namespace SERVERAPI.ViewComponents
{
    public class CalcSummary : ViewComponent
    {
        private Models.Impl.StaticData _sd;
        private Models.Impl.UserData _ud;

        public CalcSummary(Models.Impl.StaticData sd, Models.Impl.UserData ud)
        {
            _sd = sd;
            _ud = ud;
        }
        public async Task<IViewComponentResult> InvokeAsync(string fldName)
        {
            return View(await GetSummaryAsync(fldName));
        }

        private Task<CalcSummaryViewModel> GetSummaryAsync(string fldName)
        {
            CalcSummaryViewModel cvm = new CalcSummaryViewModel();
            cvm.summaryReqd = false;

            ChemicalBalanceMessage cbm = new ChemicalBalanceMessage(_ud, _sd);
            ChemicalBalances chemicalBalances = new ChemicalBalances();

            chemicalBalances = cbm.GetChemicalBalances(fldName);

            cvm.reqN = chemicalBalances.balance_AgrN.ToString();
            cvm.reqP = chemicalBalances.balance_AgrP2O5.ToString();
            cvm.reqK = chemicalBalances.balance_AgrK2O.ToString();
            cvm.remN = chemicalBalances.balance_CropN.ToString();
            cvm.remP = chemicalBalances.balance_CropP2O5.ToString();
            cvm.remK = chemicalBalances.balance_CropK2O.ToString();
            cvm.summaryReqd = cbm.displayBalances;

            return Task.FromResult(cvm);
        }
    }
    public class CalcSummaryViewModel
    {
        public bool summaryReqd { get; set; }
        public string reqN { get; set; }
        public string reqP { get; set; }
        public string reqK { get; set; }
        public string remN { get; set; }
        public string remP { get; set; }
        public string remK { get; set; }
    }
}

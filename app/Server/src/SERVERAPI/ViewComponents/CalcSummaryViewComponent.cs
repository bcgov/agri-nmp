using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agri.Interfaces;
using Agri.Models.Calculate;
using SERVERAPI.Utility;

namespace SERVERAPI.ViewComponents
{
    public class CalcSummary : ViewComponent
    {
        private IAgriConfigurationRepository _sd;
        private Models.Impl.UserData _ud;

        public CalcSummary(IAgriConfigurationRepository sd, Models.Impl.UserData ud)
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

            List<BalanceMessages> msgs = cbm.DetermineBalanceMessages(fldName);

            foreach (var m in msgs)
            {
                switch (m.Chemical)
                {
                    case "CropN":
                        cvm.remNIcon = m.Icon;
                        cvm.remNIconText = _sd.GetNutrientIcon(m.Icon).Definition;
                        break;

                    case "CropP2O5":
                        cvm.remPIcon = m.Icon;
                        cvm.remPIconText = _sd.GetNutrientIcon(m.Icon).Definition;
                        break;

                    case "CropK2O":
                        cvm.remKIcon = m.Icon;
                        cvm.remKIconText = _sd.GetNutrientIcon(m.Icon).Definition;
                        break;

                    case "AgrN":
                        cvm.reqNIcon = m.Icon;
                        cvm.reqNIconText = _sd.GetNutrientIcon(m.Icon).Definition;
                        break;

                    case "AgrP2O5":
                        cvm.reqPIcon = m.Icon;
                        cvm.reqPIconText = _sd.GetNutrientIcon(m.Icon).Definition;
                        break;

                    case "AgrK2O":
                        cvm.reqKIcon = m.Icon;
                        cvm.reqKIconText = _sd.GetNutrientIcon(m.Icon).Definition;
                        break;
                }
            }

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
        public string reqNIcon { get; set; }
        public string reqNIconText { get; set; }
        public string reqP { get; set; }
        public string reqPIcon { get; set; }
        public string reqPIconText { get; set; }
        public string reqK { get; set; }
        public string reqKIcon { get; set; }
        public string reqKIconText { get; set; }
        public string remN { get; set; }
        public string remNIcon { get; set; }
        public string remNIconText { get; set; }
        public string remP { get; set; }
        public string remPIcon { get; set; }
        public string remPIconText { get; set; }
        public string remK { get; set; }
        public string remKIcon { get; set; }
        public string remKIconText { get; set; }
    }
}
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            decimal totReqN = 0;
            decimal totReqP = 0;
            decimal totReqK = 0;
            decimal totRemN = 0;
            decimal totRemP = 0;
            decimal totRemK = 0;

            CalcSummaryViewModel cvm = new CalcSummaryViewModel();
            cvm.summaryReqd = false;

            List<FieldCrop> crps = _ud.GetFieldCrops(fldName);
            foreach(var c in crps)
            {
                cvm.summaryReqd = true;
                totReqN -= c.reqN;
                totReqP -= c.reqP2o5;
                totReqK -= c.reqK2o;
                totRemN -= c.remN;
                totRemP -= c.remP2o5;
                totRemK -= c.remK2o;
            }

            List<NutrientManure> manures = _ud.GetFieldNutrientsManures(fldName);
            foreach(var m in manures)
            {
                cvm.summaryReqd = true;
                totReqN += m.yrN;
                totReqP += m.yrP2o5;
                totReqK += m.yrK2o;
                totRemN += m.ltN;
                totRemP += m.ltP2o5;
                totRemK += m.ltK2o;
            }

            cvm.reqN = totReqN.ToString();
            cvm.reqP = totReqP.ToString();
            cvm.reqK = totReqK.ToString();
            cvm.remN = totRemN.ToString();
            cvm.remP = totRemP.ToString();
            cvm.remK = totRemK.ToString();

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

using Agri.Models.Farm;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Models.Impl;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SERVERAPI.ViewComponents
{
    public class ReportFields : ViewComponent
    {
        private readonly UserData _ud;

        public ReportFields(UserData ud)
        {
            _ud = ud;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await ReportFieldsAsync());
        }

        private Task<ReportFieldsViewModel> ReportFieldsAsync()
        {
            ReportFieldsViewModel fvm = new ReportFieldsViewModel();

            List<Field> flds = _ud.GetFields();

            foreach (var m in flds)
            {
                DisplayReportField dc = new DisplayReportField();
                dc.fldName = m.FieldName;
                if (m.SoilTest != null)
                {
                    dc.sampleDate = m.SoilTest.sampleDate.ToString("MMM-yyyy");
                    dc.dispNO3H = m.SoilTest.valNO3H.ToString();
                    dc.dispP = m.SoilTest.ValP.ToString();
                    dc.dispK = m.SoilTest.valK.ToString();
                    dc.dispPH = m.SoilTest.valPH.ToString();
                }
                fvm.fields.Add(dc);
            }

            return Task.FromResult(fvm);
        }
    }

    public class ReportFieldsViewModel
    {
        public List<DisplayReportField> fields { get; set; }
    }

    public class DisplayReportField
    {
        public string fldName { get; set; }
        public string sampleDate { get; set; }
        public string dispNO3H { get; set; }
        public string dispP { get; set; }
        public string dispK { get; set; }
        public string dispPH { get; set; }
    }
}
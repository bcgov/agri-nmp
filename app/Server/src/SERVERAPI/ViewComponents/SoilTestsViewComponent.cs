using Agri.CalculateService;
using Agri.Data;
using Agri.Models.Farm;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Models.Impl;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SERVERAPI.ViewComponents
{
    public class SoilTests : ViewComponent
    {
        private readonly IAgriConfigurationRepository _sd;
        private readonly UserData _ud;
        private readonly ISoilTestConverter _soilTestConverter;

        public SoilTests(IAgriConfigurationRepository sd, Models.Impl.UserData ud, ISoilTestConverter soilTestConverter)
        {
            _sd = sd;
            _ud = ud;
            _soilTestConverter = soilTestConverter;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await GetSoilTestAsync());
        }

        private Task<SoilTestsViewModel> GetSoilTestAsync()
        {
            SoilTestsViewModel svm = new SoilTestsViewModel();

            svm.missingTests = false;

            FarmDetails fd = _ud.FarmDetails();
            svm.testingMethod = fd.TestingMethod;

            svm.tests = new List<DisplaySoilTest>();

            List<Field> flds = _ud.GetFields();

            foreach (var m in flds)
            {
                DisplaySoilTest dc = new DisplaySoilTest();
                dc.fldName = m.fieldName;
                if (m.soilTest != null)
                {
                    dc.sampleDate = m.soilTest.sampleDate.ToString("MMM-yyyy");
                    dc.dispNO3H = m.soilTest.valNO3H.ToString("G29");
                    dc.dispP = m.soilTest.ValP.ToString("G29");
                    dc.dispK = m.soilTest.valK.ToString("G29");
                    dc.dispPH = m.soilTest.valPH.ToString("G29");
                    dc.dispPRating = _sd.GetPhosphorusSoilTestRating(_soilTestConverter.GetConvertedSTP(_ud.FarmDetails()?.TestingMethod, m.soilTest));
                    dc.dispKRating = _sd.GetPotassiumSoilTestRating(_soilTestConverter.GetConvertedSTK(_ud.FarmDetails()?.TestingMethod, m.soilTest));
                }
                else
                {
                    svm.missingTests = true;
                }
                svm.tests.Add(dc);
            }

            return Task.FromResult(svm);
        }
    }

    public class SoilTestsViewModel
    {
        public string testingMethod { get; set; }
        public bool missingTests { get; set; }
        public List<DisplaySoilTest> tests { get; set; }
    }

    public class DisplaySoilTest
    {
        public string fldName { get; set; }
        public string dispSTP { get; set; }
        public string dispSTK { get; set; }
        public string sampleDate { get; set; }
        public string dispNO3H { get; set; }
        public string dispP { get; set; }
        public string dispPRating { get; set; }
        public string dispK { get; set; }
        public string dispKRating { get; set; }
        public string dispPH { get; set; }
    }
}
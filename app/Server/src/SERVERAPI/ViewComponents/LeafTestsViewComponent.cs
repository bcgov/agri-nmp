using Agri.CalculateService;
using Agri.Data;
using Agri.Models.Farm;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Models.Impl;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SERVERAPI.ViewComponents
{
    public class LeafTests : ViewComponent
    {
        private readonly IAgriConfigurationRepository _sd;
        private readonly UserData _ud;

        public LeafTests(IAgriConfigurationRepository sd, Models.Impl.UserData ud)
        {
            _sd = sd;
            _ud = ud;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await GetLeafTestAsync());
        }

        private Task<LeafTestsViewModel> GetLeafTestAsync()
        {
            LeafTestsViewModel svm = new LeafTestsViewModel();

            svm.missingLeafTests = false;

            FarmDetails fd = _ud.FarmDetails();
            svm.testingMethod = fd.TestingMethod;

            svm.tests = new List<DisplayLeafTest>();

            List<Field> flds = _ud.GetFields();

     
            foreach (var m in flds)
            {
                DisplayLeafTest dc = new DisplayLeafTest();
                dc.fldName = m.FieldName;
                if (m.LeafTest != null)
                {
                    //dc.sampleDate = m.SoilTest.sampleDate.ToString("MMM-yyyy");
                    dc.dispLeafTissueP = m.LeafTest.leafTissueP.ToString("#.##");
                    dc.dispLeafTissueK = m.LeafTest.leafTissueK.ToString("#.##"); 

                    foreach (FieldCrop crop in m.Crops)
                    {
                        dc.dispCropRequirementN = crop.reqN.ToString("#");
                        dc.dispCropRequirementP2O5 = crop.reqP2o5.ToString("#");
                        dc.dispCropRequirementK2O5 = crop.reqK2o.ToString("#");
                        dc.dispCropRemovalP2O5 = crop.remP2o5.ToString("#"); 
                        dc.dispCropRemovalK2O5 = crop.remK2o.ToString("#");
                    }
                }
                else
                {
                    svm.missingLeafTests = true;
                }
                svm.tests.Add(dc);
            }

            return Task.FromResult(svm);
        }
    }

    public class LeafTestsViewModel
    {
        public string testingMethod { get; set; }
        public bool missingLeafTests { get; set; }
        public List<DisplayLeafTest> tests { get; set; }
    }

    public class DisplayLeafTest
    {
        public string fldName { get; set; }
        public string sampleDate { get; set; }

        public string dispLeafTissueP { get; set; }
        public string dispLeafTissueK { get; set; }
        public string dispCropRequirementN { get; set; }
        public string dispCropRequirementP2O5 { get; set; }
        public string dispCropRequirementK2O5 { get; set; }
        public string dispCropRemovalP2O5 { get; set; }
        public string dispCropRemovalK2O5 { get; set; }
    }
}
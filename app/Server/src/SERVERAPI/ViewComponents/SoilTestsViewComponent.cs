﻿using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Controllers;
using SERVERAPI.Models;
using SERVERAPI.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERVERAPI.ViewComponents
{
    public class SoilTests : ViewComponent
    {
        private Models.Impl.StaticData _sd;
        private Models.Impl.UserData _ud;

        public SoilTests(Models.Impl.StaticData sd, Models.Impl.UserData ud)
        {
            _sd = sd;
            _ud = ud;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await GetSoilTestAsync());
        }

        private Task<SoilTestsViewModel> GetSoilTestAsync()
        {
            SoilTestsViewModel svm = new SoilTestsViewModel();
            Utility.SoilTestConversions stc = new SoilTestConversions(_ud, _sd);

            svm.missingTests = false;

            FarmDetails fd = _ud.FarmDetails();
            svm.testingMethod = fd.testingMethod;

            svm.tests = new List<DisplaySoilTest>();

            List<Field> flds = _ud.GetFields();

            foreach (var m in flds)
            {
                DisplaySoilTest dc = new DisplaySoilTest();
                dc.fldName = m.fieldName;
                if (m.soilTest != null)
                {
                    dc.sampleDate = m.soilTest.sampleDate.ToString("MMM-yyyy");
                    dc.dispNO3H = m.soilTest.valNO3H.ToString();
                    dc.dispP = m.soilTest.ValP.ToString();
                    dc.dispK = m.soilTest.valK.ToString();
                    dc.dispPH = m.soilTest.valPH.ToString();
                    dc.dispPRating = _sd.SoilTestRating("phosphorous", stc.GetConvertedSTP(m.soilTest));
                    dc.dispKRating = _sd.SoilTestRating("potassium", stc.GetConvertedSTK(m.soilTest));

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

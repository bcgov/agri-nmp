using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Agri.Interfaces;
using Agri.Models.Farm;
using Agri.Models.Configuration;
using SERVERAPI.Models.Impl;

namespace SERVERAPI.ViewComponents
{
    public class CalcCrops : ViewComponent
    {
        private readonly IAgriConfigurationRepository _sd;
        private readonly UserData _ud;

        public CalcCrops(IAgriConfigurationRepository sd, UserData ud)
        {
            _sd = sd;
            _ud = ud;
        }

        public async Task<IViewComponentResult> InvokeAsync(string fldName)
        {
            return View(await GetCropAsync(fldName));
        }

        private Task<CalcCropsViewModel> GetCropAsync(string fldName)
        {
            CalcCropsViewModel mvm = new CalcCropsViewModel();
            mvm.cropList = new List<DisplayCrop>();

            List<FieldCrop> fldCrops = _ud.GetFieldCrops(fldName);

            foreach (var m in fldCrops)
            {
                Crop cp = new Crop();
                var yld = new List<Yield>();

                if (!string.IsNullOrEmpty(m.cropOther))
                {
                    cp.CropName = m.cropOther + "*";
                    yld = _sd.GetYields();
                }
                else
                {
                    cp = _sd.GetCrop(Convert.ToInt32(m.cropId));
                    yld = _sd.GetYields();
                }

                if (m.coverCropHarvested.HasValue)
                {
                    cp.CropName = m.coverCropHarvested.Value ? cp.CropName + "(harvested)" : cp.CropName;
                }

                DisplayCrop dm = new DisplayCrop()
                {
                    fldNm = fldName,
                    cropId = Convert.ToInt32(m.id),
                    cropName = cp.CropName,
                    yield = m.yield.ToString() + " tons/ac (" + yld[0].YieldDesc + ")",
                    reqN = (m.reqN * -1).ToString("G29"),
                    reqP = (m.reqP2o5 * -1).ToString("G29"),
                    reqK = (m.reqK2o * -1).ToString("G29"),
                    remN = (m.remN * -1).ToString("G29"),
                    remP = (m.remP2o5 * -1).ToString("G29"),
                    remK = (m.remK2o * -1).ToString("G29"),
                };
                mvm.cropList.Add(dm);
            }

            return Task.FromResult(mvm);
        }
    }

    public class CalcCropsViewModel
    {
        public List<DisplayCrop> cropList { get; set; }
    }

    public class DisplayCrop
    {
        public string fldNm { get; set; }
        public int cropId { get; set; }
        public string cropName { get; set; }
        public string yield { get; set; }
        public string reqN { get; set; }
        public string reqP { get; set; }
        public string reqK { get; set; }
        public string remN { get; set; }
        public string remP { get; set; }
        public string remK { get; set; }
    }
}
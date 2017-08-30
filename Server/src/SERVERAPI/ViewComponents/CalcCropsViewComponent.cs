using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Controllers;
using SERVERAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SERVERAPI.Models.StaticData;

namespace SERVERAPI.ViewComponents
{
    public class CalcCrops : ViewComponent
    {
        private Models.Impl.StaticData _sd;
        private Models.Impl.UserData _ud;

        public CalcCrops(Models.Impl.StaticData sd, Models.Impl.UserData ud)
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
                Crop cp = _sd.GetCrop(Convert.ToInt32(m.cropId));
                Yield yld = _sd.GetYield(cp.yieldcd);

                DisplayCrop dm = new DisplayCrop()
                {

                    fldNm = fldName,
                    cropId = Convert.ToInt32(m.id),
                    cropName = cp.cropname,
                    yield = m.yield.ToString() + " tons/ac ("+ yld.yielddesc + ")",
                    reqN = m.reqN.ToString(),
                    reqP = m.reqP2o5.ToString(),
                    reqK = m.reqK2o.ToString(),
                    remN = m.remN.ToString(),
                    remP = m.remP2o5.ToString(),
                    remK = m.remK2o.ToString(),
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

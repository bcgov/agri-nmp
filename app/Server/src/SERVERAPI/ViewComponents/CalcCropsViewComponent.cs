using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Controllers;
using SERVERAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agri.Models.Farm;
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
                Crop cp = new Crop();
                Yield yld = new Yield();

                if (!string.IsNullOrEmpty(m.cropOther))
                {
                    cp.cropname = m.cropOther + "*";
                    yld = _sd.GetYield(1);
                }
                else
                {
                    cp = _sd.GetCrop(Convert.ToInt32(m.cropId));
                    yld = _sd.GetYield(cp.yieldcd);
                }

                if(m.coverCropHarvested.HasValue)
                {
                    cp.cropname = m.coverCropHarvested.Value ? cp.cropname + "(harvested)" : cp.cropname;
                }

                DisplayCrop dm = new DisplayCrop()
                {

                    fldNm = fldName,
                    cropId = Convert.ToInt32(m.id),
                    cropName = cp.cropname,
                    yield = m.yield.ToString() + " tons/ac ("+ yld.yielddesc + ")",
                    reqN = (m.reqN * -1).ToString(),
                    reqP = (m.reqP2o5 * -1).ToString(),
                    reqK = (m.reqK2o * -1).ToString(),
                    remN = (m.remN * -1).ToString(),
                    remP = (m.remP2o5 * -1).ToString(),
                    remK = (m.remK2o * -1).ToString()
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

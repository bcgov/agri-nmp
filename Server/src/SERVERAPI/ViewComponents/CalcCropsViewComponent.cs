using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Controllers;
using SERVERAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            mvm.crops = new List<DisplayCrop>();

            List<Crop> fldCrops = _ud.GetFieldCrops(fldName);

            var farmData = HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");


                foreach (var m in fldCrops)
                {
                    DisplayCrop dm = new DisplayCrop()
                    {
                        fldName = fldName,
                        cropId = Convert.ToInt32(m.cropId),
                        cropName = "xxx",
                        yield = m.yield.ToString(),
                        reqN = m.reqN.ToString(),
                        reqP = m.reqP2o5.ToString(),
                        reqK = m.reqK2o.ToString(),
                        remN = m.remN.ToString(),
                        remP = m.remP2o5.ToString(),
                        remK = m.remK2o.ToString(),
                    };
                    mvm.crops.Add(dm);
                }

            return Task.FromResult(mvm);
        }
    }
    public class CalcCropsViewModel
    {
        public List<DisplayCrop> crops { get; set; }
    }
    public class DisplayCrop
    {
        public string fldName { get; set; }
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

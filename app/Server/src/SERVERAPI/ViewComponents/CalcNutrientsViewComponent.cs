using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERVERAPI.ViewComponents
{
    public class CalcNutrients : ViewComponent
    {
        private Models.Impl.StaticData _sd;
        private Models.Impl.UserData _ud;

        public CalcNutrients(Models.Impl.StaticData sd, Models.Impl.UserData ud)
        {
            _sd = sd;
            _ud = ud;
        }
        public async Task<IViewComponentResult> InvokeAsync(string fldName)
        {
            return View(await GetNutrientAsync(fldName));
        }

        private Task<CalcNutrientsViewModel> GetNutrientAsync(string fldName)
        {
            CalcNutrientsViewModel mvm = new CalcNutrientsViewModel();
            mvm.cropList = new List<DisplayCrop>();

            //List<FieldCrop> fldCrops = _ud.GetFieldCrops(fldName);

            //foreach (var m in fldCrops)
            //{
            //    Crop cp = _sd.GetCrop(Convert.ToInt32(m.cropId));
            //    Yield yld = _sd.GetYield(cp.yieldcd);

            //    DisplayCrop dm = new DisplayCrop()
            //    {

            //        fldNm = fldName,
            //        cropId = Convert.ToInt32(m.id),
            //        cropName = cp.cropname,
            //        yield = m.yield.ToString() + " tons/ac (" + yld.yielddesc + ")",
            //        reqN = m.reqN.ToString(),
            //        reqP = m.reqP2o5.ToString(),
            //        reqK = m.reqK2o.ToString(),
            //        remN = m.remN.ToString(),
            //        remP = m.remP2o5.ToString(),
            //        remK = m.remK2o.ToString(),
            //    };
            //    mvm.cropList.Add(dm);
            //}

            mvm.cropList.Add(new DisplayCrop() { cropId = 0 });

            return Task.FromResult(mvm);
        }
    }
    public class CalcNutrientsViewModel
    {
        public List<DisplayCrop> cropList { get; set; }
    }

}

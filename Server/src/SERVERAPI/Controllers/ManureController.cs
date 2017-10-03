using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using SERVERAPI.ViewModels;
using SERVERAPI.Models.Impl;
using SERVERAPI.Models;

namespace SERVERAPI.Controllers
{
    public class ManureController : Controller
    {
        public IHostingEnvironment _env { get; set; }
        public UserData _ud { get; set; }
        public Models.Impl.StaticData _sd { get; set; }

        public ManureController(IHostingEnvironment env, UserData ud, Models.Impl.StaticData sd)
        {
            _env = env;
            _ud = ud;
            _sd = sd;
        }
        // GET: /<controller>/
        public IActionResult Manure()
        {           
            return View();
        }
        public IActionResult CompostDetails(int? id)
        {
            //Utility.CalculateNutrients calculateNutrients = new CalculateNutrients(_env, _ud, _sd);
            //NOrganicMineralizations nOrganicMineralizations = new NOrganicMineralizations();

            CompostDetailViewModel mvm = new CompostDetailViewModel();

            mvm.act = id == null ? "Add" : "Edit";

            if (id != null)
            {
                //NutrientFertilizer nf = _ud.GetFieldNutrientsFertilizer(fldName, id.Value);

                //mvm.selTypOption = nf.fertilizerTypeId.ToString();
                //FertilizerType ft = _sd.GetFertilizerType(nf.fertilizerTypeId.ToString());

                //mvm.currUnit = ft.dry_liquid;
                //mvm.selFertOption = ft.custom ? 1 : nf.fertilizerId;
                //mvm.applRate = nf.applRate.ToString("#.##");
                //mvm.selRateOption = nf.applUnitId.ToString();
                //mvm.applMethod = nf.applMethod;
                //mvm.fertilizerType = ft.dry_liquid;
                //if (nf.applDate.HasValue)
                //{
                //    mvm.applDate = nf.applDate.HasValue ? nf.applDate.Value.ToString("MMM-yyyy") : "";
                //}
                //if (ft.dry_liquid == "liquid")
                //{
                //    mvm.density = nf.liquidDensity.ToString("#.##");
                //    mvm.selDenOption = nf.liquidDensityUnitId;
                //    if (!ft.custom)
                //    {
                //        if (mvm.density != _sd.GetLiquidFertilizerDensity(nf.fertilizerId, nf.liquidDensityUnitId).value.ToString("#.##"))
                //        {
                //            mvm.stdDensity = false;
                //        }
                //        else
                //        {
                //            mvm.stdDensity = true;
                //        }
                //    }
                //}
                //if (ft.custom)
                //{
                //    mvm.valN = nf.fertN.Value.ToString();
                //    mvm.valP2o5 = nf.fertP2o5.Value.ToString();
                //    mvm.valK2o = nf.fertK2o.Value.ToString();
                //    mvm.manEntry = true;
                //}
                //else
                //{
                //    Fertilizer ff = _sd.GetFertilizer(nf.fertilizerId.ToString());
                //    mvm.valN = ff.nitrogen.ToString();
                //    mvm.valP2o5 = ff.phosphorous.ToString();
                //    mvm.valK2o = ff.potassium.ToString();
                //    mvm.manEntry = false;
                //}
            }
            else

            {
                mvm.bookValue = true;
                mvm.manureName = "  ";
                mvm.moisture = "  ";
                mvm.nitrogen = "  ";
                mvm.ammonia = "  ";
                mvm.phosphorous = "  ";
                mvm.potassium = "  ";
                mvm.nitrate = "  ";
            }

            CompostDetailsSetup(ref mvm);

            return PartialView(mvm);
        }
        private void CompostDetailsSetup(ref CompostDetailViewModel cvm)
        {
            cvm.manOptions = new List<Models.StaticData.SelectListItem>();
            cvm.manOptions = _sd.GetManuresDll().ToList();

            return;
        }
        [HttpPost]
        public IActionResult CompostDetails(CompostDetailViewModel cvm)
        {

            CompostDetailsSetup(ref cvm);

            try
            {

                if (cvm.buttonPressed == "ManureChange")
                {
                    ModelState.Clear();
                    cvm.buttonPressed = "";

                    if (cvm.selManOption != 0)
                    {
                        Models.StaticData.Manure man = _sd.GetManure(cvm.selManOption.ToString());
                        if(man.manure_class == "Other" ||
                           man.manure_class == "Compost")
                        {
                            cvm.bookValue = false;
                            cvm.onlyCustom = true;
                            cvm.nitrogen = string.Empty;
                            cvm.moisture = string.Empty;
                            cvm.ammonia = string.Empty;
                            cvm.nitrate = string.Empty;
                            cvm.phosphorous = string.Empty;
                            cvm.potassium = string.Empty;
                            cvm.compost = man.manure_class == "Compost" ? true : false;
                            cvm.manureName = cvm.compost ? "Custom - " + man.name + " - " : "Custom - " + man.solid_liquid + " - ";
                        }
                        else
                        {
                            cvm.bookValue = true;
                            cvm.nitrogen = man.nitrogen.ToString();
                            cvm.moisture = man.moisture.ToString();
                            cvm.ammonia = man.ammonia.ToString();
                            cvm.nitrate = string.Empty;
                            cvm.phosphorous = man.phosphorous.ToString();
                            cvm.potassium = man.potassium.ToString();
                            cvm.manureName = man.name;
                        }
                    }
                    else
                    {
                        cvm.bookValue = true;
                        cvm.nitrogen = string.Empty;
                        cvm.moisture = string.Empty;
                        cvm.ammonia = string.Empty;
                        cvm.nitrate = string.Empty;
                        cvm.phosphorous = string.Empty;
                        cvm.potassium = string.Empty;
                        cvm.manureName = string.Empty;
                    }
                    return View(cvm);
                }
                if (cvm.buttonPressed == "TypeChange")
                {
                    ModelState.Clear();
                    cvm.buttonPressed = "";

                    if (cvm.selManOption != 0)
                    {
                        Models.StaticData.Manure man = _sd.GetManure(cvm.selManOption.ToString());
                        cvm.onlyCustom = false;
                        cvm.moisture = cvm.bookValue ? man.moisture.ToString() : "";
                        cvm.nitrogen = man.nitrogen.ToString();
                        cvm.ammonia = man.ammonia.ToString();
                        cvm.nitrate = string.Empty;
                        cvm.phosphorous = man.phosphorous.ToString();
                        cvm.potassium = man.potassium.ToString();
                        cvm.manureName = cvm.bookValue ? man.name : "Custom - " + man.name;
                    }
                    else
                    {
                    }
                    return View(cvm);
                }

                if (ModelState.IsValid)
                {
                    if (cvm.id == null)
                    {
                        FarmManure fm = new FarmManure();
                        if (cvm.bookValue)
                        {
                            fm.manureId = cvm.selManOption;
                        }
                        else
                        {
                            Models.StaticData.Manure man = _sd.GetManure(cvm.selManOption.ToString());

                            fm.manureId = (int?)null;
                            fm.ammonia = Convert.ToInt32(cvm.ammonia);
                            fm.dmid = man.dmid;
                            fm.manure_class = man.manure_class;
                            fm.moisture = cvm.moisture;
                            fm.name = cvm.manureName;
                            fm.nitrogen = Convert.ToDecimal(cvm.nitrogen);
                            fm.nminerizationid = man.nminerizationid;
                            fm.phosphorous = Convert.ToDecimal(cvm.phosphorous);
                            fm.potassium = Convert.ToDecimal(cvm.potassium);
                            fm.solid_liquid = man.solid_liquid;
                        }


                        _ud.AddFarmManure(fm);
                    }
                    else
                    {
                        //NutrientManure nm = _ud.GetFieldNutrientsManure(cvm.fieldName, cvm.id.Value);
                        //nm.manureId = cvm.selManOption;
                        //nm.applicationId = cvm.selApplOption;
                        //nm.unitId = cvm.selRateOption;
                        //nm.rate = Convert.ToDecimal(cvm.rate);
                        //nm.nh4Retention = Convert.ToDecimal(cvm.nh4);
                        //nm.nAvail = Convert.ToDecimal(cvm.avail);
                        //nm.yrN = Convert.ToDecimal(cvm.yrN);
                        //nm.yrP2o5 = Convert.ToDecimal(cvm.yrP2o5);
                        //nm.yrK2o = Convert.ToDecimal(cvm.yrK2o);
                        //nm.ltN = Convert.ToDecimal(cvm.ltN);
                        //nm.ltP2o5 = Convert.ToDecimal(cvm.ltP2o5);
                        //nm.ltK2o = Convert.ToDecimal(cvm.ltK2o);

                        //_ud.UpdateFieldNutrientsManure(cvm.fieldName, nm);
                    }

                    string url = Url.Action("RefreshCompostList", "Manure");
                    return Json(new { success = true, url = url, target = "#compost" });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unexpected system error.");
            }

            return PartialView(cvm);
        }
        public IActionResult RefreshCompostList()
        {
            return ViewComponent("Compost");
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Models;
using SERVERAPI.Models.Impl;
using SERVERAPI.Utility;
using SERVERAPI.ViewModels;
using static SERVERAPI.Models.StaticData;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SERVERAPI.Controllers
{
    public class ManureManagementController : Controller
    {
        public IHostingEnvironment _env { get; set; }
        public UserData _ud { get; set; }
        public Models.Impl.StaticData _sd { get; set; }
        public IViewRenderService _viewRenderService { get; set; }
        public AppSettings _settings;

        public ManureManagementController(IHostingEnvironment env, IViewRenderService viewRenderService, UserData ud,
            Models.Impl.StaticData sd)
        {
            _env = env;
            _ud = ud;
            _sd = sd;
            _viewRenderService = viewRenderService;
        }

        [HttpGet]
        public IActionResult ManureGeneratedObtained()
        {
            return View();
        }

        public IActionResult ManureGeneratedObtainedDetail(int? id)
        {
            ManureGeneratedObtainedDetailViewModel mgovm = new ManureGeneratedObtainedDetailViewModel();
            mgovm.title = id == null ? "Add" : "Edit";
            mgovm.stdWashWater = true;
            mgovm.stdMilkProduction = true;
            mgovm.placehldr = _sd.GetUserPrompt("averageanimalnumberplaceholder");

            if (id != null)
            {
            }
            else
            {
                animalDetailReset(ref mgovm);
                animalTypeDetailsSetup(ref mgovm);

            }

            return PartialView("ManureGeneratedObtainedDetail", mgovm);
        }

        [HttpPost]
        public IActionResult ManureGeneratedObtainedDetail(ManureGeneratedObtainedDetailViewModel mgovm)
        {
            mgovm.placehldr = _sd.GetUserPrompt("averageanimalnumberplaceholder");
            animalTypeDetailsSetup(ref mgovm);
            try
            {
                if (mgovm.buttonPressed == "TypeChange")
                {
                    ModelState.Clear();
                    mgovm.buttonPressed = "";
                    mgovm.btnText = "Save";

                    if (mgovm.selAnimalTypeOption != "" &&
                        mgovm.selAnimalTypeOption != "0" &&
                        mgovm.selAnimalTypeOption != "select animal")
                    {
                        if (mgovm.showWashWater)
                        {
                            mgovm.washWater = _sd.GetIncludeWashWater(Convert.ToInt16(mgovm.selSubTypeOption));
                            mgovm.stdWashWater = true;
                        }

                        if (mgovm.showMilkProduction)
                        {
                            mgovm.milkProduction = _sd.GetMilkProduction(Convert.ToInt16(mgovm.selSubTypeOption));
                            mgovm.stdMilkProduction = true;
                        }
                    }
                    else
                    {
                        animalDetailReset(ref mgovm);
                    }

                    return View(mgovm);

                }

                if (mgovm.buttonPressed == "ManureMaterialTypeChange")
                {
                    ModelState.Clear();
                    mgovm.buttonPressed = "";
                    mgovm.btnText = "Save";

                    return View(mgovm);
                }

                if (mgovm.buttonPressed == "AnimalTypeChange")
                {
                    ModelState.Clear();
                    mgovm.buttonPressed = "";
                    mgovm.btnText = "Save";

                    if (mgovm.selAnimalTypeOption != "" &&
                        mgovm.selAnimalTypeOption != "0" &&
                        mgovm.selAnimalTypeOption != "select animal")
                    {
                        mgovm.selSubTypeOption = "select subtype";
                        mgovm.averageAnimalNumber = "";
                        mgovm.selManureMaterialTypeOption = "select type";
                        if (!string.IsNullOrEmpty(mgovm.selSubTypeOption) &&
                            mgovm.selSubTypeOption != "select subtype")
                        {
                            if (_sd.DoesAnimalUseWashWater(Convert.ToInt32(mgovm.selSubTypeOption)))
                            {
                                mgovm.showWashWater = true;
                                mgovm.showMilkProduction = true;
                            }
                        }
                        else
                        {
                            mgovm.showWashWater = false;
                            mgovm.showMilkProduction = false;
                        }
                    }
                    else
                    {
                        animalDetailReset(ref mgovm);
                    }

                    return View(mgovm);
                }

                if (mgovm.buttonPressed == "ResetWashWater")
                {
                    ModelState.Clear();
                    mgovm.buttonPressed = "";
                    mgovm.btnText = "Save";

                    mgovm.stdWashWater = true;
                    mgovm.washWater = _sd.GetIncludeWashWater(Convert.ToInt32(mgovm.selSubTypeOption));
                    if (mgovm.milkProduction != _sd.GetMilkProduction(Convert.ToInt32(mgovm.selSubTypeOption.ToString())))
                    {
                        mgovm.stdMilkProduction = false;
                    }
                    return View(mgovm);
                }

                if (mgovm.buttonPressed == "ResetMilkProduction")
                {
                    ModelState.Clear();
                    mgovm.buttonPressed = "";
                    mgovm.btnText = "Save";

                    mgovm.stdMilkProduction = true;
                    mgovm.milkProduction = _sd.GetMilkProduction(Convert.ToInt32(mgovm.selSubTypeOption));
                    if (mgovm.washWater != _sd.GetIncludeWashWater(Convert.ToInt32(mgovm.selSubTypeOption.ToString())))
                    {
                        mgovm.stdWashWater = false;
                    }
                    return View(mgovm);
                }

                if (mgovm.btnText == "Save")
                {
                    ModelState.Clear();
                    if (!string.IsNullOrEmpty(mgovm.selSubTypeOption) &&
                        mgovm.selSubTypeOption != "select subtype")
                    {
                        if (mgovm.washWater !=
                            _sd.GetIncludeWashWater(Convert.ToInt32(mgovm.selSubTypeOption.ToString())))
                        {
                            mgovm.stdWashWater = false;
                        }

                        if (mgovm.milkProduction !=
                            _sd.GetMilkProduction(Convert.ToInt32(mgovm.selSubTypeOption.ToString())))
                        {
                            mgovm.stdMilkProduction = false;
                        }
                    }

                    if (string.IsNullOrEmpty(mgovm.selAnimalTypeOption) ||
                        mgovm.selAnimalTypeOption == "select animal")
                    {
                        ModelState.AddModelError("selAnimalTypeOption", "Required");
                    }
                    if (string.IsNullOrEmpty(mgovm.selSubTypeOption) ||
                        mgovm.selSubTypeOption == "select subtype" || mgovm.selSubTypeOption == "0")
                    {
                        ModelState.AddModelError("selSubTypeOption", "Required");
                    }
                    if (string.IsNullOrEmpty(mgovm.selManureMaterialTypeOption) ||
                        mgovm.selManureMaterialTypeOption == "select type")
                    {
                        ModelState.AddModelError("selManureMaterialTypeOption", "Required");
                    }
                    if (string.IsNullOrEmpty(mgovm.averageAnimalNumber))
                    {
                        ModelState.AddModelError("averageAnimalNumber", "Required");
                    }
                    //return View(mgovm);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unexpected system error -" + ex.Message);
            }

            return PartialView(mgovm);
        }
        private void animalDetailReset(ref ManureGeneratedObtainedDetailViewModel mgovm)
        {
            mgovm.stdWashWater = true;
            mgovm.stdMilkProduction = true;

            return;
        }

        private void animalTypeDetailsSetup(ref ManureGeneratedObtainedDetailViewModel mgovm)
        {
            mgovm.showWashWater = false;
            mgovm.showMilkProduction = false;

            mgovm.animalTypeOptions = new List<Models.StaticData.SelectListItem>();
            mgovm.animalTypeOptions = _sd.GetAnimalTypesDll().ToList();

            mgovm.subTypeOptions = new List<Models.StaticData.SelectListItem>();
            mgovm.manureMaterialTypeOptions = new List<Models.StaticData.SelectListItem>();
            mgovm.manureMaterialTypeOptions = _sd.GetManureMaterialTypesDll().ToList();

            if (!string.IsNullOrEmpty(mgovm.selAnimalTypeOption) &&
                mgovm.selAnimalTypeOption != "select animal")
            {
                mgovm.subTypeOptions = _sd.GetSubtypesDll(Convert.ToInt32(mgovm.selAnimalTypeOption)).ToList();
                mgovm.subTypeOptions.Insert(0, new SelectListItem() { Id = 0, Value = "select subtype" });

                if (!string.IsNullOrEmpty(mgovm.selSubTypeOption) &&
                    mgovm.selSubTypeOption != "select subtype")
                {
                    if (_sd.DoesAnimalUseWashWater(Convert.ToInt32(mgovm.selSubTypeOption)))
                    {
                        mgovm.showWashWater = true;
                        mgovm.showMilkProduction = true;
                    }
                }
            }
            return;
        }
    }
}

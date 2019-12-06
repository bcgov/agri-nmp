using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agri.Interfaces;
using Agri.Models;
using Agri.Models.Configuration;
using Agri.Models.Farm;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Filters;
using SERVERAPI.Models.Impl;
using SERVERAPI.ViewModels;

namespace SERVERAPI.Controllers
{
    [SessionTimeout]
    public class AnimalsController : BaseController
    {
        private readonly UserData _ud;
        private readonly IAgriConfigurationRepository _sd;

        public AnimalsController(UserData ud, IAgriConfigurationRepository sd)
        {
            _ud = ud;
            _sd = sd;
        }

        [HttpGet]
        public IActionResult AddAnimals()
        {
            AddAnimalsViewModel aavm = new AddAnimalsViewModel();
            // aavm.animalDetails = new List<AnimalDetails>();

            animalTypeDetailsSetup(ref aavm);

            //if (aavm.isManureCollected)
            //{
            //    aavm.showDurationDays = true;
            //}
            //else
            //{
            //    aavm.showDurationDays = false;
            //}

            //if (aavm.buttonPressed == "isManureCollectedChanged")
            //{
            //    if (aavm.isManureCollected)
            //    {
            //        aavm.showDurationDays = true;
            //        //if (aavm.selRegOption.HasValue)
            //        //{
            //        //    fvm.buttonPressed = "RegionChange";
            //        //    fvm = SetSubRegions(fvm);
            //        //}
            //    }
            //    else
            //    {
            //        //fvm.showAnimals = false;
            //    }
            //}

            return View(aavm);

        }
        [HttpPost]
        public IActionResult AddAnimals(AddAnimalsViewModel aavm)
        {
            try
            {
                if (!string.IsNullOrEmpty(aavm.buttonPressed))
                {
                    animalTypeDetailsSetup(ref aavm);
                    aavm.showDurationDays = false;

                    if (aavm.buttonPressed == "isManureCollectedChange")
                    {
                        if (aavm.isManureCollected)
                        {
                            aavm.showDurationDays = true;
                        }
                    }
                    else
                    {

                        //aavm.isLoadAnimalDetails = true;
                        //var animalDetail = new AnimalDetails();
                        //animalDetail.AnimalType = aavm.selAnimalTypeOption;
                        //animalDetail.SubType = aavm.selSubTypeOption;
                        //if (aavm.animalDetails == null)
                        //{
                        //    aavm.animalDetails = new List<AnimalDetails>();
                        //    aavm.animalDetails.Add(animalDetail);
                        //}
                        //else
                        //{
                        //    aavm.animalDetails.Add(animalDetail);
                        //}
                        animalTypeDetailsSetup(ref aavm);
                        // return PartialView("", "");
                    }

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unexpected system error -" + ex.Message);
            }
            return View(aavm);
        }

        public IActionResult AddNewAnimal(AddAnimalsViewModel aavm)
        {
            //var animalDetails = aavm.animalDetails;
            //aavm = new AddAnimalsViewModel();
            //aavm.animalDetails = animalDetails;
            return View(aavm);
        }

        private void animalTypeDetailsSetup(ref AddAnimalsViewModel aavm)
        {
            var farmData = _ud.FarmDetails();
            if (farmData.HasBeefCows)
            {
                aavm.animalTypeOptions = new List<SelectListItem>();
                aavm.animalTypeOptions = _sd.GetAnimalTypesDll().ToList();
                aavm.selAnimalTypeOption = "1";
                aavm.selManureMaterialTypeOption = ManureMaterialType.Solid;
            }

            aavm.subTypeOptions = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(aavm.selAnimalTypeOption) &&
                aavm.selAnimalTypeOption != "select animal")
            {
                aavm.subTypeOptions = _sd.GetSubtypesDll(Convert.ToInt32(aavm.selAnimalTypeOption)).ToList();
                if (aavm.subTypeOptions.Count() > 1)
                {
                    aavm.subTypeOptions.Insert(0, new SelectListItem() { Id = 0, Value = "select subtype" });
                }

                if (aavm.subTypeOptions.Count() == 1)
                {
                    aavm.selSubTypeOption = aavm.subTypeOptions[0].Id.ToString();

                }
            }
            return;
        }
    }
}
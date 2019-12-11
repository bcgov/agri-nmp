using System;
using System.Collections.Generic;
using System.Linq;
using Agri.Data;
using Agri.Models;
using Agri.Models.Configuration;
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

        public ActionResult AddAnimals()
        {
            FieldPageViewModel fvm = new FieldPageViewModel();
            return View(fvm);
        }

        [HttpGet]
        public IActionResult AnimalDetail(int id, string target, string actn, string cntl)
        {
            AddAnimalsViewModel aavm = new AddAnimalsViewModel();
            aavm.actn = actn;
            aavm.cntl = cntl;
            Agri.Models.Farm.Animal an = _ud.GetAnimalDetail(id);
            if (an != null)
            {
                aavm.act = "Edit";
                aavm.selSubTypeOption = an.selSubTypeOption;
                aavm.averageAnimalNumber = an.averageAnimalNumber;
                aavm.isManureCollected = an.isManureCollected;
                aavm.durationDays = an.durationDays;
                aavm.Id = an.Id;
            }
            else
            {
                aavm.act = "Add";
            }
            aavm.target = target;
            animalTypeDetailsSetup(ref aavm);
            return View(aavm);
        }

        [HttpPost]
        public IActionResult AnimalDetail(AddAnimalsViewModel aavm)
        {
            string url;
            animalTypeDetailsSetup(ref aavm);
            if (ModelState.IsValid)
            {
                Agri.Models.Farm.Animal anml = _ud.GetAnimalDetail(aavm.Id);

                if (aavm.act == "Add")
                {
                    anml = new Agri.Models.Farm.Animal();
                }
                else
                {
                    anml = _ud.GetAnimalDetail(aavm.Id);
                    if (anml == null)
                    {
                        anml = new Agri.Models.Farm.Animal();
                    }
                }

                Agri.Models.Configuration.Animal animal = _sd.GetAnimal(Convert.ToInt32(aavm.selAnimalTypeOption));
                anml.animalTypeOptions = aavm.animalTypeOptions;
                anml.selAnimalTypeOption = aavm.selAnimalTypeOption;
                AnimalSubType animalSubTypeDetails = _sd.GetAnimalSubType(Convert.ToInt32(aavm.selSubTypeOption));
                anml.selSubTypeOption = aavm.selSubTypeOption;
                anml.subTypeOptions = aavm.subTypeOptions;
                anml.subTypeName = animalSubTypeDetails.Name;
                anml.averageAnimalNumber = aavm.averageAnimalNumber;
                anml.isManureCollected = aavm.isManureCollected;
                anml.durationDays = anml.isManureCollected ? aavm.durationDays : 0;

                if (aavm.act == "Add")
                {
                    _ud.AddAnimal(anml);
                }
                else
                {
                    _ud.UpdateAnimal(anml);
                }
                if (aavm.target == "#animals")
                {
                    url = Url.Action("RefreshAnimalList", "Animals");
                }
                else
                {
                    url = Url.Action(aavm.actn, aavm.cntl, new { id = anml.Id });
                }
                return Json(new { success = true, url = url, target = aavm.target });
            }
            aavm.buttonPressed = null;
            return PartialView("AnimalDetail", aavm);
        }

        [HttpGet]
        public ActionResult AnimalDelete(int id, string target)
        {
            AnimalDeleteViewModel fvm = new AnimalDeleteViewModel();
            fvm.target = target;

            Agri.Models.Farm.Animal anml = _ud.GetAnimalDetail(id);

            fvm.id = anml.Id;
            fvm.act = "Delete";

            return PartialView("AnimalDelete", fvm);
        }

        [HttpPost]
        public ActionResult AnimalDelete(AnimalDeleteViewModel fvm)
        {
            if (ModelState.IsValid)
            {
                _ud.DeleteAnimal(fvm.id);

                string url = Url.Action("RefreshAnimalList", "Animals");
                return Json(new { success = true, url = url, target = fvm.target });
            }
            return PartialView("FieldDelete", fvm);
        }

        public IActionResult RefreshAnimalList()
        {
            return ViewComponent("Animals");
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
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
            aavm.Actn = actn;
            aavm.Cntl = cntl;
            Agri.Models.Farm.FarmAnimal an = _ud.GetAnimalDetail(id);
            if (an != null)
            {
                aavm.Act = "Edit";
                aavm.SelectSubTypeOption = an.SelectSubTypeOption;
                aavm.AverageAnimalNumber = an.AverageAnimalNumber;
                aavm.IsManureCollected = an.ManureCollected == "Yes" ? true : false;
                aavm.DurationDays = an.DurationDays;
                aavm.Id = an.Id;
            }
            else
            {
                aavm.Act = "Add";
            }
            aavm.Target = target;
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
                Agri.Models.Farm.FarmAnimal anml = _ud.GetAnimalDetail(aavm.Id);

                if (aavm.Act == "Add")
                {
                    anml = new Agri.Models.Farm.FarmAnimal();
                }
                else
                {
                    anml = _ud.GetAnimalDetail(aavm.Id);
                    if (anml == null)
                    {
                        anml = new Agri.Models.Farm.FarmAnimal();
                    }
                }

                Agri.Models.Configuration.Animal animal = _sd.GetAnimal(Convert.ToInt32(aavm.SelectAnimalTypeOption));
                anml.AnimalTypeName = animal.Name;
                anml.AnimalId = animal.Id;
                AnimalSubType animalSubTypeDetails = _sd.GetAnimalSubType(Convert.ToInt32(aavm.SelectSubTypeOption));
                anml.SelectSubTypeOption = aavm.SelectSubTypeOption;
                anml.SubTypeName = animalSubTypeDetails.Name.Trim();
                anml.SubTypeId = animalSubTypeDetails.Id;
                anml.AverageAnimalNumber = aavm.AverageAnimalNumber;
                anml.IsManureCollected = aavm.IsManureCollected;
                anml.ManureCollected = aavm.IsManureCollected ? "Yes" : "No";
                anml.DurationDays = aavm.IsManureCollected ? aavm.DurationDays : 0;
                anml.ManureMaterialType = ManureMaterialType.Solid;

                if (aavm.Act == "Add")
                {
                    _ud.AddAnimal(anml);
                }
                else
                {
                    _ud.UpdateAnimal(anml);
                }
                if (aavm.Target == "#animals")
                {
                    url = Url.Action("RefreshAnimalList", "Animals");
                }
                else
                {
                    url = Url.Action(aavm.Actn, aavm.Cntl, new { id = anml.Id });
                }
                return Json(new { success = true, url = url, target = aavm.Target });
            }
            aavm.ButtonPressed = null;
            return PartialView("AnimalDetail", aavm);
        }

        [HttpGet]
        public ActionResult AnimalDelete(int id, string target)
        {
            AnimalDeleteViewModel advm = new AnimalDeleteViewModel();
            advm.Target = target;

            Agri.Models.Farm.FarmAnimal anml = _ud.GetAnimalDetail(id);

            advm.Id = anml.Id;
            advm.SubTypeName = anml.SubTypeName;
            advm.Act = "Delete";

            return PartialView("AnimalDelete", advm);
        }

        [HttpPost]
        public ActionResult AnimalDelete(AnimalDeleteViewModel advm)
        {
            if (ModelState.IsValid)
            {
                _ud.DeleteAnimal(advm.Id);

                string url = Url.Action("RefreshAnimalList", "Animals");
                return Json(new { success = true, url = url, target = advm.Target });
            }
            return PartialView("AnimalDelete", advm);
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
                aavm.AnimalTypeOptions = new List<SelectListItem>();
                aavm.AnimalTypeOptions = _sd.GetAnimalTypesDll().ToList();
                aavm.SelectAnimalTypeOption = "1";
                aavm.SelectManureMaterialTypeOption = ManureMaterialType.Solid;
            }

            aavm.SubTypeOptions = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(aavm.SelectAnimalTypeOption) &&
                aavm.SelectAnimalTypeOption != "select animal")
            {
                aavm.SubTypeOptions = _sd.GetSubtypesDll(Convert.ToInt32(aavm.SelectAnimalTypeOption)).ToList();
                if (aavm.SubTypeOptions.Count() > 1)
                {
                    aavm.SubTypeOptions.Insert(0, new SelectListItem() { Id = 0, Value = "select type" });
                }

                if (aavm.SubTypeOptions.Count() == 1)
                {
                    aavm.SelectSubTypeOption = aavm.SubTypeOptions[0].Id.ToString();
                }
            }
            return;
        }
    }
}
using Agri.Data;
using Agri.Models;
using Agri.Models.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Models.Impl;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SERVERAPI.ViewComponents
{
    public class Animals : ViewComponent
    {
        private IHostingEnvironment _env;
        private UserData _ud;
        private IAgriConfigurationRepository _sd;

        public Animals(IHostingEnvironment env, UserData ud, IAgriConfigurationRepository sd)
        {
            _env = env;
            _ud = ud;
            _sd = sd;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await GetAnimalsAsync());
        }

        private Task<AnimalsViewModel> GetAnimalsAsync()
        {
            AnimalsViewModel avm = new AnimalsViewModel();

            avm.animals = new List<Agri.Models.Farm.Animal>();
            List<Agri.Models.Farm.Animal> animalList = _ud.GetAnimals();

            foreach (var a in animalList)
            {
                avm.animals.Add(a);
            }
            if (avm.animals.Count == 0)
            {
                avm.actn = null;
                avm.cntl = "Animals";
                avm.act = "Add";
                avm.target = "#animals";
                animalTypeDetailsSetup(ref avm);
            }

            return Task.FromResult(avm);
        }

        private void animalTypeDetailsSetup(ref AnimalsViewModel aavm)
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

    public class AnimalsViewModel
    {
        public int Id { get; set; }
        public string act { get; set; }
        public string cntl { get; set; }
        public string target { get; set; }
        public string actn { get; set; }
        public string title { get; set; }
        public string btnText { get; set; }
        public bool isMainLoad { get; set; }

        //[Required(ErrorMessage = "Required")]
        //[Range(1, 9999, ErrorMessage = "Required")]
        public string selAnimalTypeOption { get; set; }

        public List<SelectListItem> animalTypeOptions { get; set; }

        [Required(ErrorMessage = "Req" +
            "uired")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public string selSubTypeOption { get; set; }

        public string subTypeName { get; set; }
        public List<SelectListItem> subTypeOptions { get; set; }

        //[Required(ErrorMessage = "Required")]
        //[Range(1, 9999, ErrorMessage = "Required")]
        public ManureMaterialType selManureMaterialTypeOption { get; set; }

        [Required(ErrorMessage = "Required")]
        public string averageAnimalNumber { get; set; }

        public string buttonPressed { get; set; }
        public string placehldr { get; set; }
        public bool isManureCollected { get; set; }
        public int durationDays { get; set; }
        public bool showDurationDays { get; set; }
        public List<Agri.Models.Farm.Animal> animals { get; set; }
    }
}
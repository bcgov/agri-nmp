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
        private UserData _ud;
        private IAgriConfigurationRepository _sd;

        public Animals(UserData ud, IAgriConfigurationRepository sd)
        {
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

            avm.Animals = new List<Agri.Models.Farm.Animal>();
            List<Agri.Models.Farm.Animal> animalList = _ud.GetAnimals();

            foreach (var a in animalList)
            {
                avm.Animals.Add(a);
            }
            if (avm.Animals.Count == 0)
            {
                avm.Actn = null;
                avm.Cntl = "Animals";
                avm.Act = "Add";
                avm.Target = "#animals";
                animalTypeDetailsSetup(ref avm);
            }

            return Task.FromResult(avm);
        }

        private void animalTypeDetailsSetup(ref AnimalsViewModel aavm)
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

    public class AnimalsViewModel
    {
        public int Id { get; set; }
        public string Act { get; set; }
        public string Cntl { get; set; }
        public string Target { get; set; }
        public string Actn { get; set; }
        public string Title { get; set; }
        public string BtnText { get; set; }
        public bool IsMainLoad { get; set; }

        //[Required(ErrorMessage = "Required")]
        //[Range(1, 9999, ErrorMessage = "Required")]
        public string SelectAnimalTypeOption { get; set; }

        public List<SelectListItem> AnimalTypeOptions { get; set; }
        public string AnimalTypeName { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public string SelectSubTypeOption { get; set; }

        public string SubTypeName { get; set; }
        public List<SelectListItem> SubTypeOptions { get; set; }

        //[Required(ErrorMessage = "Required")]
        //[Range(1, 9999, ErrorMessage = "Required")]
        public ManureMaterialType SelectManureMaterialTypeOption { get; set; }

        [Required(ErrorMessage = "Required")]
        public string AverageAnimalNumber { get; set; }

        public string ButtonPressed { get; set; }
        public string Placehldr { get; set; }
        public bool IsManureCollected { get; set; }
        public string ManureCollected { get; set; }
        public int DurationDays { get; set; }
        public bool ShowDurationDays { get; set; }
        public List<Agri.Models.Farm.Animal> Animals { get; set; }
    }
}
using Agri.Models.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Agri.Models.Farm
{
    public class Animal
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string BtnText { get; set; }

        //[Required(ErrorMessage = "Required")]
        //[Range(1, 9999, ErrorMessage = "Required")]
        public string SelectAnimalTypeOption { get; set; }

        public string AnimalTypeName { get; set; }
        public int AnimalId { get; set; }

        public List<SelectListItem> AnimalTypeOptions { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public string SelectSubTypeOption { get; set; }

        public string SubTypeName { get; set; }
        public int SubTypeId { get; set; }
        public List<SelectListItem> SubTypeOptions { get; set; }

        //[Required(ErrorMessage = "Required")]
        //[Range(1, 9999, ErrorMessage = "Required")]
        public ManureMaterialType SelectManureMaterialTypeOption { get; set; }

        [Required(ErrorMessage = "Required")]
        public string AverageAnimalNumber { get; set; }

        public string ButtonPressed { get; set; }
        public string Placehldr { get; set; }
        public string Target { get; set; }
        public bool IsManureCollected { get; set; }
        public string ManureCollected { get; set; }
        public int DurationDays { get; set; }
        public bool ShowDurationDays { get; set; }
    }
}
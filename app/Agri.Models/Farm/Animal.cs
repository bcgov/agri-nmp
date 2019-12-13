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
        public string title { get; set; }
        public string btnText { get; set; }

        //[Required(ErrorMessage = "Required")]
        //[Range(1, 9999, ErrorMessage = "Required")]
        public string selAnimalTypeOption { get; set; }

        public string animalTypeName { get; set; }
        public int animalId { get; set; }

        public List<SelectListItem> animalTypeOptions { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(1, 9999, ErrorMessage = "Required")]
        public string selSubTypeOption { get; set; }

        public string subTypeName { get; set; }
        public int subTypeId { get; set; }
        public List<SelectListItem> subTypeOptions { get; set; }

        //[Required(ErrorMessage = "Required")]
        //[Range(1, 9999, ErrorMessage = "Required")]
        public ManureMaterialType selManureMaterialTypeOption { get; set; }

        [Required(ErrorMessage = "Required")]
        public string averageAnimalNumber { get; set; }

        public string buttonPressed { get; set; }
        public string placehldr { get; set; }
        public string target { get; set; }
        public bool isManureCollected { get; set; }
        public string manureCollected { get; set; }
        public int durationDays { get; set; }
        public bool showDurationDays { get; set; }
        public decimal? ManureGeneratedTonsPerYear { get; set; }
    }
}
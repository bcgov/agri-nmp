using Agri.Models.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Agri.Models.Farm
{
    public class FarmAnimal
    {
        public int Id { get; set; }
        public int AnimalId { get; set; }
        public string AnimalName { get; set; }
        public string AnimalSubTypeName { get; set; }
        public int AnimalSubTypeId { get; set; }
        public ManureMaterialType ManureMaterialType { get; set; }
        public string AverageAnimalNumber { get; set; }
        public bool IsManureCollected { get; set; }
        public string ManureCollected { get; set; }
        public int DurationDays { get; set; }
        public int? ManureGeneratedTonsPerYear { get; set; }
    }
}
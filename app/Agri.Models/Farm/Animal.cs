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
        public string AnimalTypeName { get; set; }
        public int AnimalId { get; set; }
        public string SelectSubTypeOption { get; set; }
        public string SubTypeName { get; set; }
        public int SubTypeId { get; set; }
        public ManureMaterialType ManureMaterialType { get; set; }
        public string AverageAnimalNumber { get; set; }
        public bool IsManureCollected { get; set; }
        public string ManureCollected { get; set; }
        public int DurationDays { get; set; }
        public decimal? ManureGeneratedTonsPerYear { get; set; }
    }
}
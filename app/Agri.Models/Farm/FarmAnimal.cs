using Agri.Models.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Agri.Models.Farm
{
    public class FarmAnimal : GeneratedManure
    {
        public override string ManureId => $"FarmAnimal{Id ?? 0}";
        public bool IsManureCollected { get; set; }
        public string ManureCollected { get; set; }
        public int DurationDays { get; set; }
        public int? ManureGeneratedTonsPerYear { get; set; }

        public override bool AssignedToStoredSystem => false;
    }
}
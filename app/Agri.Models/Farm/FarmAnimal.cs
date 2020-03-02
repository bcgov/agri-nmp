using Agri.Models.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Agri.Models.Farm
{
    public class FarmAnimal : GeneratedManure
    {
        private int? manureGeneratedTonsPerYear;
        private int? manureGeneratedGallonsPerYear;

        public override string ManureId => $"FarmAnimal{Id ?? 0}";
        public bool IsManureCollected { get; set; }
        public string ManureCollected { get; set; }
        public int DurationDays { get; set; }

        public int? ManureGeneratedTonsPerYear
        {
            get => manureGeneratedTonsPerYear;

            set
            {
                if (manureGeneratedTonsPerYear.HasValue)
                {
                    manureGeneratedGallonsPerYear = null;
                }
                manureGeneratedTonsPerYear = value;
            }
        }

        public int? ManureGeneratedGallonsPerYear
        {
            get => manureGeneratedGallonsPerYear;

            set
            {
                if (manureGeneratedGallonsPerYear.HasValue)
                {
                    manureGeneratedTonsPerYear = null;
                }
                manureGeneratedGallonsPerYear = value;
            }
        }

        public bool IsPoultry { get; set; }
        public int? BirdsPerFlock { get; set; }
        public decimal? FlocksPerYear { get; set; }
        public int? DaysPerFlock { get; set; }

        public override bool AssignedToStoredSystem => false;
    }
}
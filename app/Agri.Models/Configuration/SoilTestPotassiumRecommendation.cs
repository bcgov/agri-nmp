﻿using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class SoilTestPotassiumRecommendation : ConfigurationBase
    {
        [Key]
        public int SoilTestPotassiumKelownaRangeId { get; set; }
        [Key]
        public int SoilTestPotassiumRegionCode { get; set; }
        [Key]
        public int PotassiumCropGroupRegionCode { get; set; }
        public int K2ORecommendationKilogramPerHectare { get; set; }

        public SoilTestPotassiumKelownaRange SoilTestPotassiumKelownaRange { get; set; }
    }
}
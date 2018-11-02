﻿using System.ComponentModel.DataAnnotations;

namespace Agri.Models.StaticData
{
    public class SoilTestPotassiumRange
    {
        [Key]
        public int UpperLimit { get; set; }
        public string Rating { get; set; }
    }
}
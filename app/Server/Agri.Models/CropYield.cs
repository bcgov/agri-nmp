﻿using System.ComponentModel.DataAnnotations;

namespace Agri.Models
{
    public class CropYield
    {
        [Key]
        public int CropId { get; set; }
        [Key]
        public int LocationId { get; set; }
        public decimal? Amt { get; set; }
        public Crop Crop { get; set; }
        public Location Location { get; set; }
    }
}
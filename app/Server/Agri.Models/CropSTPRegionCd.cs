﻿using System.ComponentModel.DataAnnotations;

namespace Agri.Models
{
    public class CropSTPRegionCd
    {
        [Key]
        public int CropId { get; set; }
        [Key]
        public int SoilTestPhosphorousRegionCd { get; set; }
        public int? PhosphorousCropGroupRegionCd { get; set; }
    }
}
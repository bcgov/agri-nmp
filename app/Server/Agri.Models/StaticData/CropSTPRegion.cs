﻿using System.ComponentModel.DataAnnotations;

namespace Agri.Models.StaticData
{
    public class CropSTPRegion
    {
        [Key]
        public int CropId { get; set; }
        [Key]
        public int SoilTestPhosphorousRegionCode { get; set; }
        public int? PhosphorousCropGroupRegionCode { get; set; }
    }
}
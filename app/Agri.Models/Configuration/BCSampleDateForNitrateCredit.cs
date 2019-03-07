﻿using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class BCSampleDateForNitrateCredit : Versionable
    {
        [Key]
        public string CoastalFromDateMonth { get; set; }
        public string CoastalToDateMonth { get; set; }
        public string InteriorFromDateMonth { get; set; }
        public string InteriorToDateMonth { get; set; }

    }
}
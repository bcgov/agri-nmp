﻿using System.Collections.Generic;

namespace Agri.Models.Farm
{
    public class FarmData
    {
        public FarmDetails farmDetails { get; set; }
        public bool unsaved { get; set; }
        public List<YearData> years { get; set; }
        public string LastAppliedFarmManureId { get; set; }
        public int? NMPReleaseVersion { get; set; }
    }
}


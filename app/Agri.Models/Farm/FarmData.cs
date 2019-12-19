using System;
using System.Collections.Generic;
using System.Linq;

namespace Agri.Models.Farm
{
    public class FarmData
    {
        public FarmDetails farmDetails { get; set; }
        public bool unsaved { get; set; }
        public List<YearData> years { get; set; }

        private string lastAppliedFarmManureId;

        public string LastAppliedFarmManureId
        {
            get
            {
                lastAppliedFarmManureId = VerifyAppliedFarmManureId(lastAppliedFarmManureId);
                return lastAppliedFarmManureId;
            }
            set
            {
                lastAppliedFarmManureId = value;
            }
        }

        public int? NMPReleaseVersion { get; set; }

        private string VerifyAppliedFarmManureId(string appliedFarmManureId)
        {
            var yearData = years.FirstOrDefault(y => y.Year == farmDetails.Year);
            if (yearData.FarmManures.Any(c => c.id == Convert.ToInt32(appliedFarmManureId)))
            {
                return appliedFarmManureId;
            }
            return null;
        }
    }
}
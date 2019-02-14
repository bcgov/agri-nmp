using System.Collections.Generic;
using Agri.Models;
using Agri.Models.Calculate;
using Agri.Models.Farm;
using SERVERAPI.Utility;

namespace SERVERAPI.ViewModels
{
    public class ReportStorages
    {
        public string storageName { get; set; }
        public string materialsGeneratedImported { get; set; }
        public string yardRunoff { get; set; }
        public string materialsStoredAfterSLSeparaton { get; set; }
        public string precipitationIntoStorage { get; set; }
        public string totalStored { get; set; }
        public string storageVolume { get; set; }
        public bool isThereSolidLiquidSeparation { get; set; }
        public string footNote { get; set; }
    }
}
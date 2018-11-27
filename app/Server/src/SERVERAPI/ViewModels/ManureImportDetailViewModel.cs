using Agri.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVERAPI.ViewModels
{
    public class ManureImportDetailViewModel
    {
        public string Title { get; set; }
        public string Target { get; set; }
        public int? ManureImportId { get; set; }
        public string MaterialName { get; set; }
        public ManureMaterialType ManureType { get; set; }
        public string ManureTypeName { get; set; }
        public bool IsLandAppliedBeforeStorage { get; set; }
        public string AnnualAmount { get; set; }
    }
}

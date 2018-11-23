using System.Collections.Generic;
using Agri.Models.Farm;
using SERVERAPI.Models;

namespace SERVERAPI.ViewModels
{
    public class CalculateViewModel
    {
        public bool regionFnd { get; set; }
        public int fldsFnd { get; set; }
        public string currFld { get; set; }
        public bool itemsPresent { get; set; }
        public string noData { get; set; }
        public List<Field> fields { get; set; }
        public StaticData.NutrientIcons icons { get; set; }
    }
}
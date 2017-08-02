using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SERVERAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace SERVERAPI.ViewModels
{
    public class ViewModels
    {
    }
    public class FarmViewModel
    {
        [Display(Name = "Farm Name")]
        public string farmName { get; set; }
        [Display(Name = "Do you have soil tests for for fields?")]
        public bool? soilTests { get; set; }
        [Display(Name = "Do you use manure or compost?")]
        public bool? manure { get; set; }
    }
}

using Agri.Models.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVERAPI.ViewModels
{
    public class FarmViewModelBase
    {
        public List<SelectListItem> regOptions { get; set; }
        [Display(Name = "Region")]
        public int? selRegOption { get; set; }
        public List<SelectListItem> subRegionOptions { get; set; }
        public int? selSubRegOption { get; set; }
        public bool showSubRegion { get; set; }
        public bool multipleSubRegion { get; set; }
        public string buttonPressed { get; set; }
    }
}

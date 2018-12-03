using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Agri.Models;
using Agri.Models.Configuration;

namespace SERVERAPI.ViewModels
{
    public class NavigationDetail
    {
        public string selMainMenuOption { get; set; }
        public List<SelectListItem> mainMenuOptions { get; set; }
        public string selSubMenuOption { get; set; }
        public string subTypeName { get; set; }
        public List<SelectListItem> subTypeOptions { get; set; }
    }
}
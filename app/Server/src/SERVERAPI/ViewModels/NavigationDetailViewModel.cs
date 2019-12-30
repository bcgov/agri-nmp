using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Agri.Models;
using Agri.Models.Configuration;

namespace SERVERAPI.ViewModels
{
    public class NavigationDetailViewModel
    {
        public string CurrentAction { get; set; }
        public string CurrentPage { get; set; }
        public List<MainMenu> MainMenus { get; set; }
        public List<SubMenu> SubMenus { get; set; }
        public bool UseInterceptJS { get; set; }
        public bool UsesFeaturePages => !string.IsNullOrEmpty(CurrentPage);
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Agri.Models;
using Agri.Models.Configuration;

namespace SERVERAPI.ViewModels
{
    public class NavigationDetailViewModel
    {
        public string selMainMenuOption { get; set; }
        public List<MainMenu> mainMenuOptions { get; set; }
        public string selSubMenuOption { get; set; }
        public string subTypeName { get; set; }
        public List<SubMenu> subMenuOptions { get; set; }
    }
}
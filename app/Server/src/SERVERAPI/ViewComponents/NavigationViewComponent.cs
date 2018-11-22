using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agri.Interfaces;
using Agri.Models.Farm;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Models;
using SERVERAPI.ViewModels;
using Agri.Models.Configuration;

namespace SERVERAPI.ViewComponents
{
    public class Navigation: ViewComponent
    {
        private IAgriConfigurationRepository _sd;
        private Models.Impl.UserData _ud;

        public Navigation(IAgriConfigurationRepository sd, Models.Impl.UserData ud)
        {
            _sd = sd;
            _ud = ud;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await GetNavigationAsync());
        }

        private Task<NavigationDetailViewModel> GetNavigationAsync()
        {
            NavigationDetailViewModel ndvm = new NavigationDetailViewModel();

            ndvm.mainMenuOptions = new List<MainMenu>();
            ndvm.mainMenuOptions = _sd.GetMainMenus();

            ndvm.subTypeOptions = new List<SelectListItem>();
            ndvm.subTypeOptions = _sd.GetSubmenusDll().ToList();

            return Task.FromResult(ndvm);
        }

        public class NavigationDetailViewModel
        {
            public string selMainMenuOption { get; set; }
            public List<MainMenu> mainMenuOptions { get; set; }
            public string selSubMenuOption { get; set; }
            public string subTypeName { get; set; }
            public List<SelectListItem> subTypeOptions { get; set; }
        }
    }
}

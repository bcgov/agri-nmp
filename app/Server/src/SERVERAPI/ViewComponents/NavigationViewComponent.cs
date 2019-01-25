using Agri.Interfaces;
using Agri.Models.Configuration;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

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

            ndvm.subMenuOptions = new List<SubMenu>();
            ndvm.subMenuOptions = _sd.GetSubMenus();

            var noManureCompost = !_ud.GetAllManagedManures().Any();  //Want true to grey out Storage and Nutrient Analysis

            ndvm.subMenuOptions
                .Where(sm => sm.Id == 3 || sm.Id == 4).ToList()
                .ForEach(xm => xm.GreyOutText = noManureCompost);

            return Task.FromResult(ndvm);
        }

    }
}

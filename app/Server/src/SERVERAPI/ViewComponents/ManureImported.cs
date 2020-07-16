using Agri.Data;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Models.Impl;
using SERVERAPI.ViewModels;
using System.Threading.Tasks;

namespace SERVERAPI.ViewComponents
{
    public class ManureImportedViewComponent : ViewComponent
    {
        private UserData _userData;
        private IAgriConfigurationRepository _sdData;

        public ManureImportedViewComponent(IAgriConfigurationRepository sdData, UserData ud)
        {
            _sdData = sdData;
            _userData = ud;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await GetManureImportedAsync());
        }

        private Task<ManureImportedViewModel> GetManureImportedAsync()
        {
            var manureImportedVm = new ManureImportedViewModel()
            {
                ImportedManures = _userData.GetImportedManures(),
            };

            return Task.FromResult(manureImportedVm);
        }
    }
}
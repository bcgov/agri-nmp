using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agri.Interfaces;
using Agri.Models;
using Agri.Models.Farm;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Models.Impl;
using SERVERAPI.ViewModels;

namespace SERVERAPI.ViewComponents
{
    public class ManureStorageViewComponent : ViewComponent
    {
        private UserData _userData;
        private IAgriConfigurationRepository _sdData;

        public ManureStorageViewComponent(IAgriConfigurationRepository sdData, UserData ud)
        {
            _sdData = sdData;
            _userData = ud;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await GetManureStorageSystemAsync());
        }

        private Task<ManureStorageViewModel> GetManureStorageSystemAsync()
        {
            var manureStorageVm = new ManureStorageViewModel
            {
                GeneratedManures = _userData.GetGeneratedManures(),
                ManureStorageSystems = _userData.GetStorageSystems()
            };

            manureStorageVm.ExplainMaterialsNeedingStorageMessage = _sdData.GetUserPrompt("explainmaterialsneedingstorage");

            return Task.FromResult(manureStorageVm);
        }
    }
}

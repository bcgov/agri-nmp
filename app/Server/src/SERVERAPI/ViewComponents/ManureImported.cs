using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agri.Interfaces;
using Agri.Models;
using Agri.Models.Farm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SERVERAPI.Models;
using SERVERAPI.Models.Impl;
using SERVERAPI.ViewModels;

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
                ImportedManures = _userData.GetImportedManures()
            };

            return Task.FromResult(manureImportedVm);
        }
    }
}

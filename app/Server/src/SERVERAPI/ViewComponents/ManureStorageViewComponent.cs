using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SERVERAPI.Models.Impl;
using SERVERAPI.ViewModels;

namespace SERVERAPI.ViewComponents
{
    public class ManureStorageViewComponent : ViewComponent
    {
        private UserData _userData;
        private StaticData _sdData;

        public ManureStorageViewComponent(Models.Impl.StaticData sdData, Models.Impl.UserData ud)
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
            var manureStorageVm = new ManureStorageViewModel();
            return Task.FromResult(manureStorageVm);
        }
    }
}

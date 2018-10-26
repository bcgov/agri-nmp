﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.ViewModels;

namespace SERVERAPI.ViewComponents
{
    public class ManureGeneratedObtained: ViewComponent
    {
        private Models.Impl.StaticData _sd;
        private Models.Impl.UserData _ud;

        public ManureGeneratedObtained(Models.Impl.StaticData sd, Models.Impl.UserData ud)
        {
            _sd = sd;
            _ud = ud;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await GetManureGeneratedObtainedAsync());
        }
         
        private Task<ManureGeneratedObtainedDetailViewModel> GetManureGeneratedObtainedAsync()
        {
            ManureGeneratedObtainedDetailViewModel fvm = new ManureGeneratedObtainedDetailViewModel();
            return Task.FromResult(fvm);
        }
    }
}

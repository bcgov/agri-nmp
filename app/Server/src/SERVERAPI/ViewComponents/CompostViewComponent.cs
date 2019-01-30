using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agri.Interfaces;
using Agri.Models.Farm;
using SERVERAPI.Models;
using SERVERAPI.Controllers;
using SERVERAPI.Models.Impl;
using Microsoft.AspNetCore.Hosting;
using SERVERAPI.ViewModels;

namespace SERVERAPI.ViewComponents
{
    public class Compost : ViewComponent
    {
        private IHostingEnvironment _env;
        private UserData _ud;
        private IAgriConfigurationRepository _sd;

        public Compost(IHostingEnvironment env, UserData ud, IAgriConfigurationRepository sd)
        {
            _env = env;
            _ud = ud;
            _sd = sd;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await GetCompostAsync());
        }

        private Task<CompostViewModel> GetCompostAsync()
        {
            CompostViewModel fvm = new CompostViewModel();
            fvm.composts = new List<FarmManure>();
            fvm.compostMsg = _sd.GetUserPrompt("compostmessage");

            List<FarmManure> compostList = _ud.GetFarmManures();

            foreach (var f in compostList)
            {
                fvm.composts.Add(f);
            }

            return Task.FromResult(fvm);
        }
    }

    public class CompostViewModel 
    {
        public List<FarmManure> composts { get; set; }
        public string compostMsg { get; set; }
    }
}

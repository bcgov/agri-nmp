using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SERVERAPI.Models;
using SERVERAPI.Controllers;
using SERVERAPI.Models.Impl;
using Microsoft.AspNetCore.Hosting;

namespace SERVERAPI.ViewComponents
{
    public class Compost : ViewComponent
    {
        private IHostingEnvironment _env;
        private UserData _ud;
        private Models.Impl.StaticData _sd;

        public Compost(IHostingEnvironment env, UserData ud, Models.Impl.StaticData sd)
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

            List<FarmManure> compostList = _ud.GetFarmManures();

            foreach (var f in compostList)
            {
                if (!f.customized)
                {
                    Models.StaticData.Manure man = _sd.GetManure(f.manureId.ToString());
                    f.ammonia = man.ammonia;
                    f.dmid = man.dmid;
                    f.manure_class = man.manure_class;
                    f.moisture = man.moisture;
                    f.name = man.name;
                    f.nitrogen = man.nitrogen;
                    f.nminerizationid = man.nminerizationid;
                    f.phosphorous = man.phosphorous;
                    f.potassium = man.potassium;
                    f.solid_liquid = man.solid_liquid;
                }
                fvm.composts.Add(f);
            }

            return Task.FromResult(fvm);
        }
    }

    public class CompostViewModel
    {
        public List<FarmManure> composts { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agri.Models.Farm;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Models;
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

        private Task<ManureGeneratedDetailViewModel> GetManureGeneratedObtainedAsync()
        {
            ManureGeneratedDetailViewModel mgovm = new ManureGeneratedDetailViewModel();
            mgovm.generatedManures = new List<GeneratedManure>();

            List<GeneratedManure> generatedManureList = _ud.GetGeneratedManures();
            foreach (var gml in generatedManureList)
            {
                mgovm.generatedManures.Add(gml);
            }

            return Task.FromResult(mgovm);
        }

        public class ManureGeneratedDetailViewModel
        {
            public List<GeneratedManure> generatedManures { get; set; }
        }
    }
}

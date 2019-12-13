using Agri.Models.Farm;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Models.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVERAPI.ViewComponents
{
    public class RanchManureViewComponent : ViewComponent
    {
        private readonly UserData _userData;

        public RanchManureViewComponent(UserData userData)
        {
            _userData = userData;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await GetManureImportedAsync());
        }

        private Task<RanchManureViewModel> GetManureImportedAsync()
        {
            var viewModel = new RanchManureViewModel
            {
                Animals = _userData.GetAnimals()
            };

            return Task.FromResult(viewModel);
        }

        public class RanchManureViewModel
        {
            public List<Animal> Animals { get; set; }

            public bool ShowManureCollected => Animals.Any(a => a.isManureCollected);
        }
    }
}
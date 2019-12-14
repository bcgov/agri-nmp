using Agri.Models.Farm;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Models.Impl;
using System.Collections.Generic;
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
                Animals = _userData.GetAnimals(),
                ImportedManures = _userData.GetImportedManures()
            };

            return Task.FromResult(viewModel);
        }

        public class RanchManureViewModel
        {
            public List<Animal> Animals { get; set; }

            public List<ImportedManure> ImportedManures { get; set; }
        }
    }
}
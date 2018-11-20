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
using StaticData = SERVERAPI.Models.Impl.StaticData;

namespace SERVERAPI.ViewComponents
{
    public class ManureNutrientAnalysisViewComponent : ViewComponent
    {
        private UserData _userData;
        private IAgriConfigurationRepository _sdData;

        public ManureNutrientAnalysisViewComponent(IAgriConfigurationRepository sdData, UserData ud)
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
            var generatedManures = new List<GeneratedManure>
            {
                new GeneratedManure
                {
                    animalId = 1,
                    animalSubTypeId = 1,
                    animalSubTypeName = "Cow, including calf to weaning",
                    manureType = ManureMaterialType.Solid
                },
                new GeneratedManure
                {
                    animalId = 1,
                    animalSubTypeId = 3,
                    animalSubTypeName = "Heavy Feeders",
                    manureType = ManureMaterialType.Solid
                },
                new GeneratedManure
                {
                    animalId = 2,
                    animalSubTypeId = 4,
                    animalSubTypeName = "Calves (0 to 3 months old)",
                    manureType = ManureMaterialType.Liquid
                },
                new GeneratedManure
                {
                    animalId = 2,
                    animalSubTypeId = 9,
                    animalSubTypeName = "Milking Cow",
                    manureType = ManureMaterialType.Liquid
                }
            };

            if (!_userData.GetGeneratedManures().Any())
            {
                foreach (var generatedManure in generatedManures)
                {
                    _userData.AddGeneratedManure(generatedManure);
                }
            }

            var manureStorageVm = new ManureStorageViewModel
            {
                GeneratedManures = _userData.GetGeneratedManures(),
                ManureStorageSystems = _userData.GetStorageSystems()
            };

            return Task.FromResult(manureStorageVm);
        }
    }
}

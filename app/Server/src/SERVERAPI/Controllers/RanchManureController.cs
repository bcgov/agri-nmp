using Agri.CalculateService;
using Agri.Data;
using Agri.Models;
using Agri.Models.Farm;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SERVERAPI.Filters;
using SERVERAPI.Models.Impl;
using SERVERAPI.ViewModels;
using System;
using System.Linq;

namespace SERVERAPI.Controllers
{
    [SessionTimeout]
    public class RanchManureController : BaseController
    {
        private readonly ILogger<ManureManagementController> _logger;
        private readonly UserData _userData;
        private readonly IAgriConfigurationRepository _sd;
        private readonly IMapper _mapper;
        private readonly IManureUnitConversionCalculator _manureUnitConversionCalculator;

        public RanchManureController(ILogger<ManureManagementController> logger,
            UserData userData,
            IAgriConfigurationRepository sd,
            IMapper mapper,
            IManureUnitConversionCalculator manureUnitConversionCalculator)
        {
            _logger = logger;
            _userData = userData;
            _sd = sd;
            _mapper = mapper;
            _manureUnitConversionCalculator = manureUnitConversionCalculator;
        }

        [HttpGet]
        public IActionResult RanchManure()
        {
            return View();
        }

        public IActionResult RanchManureImportedDetail(int? id, string target)
        {
            var vm = new ManureImportedDetailViewModel();

            if (id.HasValue)
            {
                var savedImportedManure = _userData.GetImportedManure(id.Value);
                vm = _mapper.Map<ImportedManure, ManureImportedDetailViewModel>(savedImportedManure);
            }
            else
            {
                vm.StandardSolidMoisture = _sd.GetManureImportedDefault().DefaultSolidMoisture;
                vm.Moisture = vm.StandardSolidMoisture;
                vm.IsMaterialStored = true;
                vm.SelectedManureType = ManureMaterialType.Solid;
            }

            vm.Title = "Imported Material Details";
            vm.Target = target;
            vm.IsMaterialStoredLabelText = _sd.GetUserPrompt("ImportMaterialIsMaterialAppliedQuestion");

            return PartialView("RanchManureImportedDetail", vm);
        }

        [HttpPost]
        public IActionResult RanchManureImportedDetail(ManureImportedDetailViewModel vm)
        {
            try
            {
                if (vm.ButtonPressed == "MaterialNameChange")
                {
                    ModelState.Clear();
                    vm.ButtonPressed = "";
                    vm.ButtonText = "Save";

                    return PartialView("RanchManureImportedDetail", vm);
                }

                if (vm.ButtonPressed == "ManureMaterialTypeChange")
                {
                    ModelState.Clear();
                    vm.ButtonPressed = "";
                    vm.ButtonText = "Save";

                    if (vm.SelectedManureType == ManureMaterialType.Liquid)
                    {
                        vm.Moisture = null;
                    }

                    return PartialView("RanchManureImportedDetail", vm);
                }

                if (vm.ButtonPressed == "IsMaterialStoredChange")
                {
                    ModelState.Clear();
                    vm.ButtonPressed = "";
                    vm.ButtonText = "Save";
                    return PartialView("RanchManureImportedDetail", vm);
                }

                if (vm.ButtonPressed == "ResetMoisture")
                {
                    ModelState.Clear();
                    vm.ButtonPressed = "";
                    vm.ButtonText = "Save";

                    vm.Moisture = vm.StandardSolidMoisture;
                    return PartialView("RanchManureImportedDetail", vm);
                }

                var existingNames = _userData.GetImportedManures()
                    .Where(im => !vm.ManureImportId.HasValue || (vm.ManureImportId.HasValue && im.Id != vm.ManureImportId))
                    .Select(im => im.MaterialName).ToList();
                if (existingNames.Any(n => n.Trim().Equals(vm.MaterialName, StringComparison.CurrentCultureIgnoreCase)))
                {
                    ModelState.AddModelError("MaterialName", "Use a new name");
                }

                if (vm.SelectedManureType == ManureMaterialType.Solid &&
                    (!vm.Moisture.HasValue || vm.Moisture.Value <= 0 || vm.Moisture > 100))
                {
                    ModelState.AddModelError("Moisture", "Enter a value between 0 and 100");
                }

                if (!vm.AnnualAmount.HasValue || vm.AnnualAmount < 0)
                {
                    ModelState.AddModelError("AnnualAmount", "Enter a numeric value");
                }

                if (ModelState.IsValid)
                {
                    var importedManure = _mapper.Map<ImportedManure>(vm);

                    if (vm.SelectedManureType == ManureMaterialType.Solid)
                    {
                        importedManure.AnnualAmountCubicMetersVolume =
                            _manureUnitConversionCalculator.GetCubicMetersVolume(importedManure.ManureType,
                                importedManure.Moisture.Value,
                                importedManure.AnnualAmount,
                                importedManure.Units);

                        importedManure.AnnualAmountCubicYardsVolume =
                            _manureUnitConversionCalculator.GetCubicYardsVolume(importedManure.ManureType,
                                importedManure.Moisture.Value,
                                importedManure.AnnualAmount,
                                importedManure.Units);

                        importedManure.AnnualAmountTonsWeight =
                            _manureUnitConversionCalculator.GetTonsWeight(importedManure.ManureType,
                                importedManure.Moisture.Value,
                                importedManure.AnnualAmount,
                                importedManure.Units);
                    }
                    else
                    {
                        importedManure.AnnualAmountUSGallonsVolume =
                            _manureUnitConversionCalculator.GetUSGallonsVolume(importedManure.ManureType,
                                importedManure.AnnualAmount,
                                importedManure.Units);
                    }

                    if (!vm.ManureImportId.HasValue)
                    {
                        _userData.AddImportedManure(importedManure);
                        vm.ManureImportId = importedManure.Id;
                    }
                    else
                    {
                        _userData.UpdateImportedManure(importedManure);
                    }

                    var url = Url.Action("RefreshImportList", "RanchManure");
                    return Json(new { success = true, url = url, target = vm.Target });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unexpected system error -" + ex.Message);
            }

            return PartialView("RanchManureImportedDetail", vm);
        }

        public IActionResult RefreshImportList()
        {
            return ViewComponent("RanchManure");
        }

        [HttpGet]
        public IActionResult RanchManureImportedDelete(int id, string target)
        {
            var vm = new ManureImportedDeleteViewModel();
            var manure = _userData.GetImportedManure(id);

            vm.Title = "Delete";
            vm.Target = target;
            vm.ImportManureName = manure.ManagedManureName;
            vm.ImportedManureId = id;
            vm.AppliedToAField = false;

            if (_userData.GetYearData().GetFieldsAppliedWithManure(manure).Any())
            {
                vm.AppliedToAField = true;
                vm.DeleteWarningForUnstorableMaterial = _sd.GetUserPrompt("ImportMaterialNotStoredDeleteWarning");
            }

            return PartialView("RanchManureImportedDelete", vm);
        }

        [HttpPost]
        public IActionResult RanchManureImportedDelete(ManureImportedDeleteViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _userData.DeleteImportedManure(vm.ImportedManureId);

                string url = Url.Action("RefreshImportList", "RanchManure");
                return Json(new { success = true, url = url, target = vm.Target });
            }

            return PartialView("RanchManureImportedDelete", vm);
        }
    }
}
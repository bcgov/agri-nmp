using Agri.CalculateService;
using Agri.Data;
using Agri.Models;
using Agri.Models.Farm;
using AutoMapper;
using Agri.Shared;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SERVERAPI.Models.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SERVERAPI.Pages.RanchManure
{
    public class CreateEdit : BasePageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public Command Data { get; set; }

        [BindProperty]
        public bool IsModal { get; set; }

        public CreateEdit(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task OnGetCreateAsync()
        {
            Data = await _mediator.Send(new Query());
        }

        public async Task OnGetEditAsync(Query query)
        {
            Data = await _mediator.Send(query);
        }

        private async Task SetExistingNamesValidation()
        {
            var validatorContext = new ValidationContext<Command>(Data);
            var existingNames = await _mediator.Send(new ExistingNamesQuery { ManureImportId = Data.ManureImportId });
            validatorContext.RootContextData["ExistingNames"] = existingNames;
            var commandValidator = new CommandValidator();
            commandValidator.Validate(validatorContext);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Data.PostedElementEvent == ElementEvent.MaterialNameChange)
            {
                ModelState.Clear();
                Data.PostedElementEvent = ElementEvent.None;
                Data.StateChanged = true;

                return Page();
            }

            if (Data.PostedElementEvent == ElementEvent.ManureMaterialTypeChange)
            {
                ModelState.Clear();
                Data.PostedElementEvent = ElementEvent.None;
                Data.StateChanged = true;

                if (Data.SelectedManureType == ManureMaterialType.Liquid)
                {
                    Data.Moisture = null;
                }

                return Page();
            }

            if (Data.PostedElementEvent == ElementEvent.ResetMoisture)
            {
                ModelState.Clear();
                Data.PostedElementEvent = ElementEvent.None;
                Data.StateChanged = true;

                Data.Moisture = Data.StandardSolidMoisture;
                return Page();
            }

            var existingNames = await _mediator.Send(new ExistingNamesQuery { ManureImportId = Data.ManureImportId });
            if (existingNames.Any(n => n.Trim().Equals(Data.MaterialName, StringComparison.CurrentCultureIgnoreCase)))
            {
                ModelState.AddModelError("Data.MaterialName", "Use a new name");
                return Page();
            }
            //await SetExistingNamesValidation();

            if (ModelState.IsValid)
            {
                await _mediator.Send(Data);
                return this.RedirectToPageJson(nameof(Index));
            }
            return Page();
        }

        public class Query : IRequest<Command>
        {
            public int? Id { get; set; }
        }

        public class ExistingNamesQuery : IRequest<List<string>>
        {
            public int? ManureImportId { get; set; }
        }

        public class Command : IRequest<Unit>
        {
            public int? ManureImportId { get; set; }
            public string Title { get; set; }
            public string Target { get; set; }
            public string MaterialName { get; set; }
            public ManureMaterialType SelectedManureType { get; set; }
            public string ManureTypeName { get; set; }
            public decimal? AnnualAmount { get; set; }
            public AnnualAmountUnits SelectedAnnualAmountUnit { get; set; }
            public decimal? Moisture { get; set; }
            public decimal StandardSolidMoisture { get; set; }

            public bool IsStdMoisture => SelectedManureType == ManureMaterialType.Solid && Moisture.HasValue &&
                                         Moisture.Value == StandardSolidMoisture;

            public bool IsMaterialStored { get; set; }
            public string IsMaterialStoredLabelText { get; set; }
            public bool StateChanged { get; set; }
            public ElementEvent PostedElementEvent { get; set; }

            public List<SelectListItem> GetAnnualAmountUnits()
            {
                var selectListItems = new List<SelectListItem>();

                if (SelectedManureType == ManureMaterialType.Solid)
                {
                    selectListItems.Add(new SelectListItem { Value = AnnualAmountUnits.CubicYards.ToString(), Text = AnnualAmountUnits.CubicYards.GetDescription() });
                    selectListItems.Add(new SelectListItem { Value = AnnualAmountUnits.tons.ToString(), Text = AnnualAmountUnits.tons.GetDescription() });
                    selectListItems.Add(new SelectListItem { Value = AnnualAmountUnits.CubicMeters.ToString(), Text = AnnualAmountUnits.CubicMeters.GetDescription() });
                    selectListItems.Add(new SelectListItem { Value = AnnualAmountUnits.tonnes.ToString(), Text = AnnualAmountUnits.tonnes.GetDescription() });
                }
                else
                {
                    selectListItems.Add(new SelectListItem { Value = AnnualAmountUnits.USGallons.ToString(), Text = AnnualAmountUnits.USGallons.GetDescription() });
                    selectListItems.Add(new SelectListItem { Value = AnnualAmountUnits.ImperialGallons.ToString(), Text = AnnualAmountUnits.ImperialGallons.GetDescription() });
                    selectListItems.Add(new SelectListItem { Value = AnnualAmountUnits.CubicMeters.ToString(), Text = AnnualAmountUnits.CubicMeters.GetDescription() });
                }

                return selectListItems;
            }
        }

        public enum ElementEvent
        {
            None,
            MaterialNameChange,
            ManureMaterialTypeChange,
            ResetMoisture
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(m => m.MaterialName).NotNull().NotEmpty().WithMessage("Required");
                RuleFor(m => m.AnnualAmount).NotEmpty().GreaterThan(0).WithMessage("Enter numeric value greater than 0");
                When(m => m.SelectedManureType == ManureMaterialType.Solid, () =>
                     {
                         RuleFor(m => m.Moisture).NotNull().GreaterThan(0).LessThanOrEqualTo(100)
                            .WithMessage("Enter a value between 0 and 100");
                     });

                RuleFor(x => x.MaterialName).Custom((x, context) =>
                {
                    if (context.ParentContext.RootContextData.ContainsKey("ExistingNames"))
                    {
                        var existingNames = context.ParentContext.RootContextData["ExistingNames"] as List<string>;
                        if (existingNames.Any(n => n.Trim().Equals(x, StringComparison.CurrentCultureIgnoreCase)))
                        {
                            context.AddFailure("Use a new name");
                        }
                    }
                });
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Command, ImportedManure>()
                .ForMember(m => m.Id, x => x.MapFrom(src => src.ManureImportId))
                .ForMember(m => m.ManureType, x => x.MapFrom(src => src.SelectedManureType))
                .ForMember(m => m.ManureTypeName, x => x.MapFrom(src => EnumHelper<ManureMaterialType>.GetDisplayValue(src.SelectedManureType)))
                .ForMember(m => m.Units, x => x.MapFrom(src => src.SelectedAnnualAmountUnit))
                .ReverseMap();
            }
        }

        public class QueryHandler : IRequestHandler<Query, Command>, IRequestHandler<ExistingNamesQuery, List<string>>
        {
            private readonly UserData _ud;
            private readonly IMapper _mapper;
            private readonly IAgriConfigurationRepository _sd;

            public QueryHandler(UserData ud, IMapper mapper, IAgriConfigurationRepository sd)
            {
                _ud = ud;
                _mapper = mapper;
                _sd = sd;
            }

            public async Task<Command> Handle(Query request, CancellationToken cancellationToken)
            {
                var command = new Command();
                if (request.Id.HasValue)
                {
                    var savedImportedManure = _ud.GetImportedManure(request.Id.Value);
                    command = _mapper.Map<ImportedManure, Command>(savedImportedManure);
                }
                else
                {
                    command.StandardSolidMoisture = _sd.GetManureImportedDefault().DefaultSolidMoisture;
                    command.Moisture = command.StandardSolidMoisture;
                    command.IsMaterialStored = true;
                    command.SelectedManureType = ManureMaterialType.Solid;
                }

                return await Task.FromResult(command);
            }

            public async Task<List<string>> Handle(ExistingNamesQuery request, CancellationToken cancellationToken)
            {
                var existingNames = _ud.GetImportedManures()
                    .Where(im => !request.ManureImportId.HasValue || (request.ManureImportId.HasValue && im.Id != request.ManureImportId))
                    .Select(im => im.MaterialName).ToList();

                return await Task.FromResult(existingNames);
            }
        }

        public class CommandHandler : IRequestHandler<Command, Unit>
        {
            private readonly UserData _ud;
            private readonly IMapper _mapper;
            private readonly IManureUnitConversionCalculator _manureUnitConversionCalculator;

            public CommandHandler(UserData ud, IMapper mapper,
                IManureUnitConversionCalculator manureUnitConversionCalculator)
            {
                _ud = ud;
                _mapper = mapper;
                _manureUnitConversionCalculator = manureUnitConversionCalculator;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var importedManure = _mapper.Map<ImportedManure>(request);

                if (importedManure.ManureType == ManureMaterialType.Solid)
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

                if (!importedManure.Id.HasValue)
                {
                    _ud.AddImportedManure(importedManure);
                    request.ManureImportId = importedManure.Id;
                }
                else
                {
                    _ud.UpdateImportedManure(importedManure);
                }

                return await Task.FromResult(new Unit());
            }
        }
    }
}
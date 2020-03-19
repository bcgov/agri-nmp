using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Agri.Data;
using Agri.Models;
using Agri.Models.Configuration;
using Agri.Models.Farm;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SERVERAPI.Models.Impl;

namespace SERVERAPI.Pages.Ranch.RanchNutrients
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
            Title = "Nutrient Analysis";
            _mediator = mediator;
        }

        public async Task OnGetCreateAsync(bool ismodal)
        {
            IsModal = ismodal;
            await PopulateData(new Query());
        }

        public async Task<IActionResult> OnGetEditAsync(bool ismodal, Query query)
        {
            IsModal = ismodal;
            await PopulateData(query);
            return Page();
        }

        private async Task PopulateData(Query query)
        {
            Data = await _mediator.Send(query);
            Data = await _mediator.Send(new LookupDataQuery { PopulatedData = Data });
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Data.ExcludedSourceOfMaterialIds.Clear();
            Data.IncludedSourceOfMaterialIds.Clear();

            if (Data.RanchSolidManures.Any(rm => !rm.Selected))
            {
                Data.ExcludedSourceOfMaterialIds
                    .AddRange(Data.RanchSolidManures.Where(rm => !rm.Selected).Select(rm => rm.ManureId).ToList());
            }
            if (Data.RanchSolidManures.Any(rm => rm.Selected))
            {
                var includedManures = Data.RanchSolidManures.Where(rm => rm.Selected).ToList();
                Data.IncludedSourceOfMaterialIds.AddRange(includedManures.Select(rm => rm.ManureId).ToList());
                Data.SourceOfMaterialName = string.Join(',', includedManures.Select(iam => iam.ManureName).ToList());
            }

            if (Data.RanchLiquidManures.Any(rm => !rm.Selected))
            {
                Data.ExcludedSourceOfMaterialIds
                    .AddRange(Data.RanchLiquidManures.Where(rm => !rm.Selected).Select(rm => rm.ManureId).ToList());
            }
            if (Data.RanchLiquidManures.Any(rm => rm.Selected))
            {
                var includedManures = Data.RanchLiquidManures.Where(rm => rm.Selected).ToList();
                Data.IncludedSourceOfMaterialIds.AddRange(includedManures.Select(rm => rm.ManureId).ToList());
                Data.SourceOfMaterialName = string.Join(',', includedManures.Select(iam => iam.ManureName).ToList());
            }

            if (Data.PostedElementEvent == ElementEvent.UseCustomAnalysis)
            {
                if (Data.UseCustomAnalysis)
                {
                    Data = await _mediator.Send(new BookValueQuery { PopulatedData = Data });
                }

                ModelState.Clear();
                Data.PostedElementEvent = ElementEvent.None;
            }
            else if (Data.PostedElementEvent == ElementEvent.NutrientAnalysisChanged)
            {
                Data.UseCustomAnalysis = false;
                ModelState.Clear();
                Data.PostedElementEvent = ElementEvent.None;
            }
            else
            {
                if (Data.UseCustomAnalysis)
                {
                    var validations = await _mediator.Send(new ValidationListsQuery { ManureName = Data.ManureName });

                    if (validations.FarmManures.Any(a => a.Customized && Data.UseCustomAnalysis &&
                         a.Name.Equals(Data.ManureName) &&
                         a.Id != Data.Id))
                    {
                        ModelState.AddModelError("Data.ManureName", "Descriptions must be unique.");
                    }

                    if (validations.NutrientAnalyticNamesUsedAsManureName)
                    {
                        ModelState.AddModelError("Data.ManureName", "Description cannot match predefined entries.");
                    }
                }

                if (ModelState.IsValid)
                {
                    if (Data.UseBookValue)
                    {
                        Data.Moisture = null;
                        Data.Nitrogen = null;
                        Data.Ammonia = null;
                        Data.Nitrate = null;
                        Data.Phosphorous = null;
                        Data.Potassium = null;
                        Data.ShowNitrate = false;
                        Data.Compost = false;
                    }

                    await _mediator.Send(Data);

                    if (IsModal)
                    {
                        return this.RedirectToPageJson(nameof(Index));
                    }
                    return RedirectToPage(nameof(Index));
                }
            }

            Data = await _mediator.Send(new LookupDataQuery { PopulatedData = Data });
            return Page();
        }

        public class Query : IRequest<Command>
        {
            public int? Id { get; set; }
        }

        public class LookupDataQuery : IRequest<Command>
        {
            public Command PopulatedData { get; set; }
        }

        public class BookValueQuery : IRequest<Command>
        {
            public Command PopulatedData { get; set; }
        }

        public class ValidationListsQuery : IRequest<ValidationLists>
        {
            public string ManureName { get; set; }
        }

        public class Command : IRequest<MediatR.Unit>
        {
            public int? Id { get; set; }
            public List<RanchManure> RanchSolidManures { get; set; } = new List<RanchManure>();
            public List<RanchManure> RanchLiquidManures { get; set; } = new List<RanchManure>();

            public bool ManuresSelected =>
                RanchSolidManures.Any(s => s.Selected) || RanchLiquidManures.Any(l => l.Selected);

            public int SelectedNutrientAnalysis { get; set; }
            public SelectList BeefNutrientAnalysisOptions { get; set; }
            public List<string> ExcludedSourceOfMaterialIds { get; set; } = new List<string>();
            public List<string> IncludedSourceOfMaterialIds { get; set; } = new List<string>();

            [Display(Name = "Material Type")]
            public string ManureName { get; set; }

            public string ManureClass { get; set; }

            public bool UseBookValue => !UseCustomAnalysis;

            [Display(Name = "Moisture (%)")]
            public string Moisture { get; set; }

            [Display(Name = "N (%)")]
            public decimal? Nitrogen { get; set; }

            [Display(Name = "NH<sub>4</sub>-N (ppm)")]
            public decimal? Ammonia { get; set; }

            [Display(Name = "P (%)")]
            public decimal? Phosphorous { get; set; }

            [Display(Name = "K (%)")]
            public decimal? Potassium { get; set; }

            [Display(Name = "NO<sub>3</sub>-N (ppm)")]
            public decimal? Nitrate { get; set; }

            public bool ShowNitrate { get; set; }
            public bool OnlyCustom { get; set; }
            public bool Compost { get; set; }
            public string SolidLiquid { get; set; }
            public ManureNutrientBookValues BookValues { get; set; }
            public NutrientAnalysisTypes StoredImported { get; set; }
            public string RanchNutrientAnalysisEntryCreateEditMessage { get; set; }
            public string ExplainNutrientAnalysisMoisture { get; set; }
            public string ExplainNutrientAnalysisNitrogen { get; set; }
            public string ExplainNutrientAnlalysisAmmonia { get; set; }
            public string ExplainNutrientAnlalysisPhosphorous { get; set; }
            public string ExplainNutrientAnlalysisPotassium { get; set; }
            public string SourceOfMaterialName { get; set; }

            public bool UseCustomAnalysis { get; set; }
            public ElementEvent PostedElementEvent { get; set; }

            public class RanchManure
            {
                public string ManureId { get; set; }
                public string ManureName { get; set; }
                public bool Selected { get; set; }
            }

            public class ManureNutrientBookValues
            {
                public string Moisture { get; set; }

                public decimal Nitrogen { get; set; }

                public decimal Ammonia { get; set; }

                public decimal Phosphorous { get; set; }

                public decimal Potassium { get; set; }
                //Beef manures don't have Custom Only or Nitrate
            }
        }

        public class ValidationLists
        {
            public List<FarmManure> FarmManures { get; set; }
            public bool NutrientAnalyticNamesUsedAsManureName { get; set; }
        }

        public enum ElementEvent
        {
            None,
            UseCustomAnalysis,
            NutrientAnalysisChanged,
            MaterialStateChanged,
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<FarmManure, Command>()
                    .ForMember(m => m.SelectedNutrientAnalysis, opts => opts.MapFrom(s => s.ManureId))
                    .ForMember(m => m.ManureName, opts => opts.MapFrom(s => s.Name))
                    .ForMember(m => m.UseCustomAnalysis, opts => opts.MapFrom(s => s.Customized))
                    .ReverseMap();
                CreateMap<ManagedManure, Command.RanchManure>()
                    .ForMember(m => m.ManureName, opts => opts.MapFrom(s => s.ManagedManureName));
                CreateMap<Manure, Command.ManureNutrientBookValues>();
            }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(m => m.ManuresSelected).Must(ms => ms == true)
                    .When(m => m.RanchLiquidManures.Any() || m.RanchSolidManures.Any())
                    .WithMessage("One or more Solid or Liquid manure must be selected");
                RuleFor(m => m.SelectedNutrientAnalysis).GreaterThan(0)
                    .WithMessage("A nutrient analysis must be selected");
                When(m => !m.UseBookValue, () =>
                {
                    RuleFor(m => m.ManureName).NotNull().WithMessage("Material Name is required")
                        .NotEmpty().WithMessage("Material Name is required");
                    RuleFor(m => m.Moisture).NotNull().WithMessage("Required")
                        .NotEmpty().WithMessage("Required")
                    .Must(BeNumericMoisture).WithMessage("Numbers only")
                    .Must(BeValidMosturePercent).WithMessage("Invalid %")
                    .Must(BeIfOtherSolidValidPercent).WithMessage("Must be \u2264 80%.")
                    .Must(BeIfOtherLiquidValidPercent).WithMessage("Must be > 80%.");

                    RuleFor(m => m.Nitrogen).NotNull().WithMessage("Required")
                    .InclusiveBetween(0, 100).WithMessage("Invalid %");
                    RuleFor(m => m.Ammonia).NotNull().WithMessage("Required");
                    RuleFor(m => m.Phosphorous).NotNull().WithMessage("Required")
                        .InclusiveBetween(0, 100).WithMessage("Invalid %");
                    RuleFor(m => m.Potassium).NotNull().WithMessage("Required")
                        .InclusiveBetween(0, 100).WithMessage("Invalid %");
                });
            }

            private static bool BeNumericMoisture(string moisture)
            {
                return IsMoistureNumeric(moisture, out decimal moistureDecimal);
            }

            private static bool BeValidMosturePercent(string moisture)
            {
                IsMoistureNumeric(moisture, out decimal moistureDecimal);

                if (moistureDecimal < 0 || moistureDecimal > 100)
                {
                    return false;
                }
                return true;
            }

            private static bool BeIfOtherSolidValidPercent(Command command, string moisture)
            {
                IsMoistureNumeric(moisture, out decimal moistureDecimal);

                if (!string.IsNullOrWhiteSpace(command.SolidLiquid) &&
                    command.SolidLiquid.Equals("Solid", StringComparison.OrdinalIgnoreCase) &&
                   command.ManureClass.Equals("Other", StringComparison.OrdinalIgnoreCase))
                {
                    if (moistureDecimal > 80)
                    {
                        return false;
                    }
                }
                return true;
            }

            private static bool BeIfOtherLiquidValidPercent(Command command, string moisture)
            {
                IsMoistureNumeric(moisture, out decimal moistureDecimal);

                if (!string.IsNullOrWhiteSpace(command.SolidLiquid) &&
                    command.SolidLiquid.Equals("Liquid", StringComparison.OrdinalIgnoreCase) &&
                   command.ManureClass.Equals("Other", StringComparison.OrdinalIgnoreCase))
                {
                    if (moistureDecimal <= 80)
                    {
                        return false;
                    }
                }
                return true;
            }

            private static bool IsMoistureNumeric(string moisture, out decimal moistureDecimal)
            {
                return decimal.TryParse(moisture, out moistureDecimal);
            }
        }

        public class QueryHandler : IRequestHandler<Query, Command>,
            IRequestHandler<BookValueQuery, Command>,
            IRequestHandler<LookupDataQuery, Command>,
            IRequestHandler<ValidationListsQuery, ValidationLists>
        {
            private readonly UserData _ud;
            private readonly IMapper _mapper;
            private readonly IAgriConfigurationRepository _sd;
            private readonly AgriConfigurationContext _db;

            public QueryHandler(UserData ud, IMapper mapper,
                IAgriConfigurationRepository sd,
                AgriConfigurationContext db)
            {
                _ud = ud;
                _mapper = mapper;
                _sd = sd;
                _db = db;
            }

            public async Task<Command> Handle(Query request, CancellationToken cancellationToken)
            {
                var command = new Command();

                if (request.Id.HasValue)
                {
                    var nutrientAnalytic = _ud.GetFarmManure(request.Id.Value);
                    command = _mapper.Map<Command>(nutrientAnalytic);

                    command.Compost = _sd.IsManureClassCompostType(command.ManureClass);
                    command.OnlyCustom = _sd.IsManureClassOtherType(nutrientAnalytic.ManureClass) ||
                        _sd.IsManureClassCompostType(nutrientAnalytic.ManureClass) ||
                        _sd.IsManureClassCompostClassType(nutrientAnalytic.ManureClass);
                    command.ShowNitrate = (_sd.IsManureClassCompostType(command.ManureClass) ||
                        _sd.IsManureClassCompostClassType(command.ManureClass));
                    if (command.OnlyCustom)
                    {
                        command.UseCustomAnalysis = true;
                    }
                }

                return await Task.FromResult(command);
            }

            public async Task<Command> Handle(LookupDataQuery request, CancellationToken cancellationToken)
            {
                var command = request.PopulatedData;
                command.RanchNutrientAnalysisEntryCreateEditMessage = _sd
                    .GetUserPrompt("RanchNutrientAnalysisEntryCreateEditMessage");

                var otherAnalyticIds = _ud.GetFarmManures()
                    .Where(fm => fm.Id != command.Id.GetValueOrDefault(0))
                    .SelectMany(fm => fm.IncludedSourceOfMaterialIds)
                    .ToList();

                var manures = _ud.GetAllManagedManures()
                    .Where(mm => (command.Id.HasValue && !otherAnalyticIds.Contains(mm.ManureId)) ||
                                    (!command.Id.HasValue && !mm.AssignedWithNutrientAnalysis));

                command.RanchSolidManures = _mapper
                    .Map<List<Command.RanchManure>>(manures.Where(m => m.ManureType == ManureMaterialType.Solid)
                    .ToList());

                command.RanchSolidManures.Select(m =>
                {
                    m.Selected = command.IncludedSourceOfMaterialIds.Contains(m.ManureId);
                    return m;
                }).ToList();

                command.RanchLiquidManures = _mapper
                    .Map<List<Command.RanchManure>>(manures.Where(m => m.ManureType == ManureMaterialType.Liquid)
                    .ToList());

                command.RanchLiquidManures.Select(m =>
                {
                    m.Selected = command.IncludedSourceOfMaterialIds.Contains(m.ManureId);
                    return m;
                }).ToList();

                if (command.RanchLiquidManures.Any(s => s.Selected))
                {
                    command.RanchSolidManures.Select(l => { l.Selected = false; return l; }).ToList();
                }
                else
                {
                    command.RanchSolidManures
                        .Select(l =>
                        {
                            l.Selected = !request.PopulatedData.ExcludedSourceOfMaterialIds.Any(m => m.Equals(l.ManureId));
                            return l;
                        }).ToList();
                }

                if (command.RanchSolidManures.Any(s => s.Selected))
                {
                    command.RanchLiquidManures.Select(l => { l.Selected = false; return l; }).ToList();
                }
                else
                {
                    command.RanchLiquidManures
                        .Select(l =>
                        {
                            l.Selected = !request.PopulatedData.ExcludedSourceOfMaterialIds.Any(m => m.Equals(l.ManureId));
                            return l;
                        }).ToList();
                }

                var beefManuresNutrients = _sd.GetManures();

                command.BeefNutrientAnalysisOptions = new SelectList(beefManuresNutrients
                    .Select(m => new { Id = m.Id, Name = m.Name }).ToList(), "Id", "Name");

                if (request.PopulatedData.SelectedNutrientAnalysis > 0)
                {
                    var nutrient = beefManuresNutrients.Single(m => m.Id == request.PopulatedData.SelectedNutrientAnalysis);
                    command.ManureClass = nutrient.ManureClass;
                    command.Compost = _sd.IsManureClassCompostType(nutrient.ManureClass);
                    command.OnlyCustom = _sd.IsManureClassOtherType(nutrient.ManureClass) ||
                        _sd.IsManureClassCompostType(nutrient.ManureClass) ||
                        _sd.IsManureClassCompostClassType(nutrient.ManureClass);
                    command.ShowNitrate = _sd.IsManureClassCompostClassType(nutrient.ManureClass) ||
                        _sd.IsManureClassCompostType(nutrient.ManureClass);

                    if (command.OnlyCustom)
                    {
                        command.UseCustomAnalysis = true;
                    }

                    if (command.UseCustomAnalysis)
                    {
                        command.SolidLiquid = nutrient.SolidLiquid;
                    }

                    command.BookValues = _mapper.Map<Command.ManureNutrientBookValues>(nutrient);
                }

                var prompts = _db.UserPrompts
                    .Where(p => p.UserPromptPage == UserPromptPage.NutrientsAnalysisCreateEdit.ToString() &&
                                    p.UserJourney == UserJourney.Ranch.ToString())
                    .ToDictionary(p => p.Name, p => p.Text);

                command.ExplainNutrientAnalysisMoisture = prompts["ExplainNutrientAnalysisMoisture-Ranch"];
                command.ExplainNutrientAnalysisNitrogen = prompts["ExplainNutrientAnalysisNitrogen-Ranch"];
                command.ExplainNutrientAnlalysisAmmonia = prompts["ExplainNutrientAnlalysisAmmonia-Ranch"];
                command.ExplainNutrientAnlalysisPhosphorous = prompts["ExplainNutrientAnlalysisPhosphorous-Ranch"];
                command.ExplainNutrientAnlalysisPotassium = prompts["ExplainNutrientAnlalysisPotassium-Ranch"];

                return await Task.FromResult(command);
            }

            public async Task<ValidationLists> Handle(ValidationListsQuery request, CancellationToken cancellationToken)
            {
                var validationLists = new ValidationLists
                {
                    FarmManures = _ud.GetFarmManures(),
                    NutrientAnalyticNamesUsedAsManureName = _sd.GetManures()
                        .Any(m => m.Name.Equals(request.ManureName, StringComparison.CurrentCultureIgnoreCase))
                };

                return await Task.FromResult(validationLists);
            }

            public Task<Command> Handle(BookValueQuery request, CancellationToken cancellationToken)
            {
                var command = request.PopulatedData;

                var manure = _sd.GetManure(request.PopulatedData.SelectedNutrientAnalysis);
                command.BookValues = _mapper.Map<Command.ManureNutrientBookValues>(manure);
                command.ManureName = !request.PopulatedData.Compost ?
                    "Custom - " + manure.Name + " - " : "Custom - " + manure.SolidLiquid + " - ";

                return Task.FromResult(command);
            }
        }

        public class CommandHandler : IRequestHandler<Command, MediatR.Unit>
        {
            private readonly UserData _ud;
            private readonly IMapper _mapper;

            public CommandHandler(UserData ud, IMapper mapper)
            {
                _ud = ud;
                _mapper = mapper;
            }

            public async Task<MediatR.Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var farmManure = _mapper.Map<FarmManure>(request);

                farmManure.StoredImported = NutrientAnalysisTypes.Collected;

                if (request.Id.HasValue)
                {
                    _ud.UpdateFarmManure(farmManure);
                    _ud.ReCalculateManure(farmManure.Id);
                }
                else
                {
                    _ud.AddFarmManure(farmManure);
                }
                _ud.UpdateManagedFarmAnimalsAllocationToNutrientAnalysis();
                _ud.UpdateManagedImportedManuresAllocationToNutrientAnalysis();

                return await Task.FromResult(new MediatR.Unit());
            }
        }
    }
}
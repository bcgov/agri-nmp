using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Agri.Data;
using Agri.Models;
using Agri.Models.Farm;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SERVERAPI.Models.Impl;

namespace SERVERAPI.Pages.RanchNutrients
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
            if (Data.PostedElementEvent == ElementEvent.UseCustomAnalysis)
            {
                Data.UseBookValue = !Data.UseCustomAnalysis;
                ModelState.Clear();
                Data.PostedElementEvent = ElementEvent.None;
            }
            else if (Data.PostedElementEvent == ElementEvent.MaterialStateChanged)
            {
                ModelState.Clear();
                Data.PostedElementEvent = ElementEvent.None;
            }
            else if (Data.PostedElementEvent == ElementEvent.NutrientAnalysisChanged)
            {
                Data.ShowCustomCheckbox = Data.SelectedNutrientAnalysis > 0;
                ModelState.Clear();
                Data.PostedElementEvent = ElementEvent.None;
            }
            else
            {
                if (ModelState.IsValid)
                {
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

        public class Command : IRequest<Unit>
        {
            public int? Id { get; set; }
            public List<RanchManure> RanchManures { get; set; }
            public int SelectedNutrientAnalysis { get; set; }
            public SelectList BeefNutrientAnalysisOptions { get; set; }

            public string Action { get; set; }
            public string SourceOfMaterialName { get; set; }

            [Display(Name = "Material Type")]
            public string ManureName { get; set; }

            public ManureMaterialType MaterialType { get; set; }

            [Display(Name = "Moisture (%)")]
            public decimal? Moisture { get; set; }

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

            public bool UseBookValue { get; set; } = true;
            public bool Compost { get; set; }
            public decimal MoistureBook { get; set; }
            public decimal NitrogenBook { get; set; }
            public decimal AmmoniaBook { get; set; }
            public decimal NitrateBook { get; set; }
            public decimal PhosphorousBook { get; set; }
            public decimal PotassiumBook { get; set; }
            public bool ShowNitrate { get; set; }
            public NutrientAnalysisTypes StoredImported { get; set; }
            public string ExplainNutrientAnalysisMoisture { get; set; }
            public string ExplainNutrientAnalysisNitrogen { get; set; }
            public string ExplainNutrientAnlalysisAmmonia { get; set; }
            public string ExplainNutrientAnlalysisPhosphorous { get; set; }
            public string ExplainNutrientAnlalysisPotassium { get; set; }
            public bool IsLegacyNMPReleaseVersion { get; set; }
            public int? LegacyNMPReleaseVersionManureId { get; set; }
            public bool UseCustomAnalysis { get; set; }
            public bool ShowCustomCheckbox { get; set; }
            public ElementEvent PostedElementEvent { get; set; }

            public class RanchManure
            {
                public string ManureId { get; set; }
                public string ManureName { get; set; }

                [IgnoreMap]
                public bool Selected { get; set; }
            }
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
                CreateMap<ManagedManure, Command.RanchManure>()
                    .ForMember(m => m.ManureName, opts => opts.MapFrom(s => s.ManagedManureName));
                //CreateMap<ImportedManure, Command.RanchManure>()
                //    .ForMember(m => m.ManureName, opts => opts.MapFrom(s => s.ManagedManureName));
            }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                When(m => !m.UseBookValue, () =>
                {
                    RuleFor(m => m.ManureName).NotNull().WithMessage("Material Name is required")
                        .NotEmpty().WithMessage("Material Name is required");
                    RuleFor(m => m.Moisture).NotNull().WithMessage("Required")
                        .InclusiveBetween(0, 100).WithMessage("Invalid %");
                    //TODO Moisture for Manure type
                    RuleFor(m => m.Nitrogen).NotNull().WithMessage("Required")
                        .InclusiveBetween(0, 100).WithMessage("Invalid %");
                    RuleFor(m => m.Ammonia).NotNull().WithMessage("Required");
                    RuleFor(m => m.Phosphorous).NotNull().WithMessage("Required")
                        .InclusiveBetween(0, 100).WithMessage("Invalid %");
                    RuleFor(m => m.Potassium).NotNull().WithMessage("Required")
                        .InclusiveBetween(0, 100).WithMessage("Invalid %");
                    When(m => m.ShowNitrate, () =>
                    {
                        RuleFor(m => m.Nitrate).NotNull().WithMessage("Required");
                    });
                });
            }
        }

        public class Handler : IRequestHandler<Query, Command>,
            IRequestHandler<LookupDataQuery, Command>
        {
            private readonly UserData _ud;
            private readonly IMapper _mapper;
            private readonly IAgriConfigurationRepository _sd;
            private readonly AgriConfigurationContext _db;

            public Handler(UserData ud, IMapper mapper,
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

                var manures = _ud.GetAllManagedManures();
                command.RanchManures = _mapper.Map<List<Command.RanchManure>>(manures);

                return await Task.FromResult(command);
            }

            public Task<Command> Handle(LookupDataQuery request, CancellationToken cancellationToken)
            {
                var command = request.PopulatedData;
                //var ranchManures = _ud.
                var beefManuresNutrients = _sd.GetManures()
                    .Where(m => m.ManureClass.Contains("Beef"))
                    .Select(m => new { Id = m.Id, Name = m.Name })
                    .ToList();

                command.BeefNutrientAnalysisOptions = new SelectList(beefManuresNutrients, "Id", "Name");

                if (request.PopulatedData.SelectedNutrientAnalysis > 0)
                {
                }

                if (!request.PopulatedData.UseBookValue)
                {
                    var prompts = _db.UserPrompts
                        .Where(p => new List<string>
                        {
                        "NutrientAnalysisMoistureMessage","NutrientAnlalysisNitrogenMessage", "NutrientAnlalysisAmmoniaMessage",
                        "NutrientAnlalysisPhosphorousMessage", "NutrientAnlalysisPotassiumMessage"
                        }.Any(s => s.Equals(p.Name)))
                        .ToDictionary(p => p.Name, p => p.Text);

                    command.ExplainNutrientAnalysisMoisture = prompts["NutrientAnalysisMoistureMessage"];
                    command.ExplainNutrientAnalysisNitrogen = prompts["NutrientAnlalysisNitrogenMessage"];
                    command.ExplainNutrientAnlalysisAmmonia = prompts["NutrientAnlalysisAmmoniaMessage"];
                    command.ExplainNutrientAnlalysisPhosphorous = prompts["NutrientAnlalysisPhosphorousMessage"];
                    command.ExplainNutrientAnlalysisPotassium = prompts["NutrientAnlalysisPotassiumMessage"];
                }

                return Task.FromResult(command);
            }
        }
    }
}
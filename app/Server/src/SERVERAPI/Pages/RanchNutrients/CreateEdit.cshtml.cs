﻿using System;
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
            Data.IncludedSourceOfMaterialIds.Clear();

            if (Data.RanchManures.Any(rm => rm.Selected))
            {
                Data.IncludedSourceOfMaterialIds.AddRange(Data.RanchManures.Where(rm => rm.Selected).Select(rm => rm.ManureId).ToList());
            }

            if (Data.PostedElementEvent == ElementEvent.UseCustomAnalysis)
            {
                Data.UseBookValue = !Data.UseCustomAnalysis;

                if (Data.UseCustomAnalysis)
                {
                    Data = await _mediator.Send(new BookValueQuery { PopulatedData = Data });
                }

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
                if (Data.UseCustomAnalysis)
                {
                    var validations = await _mediator.Send(new ValidationListsQuery { ManureName = Data.ManureName });

                    if (validations.FarmManures.Any(a => a.Customized && Data.UseCustomAnalysis &&
                         a.Name.Equals(Data.ManureName) &&
                         a.Id != Data.Id))
                    {
                        ModelState.AddModelError("ManureName", "Descriptions must be unique.");
                    }

                    if (validations.NutrientAnalyticNamesUsedAsManureName)
                    {
                        ModelState.AddModelError("ManureName", "Description cannot match predefined entries.");
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
                        Data.ManureName = null;
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

        //public async Task<IActionResult> OnPostEditAsync()
        //{
        //    return Page();
        //}

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
            public List<RanchManure> RanchManures { get; set; }
            public int SelectedNutrientAnalysis { get; set; }
            public SelectList BeefNutrientAnalysisOptions { get; set; }
            public List<string> IncludedSourceOfMaterialIds { get; set; } = new List<string>();

            [Display(Name = "Material Type")]
            public string ManureName { get; set; }

            public bool UseBookValue { get; set; } = true;

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

            public ManureNutrientBookValues BookValues { get; set; }
            public NutrientAnalysisTypes StoredImported { get; set; }

            public string ExplainNutrientAnalysisMoisture { get; set; }
            public string ExplainNutrientAnalysisNitrogen { get; set; }
            public string ExplainNutrientAnlalysisAmmonia { get; set; }
            public string ExplainNutrientAnlalysisPhosphorous { get; set; }
            public string ExplainNutrientAnlalysisPotassium { get; set; }
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
                RuleFor(m => m.RanchManures).Must(m => m.Any(rm => rm.Selected))
                    .WithMessage("One or more materials must be checked");
                RuleFor(m => m.SelectedNutrientAnalysis).GreaterThan(0)
                    .WithMessage("A nutrient analysis must be selected");
                When(m => !m.UseBookValue, () =>
                {
                    RuleFor(m => m.ManureName).NotNull().WithMessage("Material Name is required")
                        .NotEmpty().WithMessage("Material Name is required");
                    RuleFor(m => m.Moisture).NotNull().WithMessage("Required")
                        .NotEmpty().WithMessage("Required")
                        .Custom((moisture, context) =>
                        {
                            decimal moistureDecimal;
                            if (!decimal.TryParse(moisture, out moistureDecimal))
                            {
                                context.AddFailure("Moisture", "Numbers only");
                            }
                            else
                            {
                                if (moistureDecimal < 0 || moistureDecimal > 100)
                                {
                                    context.AddFailure("Moisture", "Invalid %");
                                }
                            }
                        });
                    RuleFor(m => m.Nitrogen).NotNull().WithMessage("Required")
                    .InclusiveBetween(0, 100).WithMessage("Invalid %");
                    RuleFor(m => m.Ammonia).NotNull().WithMessage("Required");
                    RuleFor(m => m.Phosphorous).NotNull().WithMessage("Required")
                        .InclusiveBetween(0, 100).WithMessage("Invalid %");
                    RuleFor(m => m.Potassium).NotNull().WithMessage("Required")
                        .InclusiveBetween(0, 100).WithMessage("Invalid %");
                });
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
                }

                return await Task.FromResult(command);
            }

            public async Task<Command> Handle(LookupDataQuery request, CancellationToken cancellationToken)
            {
                var command = request.PopulatedData;

                var manures = _ud.GetAllManagedManures();
                command.RanchManures = _mapper.Map<List<Command.RanchManure>>(manures);
                foreach (var manure in command.RanchManures)
                {
                    manure.Selected = request.PopulatedData.IncludedSourceOfMaterialIds.Any(m => m.Equals(manure.ManureId));
                }

                var beefManuresNutrients = _sd.GetManures()
                    .Where(m => m.ManureClass.Contains("Beef"))
                    .ToList();

                command.BeefNutrientAnalysisOptions = new SelectList(beefManuresNutrients
                    .Select(m => new { Id = m.Id, Name = m.Name }).ToList(), "Id", "Name");

                if (!request.PopulatedData.UseBookValue)
                {
                    if (request.PopulatedData.SelectedNutrientAnalysis > 0)
                    {
                        var manure = beefManuresNutrients.Single(m => m.Id == request.PopulatedData.SelectedNutrientAnalysis);
                        command.BookValues = _mapper.Map<Command.ManureNutrientBookValues>(manure);
                    }

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
                command.ManureName = "Custom - " + manure.Name + " - ";

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

                if (request.Id.HasValue)
                {
                    _ud.UpdateFarmManure(farmManure);
                    _ud.ReCalculateManure(farmManure.Id);
                }
                else
                {
                    _ud.AddFarmManure(farmManure);
                }
                _ud.UpdateManagedImportedManuresAllocationToNutrientAnalysis();

                return await Task.FromResult(new MediatR.Unit());
            }
        }
    }
}
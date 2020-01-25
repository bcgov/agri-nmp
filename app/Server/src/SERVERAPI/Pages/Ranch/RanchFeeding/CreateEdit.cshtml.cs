using Agri.Data;
using Agri.Models.Configuration;
using Agri.Models.Farm;
using Agri.Models.Settings;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SERVERAPI.Models.Impl;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SERVERAPI.Pages.Ranch.RanchFeeding
{
    public class CreateEdit : BasePageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public Command Data { get; set; }

        [BindProperty]
        public string FieldName { get; set; }

        public CreateEdit(IMediator mediator) => _mediator = mediator;

        public async Task OnGetCreateAsync(string fieldName)
        {
            FieldName = fieldName;
            Title = fieldName + " Feeding Area - Add Feed/Forage";
            await PopulateData(new Query());
        }

        public async Task OnGetEditAsync(string fieldName, Query query)
        {
            FieldName = fieldName;
            Title = "Edit Field";
            await PopulateData(query);
        }

        private async Task PopulateData(Query query)
        {
            Data = await _mediator.Send(query);
            Data = await _mediator.Send(new LookupDataQuery { PopulatedData = Data });
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            return await ProcessPost();
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            return await ProcessPost();
        }

        private async Task<IActionResult> ProcessPost()
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(Data);
                return RedirectToPage(nameof(Index));
            }
            Data = await _mediator.Send(new LookupDataQuery { PopulatedData = Data });
            return Page();
        }

        public class Query : IRequest<Command>
        {
            public int Id { get; set; }
        }

        public class LookupDataQuery : IRequest<Command>
        {
            public Command PopulatedData { get; set; }
        }

        [BindProperties]
        public class Command : IRequest<MediatR.Unit>
        {
            public int Id { get; set; }
            public string FieldName { get; set; }
            //public Nutrients Nutrients { get; set; }

            //public List<FieldCrop> Crops { get; set; }
            //public SoilTest SoilTest { get; set; }

            public string FieldArea { get; set; }
            public string FieldComment { get; set; }

            public List<PreviousManureApplicationYear> SelectPrevYrManureOptions { get; set; }
            public string SelectPrevYrManureOption { get; set; }
            public string PrevYearManureApplicationFrequency { get; set; }
            public int? PrevYearManureApplicationNitrogenCredit { get; set; }

            //public decimal? SoilTestNitrateOverrideNitrogenCredit { get; set; }
            public string Placehldr { get; set; }

            public bool IsSeasonalFeedingArea { get; set; }
            public string SeasonalFeedingArea { get; set; }
            public string FeedingDaysSpentInFeedingArea { get; set; }
            public string FeedingPercentageOutsideFeeingArea { get; set; }
            public string MatureAnimalCount { get; set; }
            public string GrowingAnimalCount { get; set; }
            public string MatureAnimalAverageWeight { get; set; }
            public string GrowingAnimalAverageWeight { get; set; }
            public List<DailyFeedRequirement> SelectDailyFeedOptions { get; set; }
            public string MatureAnimalDailyFeedRequirementId { get; set; }
            public string GrowingAnimalDailyFeedRequirementId { get; set; }
            public string DailyFeedWarning { get; set; }
            public string FeedName { get; set; }
            public string FeedType { get; set; }
            public string ForageName { get; set; }
            public bool IsBookAnalysis { get; set; }
            public bool IsCustomValues { get; set; }
            public List<FeedForageAnalysis> FeedForageAnalyses { get; set; }

            public class FeedForageAnalysis
            {
                public int Id { get; set; }
                public int FeedForageTypeId { get; set; }
                public int FeedForageId { get; set; }
                public bool UseBookValues { get; set; }
                public decimal DryMatterPercent { get; set; }
                public decimal CrudeProteinPercent { get; set; }
                public decimal Phosphorus { get; set; }
                public decimal Potassium { get; set; }
                public decimal PercentOfTotalFeedForageToAnimals { get; set; }
                public decimal PercentOfFeedForageWastage { get; set; }
            }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            private readonly IOptions<AppSettings> _appSettings;

            public CommandValidator(IOptions<AppSettings> appSettings)
            {
                _appSettings = appSettings;
                RuleFor(x => x.FieldName).NotNull().WithMessage("Field Name is required");
                RuleFor(x => x.FieldArea).NotNull().WithMessage("Field Area is required");
                RuleFor(x => x.SelectPrevYrManureOption).NotEqual("select").WithMessage("Manure application in previous years must be selected");
                RuleFor(x => x.FieldComment).MaximumLength(Convert.ToInt32(_appSettings.Value.CommentLength)).WithMessage("Exceeds maximum length of " + _appSettings.Value.CommentLength);
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Field, Command>()
                //Command as Destination
                .ForMember(m => m.FieldArea, opts => opts.MapFrom(s => s.Area.ToString("G29")))
                .ForMember(m => m.FeedingPercentageOutsideFeeingArea, opts => opts.MapFrom(s => s.FeedingPercentageOutsideFeeingArea != null ? s.FeedingPercentageOutsideFeeingArea.Value.ToString("G29") : null))
                .ForMember(m => m.FeedingDaysSpentInFeedingArea, opts => opts.MapFrom(s => s.FeedingDaysSpentInFeedingArea != null ? s.FeedingDaysSpentInFeedingArea.Value.ToString("G29") : null))
                .ForMember(m => m.MatureAnimalCount, opts => opts.MapFrom(s => s.MatureAnimalCount != null ? s.MatureAnimalCount.Value.ToString("G29") : null))
                .ForMember(m => m.GrowingAnimalCount, opts => opts.MapFrom(s => s.GrowingAnimalCount != null ? s.GrowingAnimalCount.Value.ToString("G29") : null))
                .ForMember(m => m.MatureAnimalAverageWeight, opts => opts.MapFrom(s => s.MatureAnimalAverageWeight != null ? s.MatureAnimalAverageWeight.Value.ToString("G29") : null))
                .ForMember(m => m.GrowingAnimalAverageWeight, opts => opts.MapFrom(s => s.GrowingAnimalAverageWeight != null ? s.GrowingAnimalAverageWeight.Value.ToString("G29") : null))
                .ForMember(m => m.FieldComment, opts => opts.MapFrom(s => s.Comment))
                .ForMember(m => m.SelectPrevYrManureOption, opts => opts.MapFrom(s => s.PreviousYearManureApplicationFrequency))
                .ReverseMap()
                //FarmField as Destination
                .ForMember(m => m.Area, opts => opts.MapFrom(s => s.FieldArea != null ? Convert.ToDecimal(s.FieldArea) : 0))
                .ForMember(m => m.SeasonalFeedingArea, opts => opts.MapFrom(s => s.IsSeasonalFeedingArea ? "Yes" : "No")); ;
            }
        }

        public class Handler : IRequestHandler<Query, Command>
        {
            private readonly UserData _ud;
            private readonly IMapper _mapper;

            public Handler(UserData ud, IMapper mapper)
            {
                _ud = ud;
                _mapper = mapper;
            }

            public async Task<Command> Handle(Query request, CancellationToken cancellationToken)
            {
                var command = new Command();
                if (request.Id != 0)
                {
                    var field = _ud.GetFieldDetailById(request.Id);
                    command = _mapper.Map<Command>(field);
                }

                return await Task.FromResult(command);
            }
        }

        public class LookupDataHandler : IRequestHandler<LookupDataQuery, Command>
        {
            private readonly IAgriConfigurationRepository _sd;

            public LookupDataHandler(IAgriConfigurationRepository sd)
            {
                _sd = sd;
            }

            public async Task<Command> Handle(LookupDataQuery request, CancellationToken cancellationToken)
            {
                var command = request.PopulatedData;
                //command.SelectPrevYrManureOptions = _sd.GetPrevManureApplicationInPrevYears();
                //command.SelectDailyFeedOptions = _sd.GetDailyFeedRequirement();
                //if (command.MatureAnimalDailyFeedRequirementId == null)
                //{
                //    command.MatureAnimalDailyFeedRequirementId = command.SelectDailyFeedOptions[0].Name;
                //}
                //if (command.GrowingAnimalDailyFeedRequirementId == null)
                //{
                //    command.GrowingAnimalDailyFeedRequirementId = command.SelectDailyFeedOptions[0].Name;
                //}
                //command.Placehldr = _sd.GetUserPrompt("fieldcommentplaceholder");
                //command.DailyFeedWarning = _sd.GetUserPrompt("DailyFeedWarning");
                return await Task.FromResult(command);
            }
        }

        public class CommandHandler : IRequestHandler<Command, MediatR.Unit>
        {
            private readonly UserData _ud;
            private readonly IMapper _mapper;
            private readonly IOptions<AppSettings> _appSettings;

            public CommandHandler(UserData ud, IMapper mapper, IOptions<AppSettings> appSettings)
            {
                _ud = ud;
                _mapper = mapper;
                _appSettings = appSettings;
            }

            public async Task<MediatR.Unit> Handle(Command message, CancellationToken cancellationToken)
            {
                var field = new Field();
                field = _mapper.Map<Field>(message);

                if (field.Id == 0)//Need to check here
                {
                    _ud.AddField(field);
                }
                else
                {
                    _ud.UpdateField(field);
                }

                return await Task.FromResult(new MediatR.Unit());
            }
        }
    }
}
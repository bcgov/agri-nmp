using Agri.Data;
using Agri.Models;
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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SERVERAPI.Pages.Ranch.RanchFields
{
    public class CreateEdit : BasePageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public Command Data { get; set; }

        [BindProperty]
        public bool IsModal { get; set; }

        public string PageLayout => IsModal ? null : PageConstants.PageLayout;

        public CreateEdit(IMediator mediator) => _mediator = mediator;

        public async Task OnGetCreateAsync(bool ismodal)
        {
            IsModal = ismodal;
            Title = "Add Field";
            await PopulateData(new Query());
        }

        public async Task OnGetEditAsync(bool ismodal, Query query)
        {
            IsModal = ismodal;
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

                if (IsModal)
                {
                    return this.RedirectToPageJson(nameof(Index));
                }
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
            public decimal? FieldArea { get; set; }
            public string FieldComment { get; set; }

            public List<PreviousManureApplicationYear> SelectPrevYrManureOptions { get; set; }
            public string SelectPrevYrManureOption { get; set; }
            public string PrevYearManureApplicationFrequency { get; set; }
            public int? PrevYearManureApplicationNitrogenCredit { get; set; }

            public string Placehldr { get; set; }

            public bool IsSeasonalFeedingArea { get; set; }
            public string SeasonalFeedingArea { get; set; }
            public int? FeedingDaysSpentInFeedingArea { get; set; }
            public int? FeedingPercentageOutsideFeeingArea { get; set; }
            public int? MatureAnimalCount { get; set; }
            public int? GrowingAnimalCount { get; set; }
            public int? MatureAnimalAverageWeight { get; set; }
            public int? GrowingAnimalAverageWeight { get; set; }
            public List<DailyFeedRequirement> SelectDailyFeedOptions { get; set; }
            public int? MatureAnimalDailyFeedRequirementId { get; set; }
            public int? GrowingAnimalDailyFeedRequirementId { get; set; }
            public string DailyFeedWarning { get; set; }
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

                .ForMember(m => m.FieldComment, opts => opts.MapFrom(s => s.Comment))
                .ForMember(m => m.SelectPrevYrManureOption, opts => opts.MapFrom(s => s.PreviousYearManureApplicationFrequency))
                .ForMember(m => m.FieldArea, opts => opts.MapFrom(s => s.Area))
                .ReverseMap()
                //FarmField as Destination
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
            private readonly AgriConfigurationContext _db;

            public LookupDataHandler(IAgriConfigurationRepository sd,
                AgriConfigurationContext db)
            {
                _sd = sd;
                _db = db;
            }

            public async Task<Command> Handle(LookupDataQuery request, CancellationToken cancellationToken)
            {
                var command = request.PopulatedData;
                command.SelectPrevYrManureOptions = _sd.GetPrevManureApplicationInPrevYears();
                command.SelectDailyFeedOptions = _sd.GetDailyFeedRequirement();
                if (command.MatureAnimalDailyFeedRequirementId.GetValueOrDefault(0) == 0)
                {
                    command.MatureAnimalDailyFeedRequirementId = command.SelectDailyFeedOptions[0].Id;
                }
                if (command.GrowingAnimalDailyFeedRequirementId.GetValueOrDefault(0) == 0)
                {
                    command.GrowingAnimalDailyFeedRequirementId = command.SelectDailyFeedOptions[0].Id;
                }

                var prompts = _db.UserPrompts
                    .Where(p => p.UserPromptPage == UserPromptPage.FieldCreateEdit.ToString() &&
                                    p.UserJourney == UserJourney.Ranch.ToString())
                    .ToDictionary(p => p.Name, p => p.Text);

                command.Placehldr = prompts["fieldcommentplaceholder-Ranch"];
                command.DailyFeedWarning = prompts["DailyFeedWarning"];

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
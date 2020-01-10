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

namespace SERVERAPI.Pages.RanchFields
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
            public int? FeedingValueDays { get; set; }
            public decimal? FeedingPercentage { get; set; }
            public int? MatureAnimalCount { get; set; }
            public int? GrowingAnimalCount { get; set; }
            public decimal? MatureAnimalAverage { get; set; }
            public decimal? GrowingAnimalAverage { get; set; }
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
.ForMember(m => m.FieldArea, opts => opts.MapFrom(s => s.area.ToString("G29")))
.ForMember(m => m.FieldComment, opts => opts.MapFrom(s => s.comment))
.ForMember(m => m.SelectPrevYrManureOption, opts => opts.MapFrom(s => s.prevYearManureApplicationFrequency))
.ReverseMap()
//FarmField as Destination
.ForMember(m => m.area, opts => opts.MapFrom(s => s.FieldArea != null ? Convert.ToDecimal(s.FieldArea) : 0))
.ForMember(m => m.SeasonalFeedingArea, opts => opts.MapFrom(s => s.IsSeasonalFeedingArea ? "Yes" : "No"))
;
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
                    //command.FieldName = field.fieldName;
                    //command.Id = field.Id;
                    //command.FieldArea = field.area.ToString("G29");
                    //command.IsSeasonalFeedingArea = field.IsSeasonalFeedingArea;
                    //command.SeasonalFeedingArea = field.SeasonalFeedingArea;
                    //command.FieldComment = field.comment;
                    //command.SelectPrevYrManureOption = field.prevYearManureApplicationFrequency;
                    //command.FeedingValueDays = field.FeedingValueDays;
                    //command.FeedingPercentage = field.FeedingPercentage;
                    //command.MatureAnimalAverage = field.MatureAnimalAverage;
                    //command.MatureAnimalCount = field.MatureAnimalCount;
                    //command.GrowingAnimalAverage = field.GrowingAnimalAverage;
                    //command.GrowingAnimalCount = field.GrowingAnimalCount;

                    //command.FeedingValueDays = field.feedingValueDays;
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
                command.SelectPrevYrManureOptions = _sd.GetPrevManureApplicationInPrevYears();
                command.Placehldr = _sd.GetUserPrompt("fieldcommentplaceholder");
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
                //var field = _mapper.Map<Command, Field>(message);
                var field = new Field();
                //field.Id = message.Id;
                //field.fieldName = message.FieldName;
                //field.area = message.FieldArea != null ? Convert.ToDecimal(message.FieldArea) : 0;
                //field.comment = message.FieldComment;
                //field.prevYearManureApplicationFrequency = message.SelectPrevYrManureOption;
                //field.IsSeasonalFeedingArea = message.IsSeasonalFeedingArea;
                //field.SeasonalFeedingArea = message.IsSeasonalFeedingArea ? "Yes" : "No";
                field = _mapper.Map<Field>(message);

                //field.FeedingValueDays = message.FeedingValueDays;
                //field.FeedingPercentage = message.FeedingPercentage;
                //field.MatureAnimalAverage = message.MatureAnimalAverage;
                //field.MatureAnimalCount = message.MatureAnimalCount;
                //field.GrowingAnimalAverage = message.GrowingAnimalAverage;
                //field.GrowingAnimalCount = message.GrowingAnimalCount;

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
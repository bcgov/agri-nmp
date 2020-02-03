using Agri.Data;
using Agri.Models.Configuration;
using Agri.Models.Farm;
using Agri.Models.Settings;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SERVERAPI.Models.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public CreateEdit(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task OnGetCreateAsync(string fieldName, Query query)
        {
            FieldName = fieldName;
            Title = fieldName + " Feeding Area - Add Feed/Forage";
            await PopulateData(query);
        }

        public async Task OnGetEditAsync(string fieldName, Query query)
        {
            Title = fieldName + " Feeding Area - Edit Feed/Forage";
            await PopulateData(query);
        }

        private async Task PopulateData(Query query)
        {
            Data = await _mediator.Send(query);
            Data = await _mediator.Send(new LookupDataQuery { PopulatedData = Data });
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Data.PostedElementEvent == ElementEvent.AddFeedForageAnalysis)
            {
                ModelState.Clear();
                Data.PostedElementEvent = ElementEvent.None;
                Data.StateChanged = true;

                var newId = Data.FeedForageAnalyses.Count + 1;
                Data.FeedForageAnalyses.Add(new Command.FeedForageAnalysis { Id = newId });
            }
            else if (Data.PostedElementEvent == ElementEvent.FeedForageChange)
            {
                ModelState.Clear();
                Data.PostedElementEvent = ElementEvent.None;
                Data.StateChanged = true;
            }
            else if (Data.PostedElementEvent == ElementEvent.UseCustomAnalysis)
            {
                ModelState.Clear();
                Data.PostedElementEvent = ElementEvent.None;
                Data.StateChanged = true;
            }
            else
            {
                if (Data.FeedForageAnalyses.Sum(f => f.PercentOfTotalFeedForageToAnimals) != 100)
                {
                    for (int i = 0; i < Data.FeedForageAnalyses.Count; i++)
                    {
                        ModelState.AddModelError($"Data.FeedForageAnalyses[{i}].PercentOfTotalFeedForageToAnimals", Data.FeedingAreaWarning);
                    }
                }

                //var context = new ValidationContext<Command>(Data);
                //context.RootContextData["TotalFeedForageMessage"] = Data.FeedingAreaWarning;
                //var validator = new CommandValidator();
                //validator.Validate(context);

                if (ModelState.IsValid)
                {
                    await _mediator.Send(Data);
                    return this.RedirectToPageJson(nameof(Index));
                }
            }
            Data = await _mediator.Send(new LookupDataQuery { PopulatedData = Data });
            return Page();
        }

        public class Query : IRequest<Command>
        {
            public string FieldName { get; set; }
        }

        public class LookupDataQuery : IRequest<Command>
        {
            public Command PopulatedData { get; set; }
        }

        [BindProperties]
        public class Command : IRequest<MediatR.Unit>
        {
            public string FieldName { get; set; }

            public List<FeedForageAnalysis> FeedForageAnalyses { get; set; } = new List<FeedForageAnalysis>();

            public ElementEvent PostedElementEvent { get; set; }
            public bool StateChanged { get; set; }

            public string FeedingAreaWarning { get; set; }

            public class FeedForageAnalysis
            {
                private decimal? crudeProteinPercent;
                private decimal? phosphorus;
                private decimal? potassium;
                private decimal? percentOfTotalFeedForageToAnimals;
                private decimal? percentOfFeedForageWastage;

                public int Id { get; set; }
                public int? FeedForageTypeId { get; set; }
                public int? FeedForageId { get; set; }
                public bool UseBookValues { get; set; } = true;

                public decimal? CrudeProteinPercent
                {
                    get => crudeProteinPercent.HasValue ? Math.Round(crudeProteinPercent.Value, 2) : default(decimal?);
                    set => crudeProteinPercent = value;
                }

                public decimal? Phosphorus
                {
                    get => phosphorus.HasValue ? Math.Round(phosphorus.Value, 2) : default(decimal?);
                    set => phosphorus = value;
                }

                public decimal? Potassium
                {
                    get => potassium.HasValue ? Math.Round(potassium.Value, 2) : default(decimal?);
                    set => potassium = value;
                }

                public decimal? PercentOfTotalFeedForageToAnimals
                {
                    get => percentOfTotalFeedForageToAnimals.HasValue ? Math.Round(percentOfTotalFeedForageToAnimals.Value, 2) : default(decimal?);
                    set => percentOfTotalFeedForageToAnimals = value;
                }

                public decimal? PercentOfFeedForageWastage
                {
                    get => percentOfFeedForageWastage.HasValue ? Math.Round(percentOfFeedForageWastage.Value, 2) : default(decimal?);
                    set => percentOfFeedForageWastage = value;
                }

                public List<FeedForageType> SelectFeedTypeOptions { get; set; }
                public List<Feed> SelectFeedNameOptions { get; set; }
            }
        }

        public class CommandFeedForageAnalysisValidator : AbstractValidator<Command.FeedForageAnalysis>
        {
            public CommandFeedForageAnalysisValidator()
            {
                RuleFor(m => m.FeedForageTypeId).GreaterThan(0).WithMessage("Feed Forage Type must be selected");
                RuleFor(m => m.FeedForageId).GreaterThan(0)
                    .When(m => m.FeedForageTypeId > 0).WithMessage("Feed Forage must be selected");
                When(m => !m.UseBookValues, () =>
                {
                    RuleFor(m => m.CrudeProteinPercent).NotEmpty().WithMessage("Must be a valid percent")
                        .GreaterThanOrEqualTo(0).WithMessage("Must be a valid percent");
                    RuleFor(m => m.Phosphorus).NotEmpty().WithMessage("Must be a valid percent")
                        .GreaterThanOrEqualTo(0).WithMessage("Must be a valid percent");
                    RuleFor(m => m.Potassium).NotEmpty().WithMessage("Must be a valid percent")
                        .GreaterThanOrEqualTo(0).WithMessage("Must be a valid percent");
                });

                RuleFor(m => m.PercentOfTotalFeedForageToAnimals)
                    .NotEmpty().WithMessage("Requires a valid number less than or equal to 100")
                    .LessThanOrEqualTo(100).WithMessage("Requires a valid number less than or equal to 100");
                RuleFor(m => m.PercentOfFeedForageWastage)
                    .NotEmpty().WithMessage("Requires a valid number less than or equal to 100")
                    .LessThanOrEqualTo(100).WithMessage("Requires a valid number less than or equal to 100");
            }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleForEach(m => m.FeedForageAnalyses)
                    .SetValidator(new CommandFeedForageAnalysisValidator());
                //RuleFor(m => m.FeedForageAnalyses).Custom((list, context) =>
                //{
                //    if (list.Sum(l => l.PercentOfTotalFeedForageToAnimals) != 100)
                //    {
                //        var message = context.ParentContext.RootContextData["TotalFeedForageMessage"].ToString();
                //        for (int i = 0; i < list.Count; i++)
                //        {
                //            var propName = $"FeedForageAnalyses[{i}].PercentOfTotalFeedForageToAnimals";
                //            context.AddFailure(new ValidationFailure(propName, message));
                //        }
                //    }
                //});
            }
        }

        public enum ElementEvent
        {
            None,
            UseCustomAnalysis,
            AddFeedForageAnalysis,
            FeedForageChange,
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Field, Command>();
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
                if (!string.IsNullOrEmpty(request.FieldName))
                {
                    command.FieldName = request.FieldName;
                    var feedForageAnalyses = _ud.GetFeedForageAnalysis(request.FieldName);
                    foreach (var feed in feedForageAnalyses)
                    {
                        command.FeedForageAnalyses.Add(new Command.FeedForageAnalysis()
                        {
                            Id = feed.Id,
                            FeedForageId = feed.FeedForageId,
                            FeedForageTypeId = feed.FeedForageTypeId,
                            UseBookValues = feed.UseBookValues,
                            CrudeProteinPercent = feed.CrudeProteinPercent,
                            Phosphorus = feed.Phosphorus,
                            Potassium = feed.Potassium,
                            PercentOfTotalFeedForageToAnimals = feed.PercentOfTotalFeedForageToAnimals,
                            PercentOfFeedForageWastage = feed.PercentOfFeedForageWastage
                        });
                    }
                    if (feedForageAnalyses.Count == 0)
                    {
                        command.FeedForageAnalyses.Add(new Command.FeedForageAnalysis
                        {
                            Id = 1
                        });
                    }
                }
                else
                {
                    command.FeedForageAnalyses.Add(new Command.FeedForageAnalysis
                    {
                        Id = 1
                    });
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

                //command.FeedingAreaWarning = _sd.GetUserPrompt("feedingcommentplaceholder-Ranch");
                command.FeedingAreaWarning = "Percent Sum must be 100";

                foreach (var feedAnalysis in command.FeedForageAnalyses)
                {
                    feedAnalysis.SelectFeedTypeOptions = _sd.GetFeedForageTypes();
                    feedAnalysis.SelectFeedNameOptions = _sd.GetFeedForageNames();
                    if (feedAnalysis.FeedForageId == 0 && feedAnalysis.FeedForageTypeId == 0)
                    {
                        feedAnalysis.UseBookValues = true;
                    }
                    if (feedAnalysis.FeedForageTypeId != 0)
                    {
                        var selectedFeedType = feedAnalysis.SelectFeedTypeOptions.Find(x => x.Id == feedAnalysis.FeedForageTypeId);
                        if (selectedFeedType != null)
                        {
                            var nameOptions = new List<Feed>();
                            foreach (var feedName in feedAnalysis.SelectFeedNameOptions)
                            {
                                if (feedName.FeedForageTypeId == feedAnalysis.FeedForageTypeId)
                                {
                                    nameOptions.Add(feedName);
                                }
                            }
                            feedAnalysis.SelectFeedNameOptions = nameOptions;
                        }
                    }

                    if (feedAnalysis.FeedForageId != 0)
                    {
                        var selectedFeedName = feedAnalysis.SelectFeedNameOptions.Find(x => x.Id == feedAnalysis.FeedForageId);
                        if (selectedFeedName != null)
                        {
                            if (feedAnalysis.UseBookValues)
                            {
                                feedAnalysis.CrudeProteinPercent = Convert.ToDecimal(selectedFeedName.CPPercent);
                                feedAnalysis.Phosphorus = Convert.ToDecimal(selectedFeedName.PhosphorousPercent);
                                feedAnalysis.Potassium = Convert.ToDecimal(selectedFeedName.PotassiumPercent);
                            }
                        }
                    }
                }

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
                //var field = new Field();
                //vb wer sxfield = _mapper.Map<Field>(message);
                foreach (var feed in message.FeedForageAnalyses)
                {
                    var feedAnalysis = _ud.GetFeedForageAnalysisDetail(feed.Id, message.FieldName);
                    var feedForage = _mapper.Map<FeedForageAnalysis>(feed);
                    if (feedAnalysis != null)
                    {
                        _ud.UpdateFeedForageAnalysis(feedForage, message.FieldName);
                    }
                    else
                    {
                        _ud.AddFeedForageAnalysis(feedForage, message.FieldName);
                    }
                }

                return await Task.FromResult(new MediatR.Unit());
            }
        }
    }
}
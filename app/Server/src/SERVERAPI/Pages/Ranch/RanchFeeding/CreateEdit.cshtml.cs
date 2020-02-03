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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static SERVERAPI.Pages.Ranch.RanchFeeding.CreateEdit.Command;

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
            if (Data.PostedElementEvent == "AddFeedForageAnalysis")
            {
                ModelState.Clear();
                Data.PostedElementEvent = "None";
                Data.StateChanged = true;

                var newId = Data.FeedForageAnalyses.Count + 1;
                Data.FeedForageAnalyses.Add(new Command.FeedForageAnalysis { Id = newId });
            }
            else if (Data.PostedElementEvent == "FeedForageChange")
            {
                ModelState.Clear();
                Data.PostedElementEvent = "None";
                Data.StateChanged = true;
            }
            else
            {
                if (ModelState.IsValid)
                {
                    await _mediator.Send(Data);
                    return RedirectToPage(nameof(Index));
                }
            }
            Data = await _mediator.Send(new LookupDataQuery { PopulatedData = Data });
            return Page();
        }

        public class Query : IRequest<Command>
        {
            public string fieldName { get; set; }
        }

        public class LookupDataQuery : IRequest<Command>
        {
            public Command PopulatedData { get; set; }
        }

        [BindProperties]
        public class Command : IRequest<MediatR.Unit>
        {
            public string FieldName { get; set; }

            public List<FeedingArea> feedingAreas { get; set; }
            public List<FeedForageAnalysis> FeedForageAnalyses { get; set; } = new List<FeedForageAnalysis>();
            public string PostedElementEvent { get; set; }
            public bool StateChanged { get; set; }

            public class FeedingArea
            {
                public string DailyFeedWarning { get; set; }
                public bool isAvailable { get; set; }
                public string FeedName { get; set; }
                public string FeedType { get; set; }
                public string ForageName { get; set; }
                public bool IsBookAnalysis { get; set; }
                public bool IsCustomValues { get; set; }
            }

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

                public List<FeedForageType> selectFeedTypeOptions { get; set; }
                public List<Feed> selectFeedNameOptions { get; set; }
            }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            private readonly IOptions<AppSettings> _appSettings;
            private IAgriConfigurationRepository _sd;

            public CommandValidator(IOptions<AppSettings> appSettings, IAgriConfigurationRepository sd)
            {
                _appSettings = appSettings;
                _sd = sd;

                //FeedingAreaWarning = _sd.GetUserPrompt("feedingcommentplaceholder-Ranch")
                RuleFor(x => x.FeedForageAnalyses.Sum(y => y.PercentOfTotalFeedForageToAnimals)).Equal(100).WithMessage(_sd.GetUserPrompt("feedingcommentplaceholder-Ranch"));
            }
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
                if (!string.IsNullOrEmpty(request.fieldName))
                {
                    command.FieldName = request.fieldName;
                    var feedForageAnalyses = _ud.GetFeedForageAnalysis(request.fieldName);
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
                foreach (var feedAnalysis in command.FeedForageAnalyses)
                {
                    feedAnalysis.selectFeedTypeOptions = _sd.GetFeedForageTypes();
                    feedAnalysis.selectFeedNameOptions = _sd.GetFeedForageNames();
                    if (feedAnalysis.FeedForageId == 0 && feedAnalysis.FeedForageTypeId == 0)
                    {
                        feedAnalysis.UseBookValues = true;
                    }
                    if (feedAnalysis.FeedForageTypeId != 0)
                    {
                        var selectedFeedType = feedAnalysis.selectFeedTypeOptions.Find(x => x.Id == feedAnalysis.FeedForageTypeId);
                        if (selectedFeedType != null)
                        {
                            var nameOptions = new List<Feed>();
                            foreach (var feedName in feedAnalysis.selectFeedNameOptions)
                            {
                                if (feedName.FeedForageTypeId == feedAnalysis.FeedForageTypeId)
                                {
                                    nameOptions.Add(feedName);
                                }
                            }
                            feedAnalysis.selectFeedNameOptions = nameOptions;
                        }
                    }

                    if (feedAnalysis.FeedForageId != 0)
                    {
                        var selectedFeedName = feedAnalysis.selectFeedNameOptions.Find(x => x.Id == feedAnalysis.FeedForageId);
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
                    var feedForage = _mapper.Map<Agri.Models.Farm.FeedForageAnalysis>(feed);
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
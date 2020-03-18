﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Agri.Data;
using Agri.Models;
using Agri.Models.Farm;
using Agri.Shared;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SERVERAPI.Models.Impl;

namespace SERVERAPI.Pages.Ranch.RanchNutrients
{
    public class Index : BasePageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public Model Data { get; set; }

        public Index(IMediator mediator)
        {
            Title = "Nutrient Analysis";
            _mediator = mediator;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Data = await _mediator.Send(new Query());

            if (!Data.RanchManures.Any() && !Data.ManureAnalytics.Any())
            {
                if (Request.Headers["referer"].ToString().Contains("RanchManure"))
                {
                    return RedirectToPage(FeaturePages.RanchFieldsIndex.GetDescription());
                }
                else
                {
                    return RedirectToPage(FeaturePages.RanchManureIndex.GetDescription());
                }
            }
            if (!Data.ManureAnalytics.Any())
            {
                return RedirectToPage("CreateEdit", "Create", new { ismodal = false });
            }
            return Page();
        }

        public class Query : IRequest<Model>
        {
        }

        public class Model
        {
            public List<ManureNutrientAnalysis> ManureAnalytics { get; set; }
            public List<RanchManure> RanchManures { get; set; }
            public string RanchNutrientAnalysisEntryListMessage { get; set; }

            public class ManureNutrientAnalysis
            {
                public int Id { get; set; }
                public string Name { get; set; }
                public string ManureClass { get; set; }
                public string SolidLiquid { get; set; }
                public string Moisture { get; set; }
                public decimal Nitrogen { get; set; }
                public decimal Ammonia { get; set; }
                public decimal Phosphorous { get; set; }
                public decimal Potassium { get; set; }
                public int DMId { get; set; }
                public int NMinerizationId { get; set; }
            }

            public class RanchManure
            {
                public string ManureId { get; set; }
                public string ManureName { get; set; }
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<FarmManure, Model.ManureNutrientAnalysis>()
                    .ForMember(m => m.Moisture, opts => opts.MapFrom(s => s.Customized ? $"{s.Moisture}%" : s.Moisture));
                CreateMap<ManagedManure, Model.RanchManure>()
                    .ForMember(m => m.ManureName, opts => opts.MapFrom(s => s.ManagedManureName));
            }
        }

        public class Handler : IRequestHandler<Query, Model>
        {
            private readonly UserData _ud;
            private readonly IMapper _mapper;
            private readonly AgriConfigurationContext _db;

            public Handler(UserData ud, IMapper mapper, AgriConfigurationContext db)
            {
                _ud = ud;
                _mapper = mapper;
                _db = db;
            }

            public async Task<Model> Handle(Query request, CancellationToken cancellationToken)
            {
                var farmManures = _ud.GetFarmManures();
                var managedManure = _ud.GetAllManagedManures().Where(mm => !mm.AssignedWithNutrientAnalysis).ToList();
                var message = _db.UserPrompts
                    .Single(p => p.UserPromptPage == UserPromptPage.NutrientsAnalysisList.ToString() &&
                                p.UserJourney == UserJourney.Ranch.ToString()).Text;

                var model = new Model
                {
                    ManureAnalytics = _mapper.Map<List<Model.ManureNutrientAnalysis>>(farmManures),
                    RanchManures = _mapper.Map<List<Model.RanchManure>>(managedManure),
                    RanchNutrientAnalysisEntryListMessage = message
                };

                return await Task.FromResult(model);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Agri.Models.Farm;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SERVERAPI.Models.Impl;

namespace SERVERAPI.Pages.RanchNutrients
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
                CreateMap<FarmManure, Model.ManureNutrientAnalysis>();
                CreateMap<ManagedManure, Model.RanchManure>()
                    .ForMember(m => m.ManureName, opts => opts.MapFrom(s => s.ManagedManureName));
            }
        }

        public class Handler : IRequestHandler<Query, Model>
        {
            private readonly UserData _ud;
            private readonly IMapper _mapper;

            public Handler(UserData ud, IMapper mapper)
            {
                _ud = ud;
                _mapper = mapper;
            }

            public async Task<Model> Handle(Query request, CancellationToken cancellationToken)
            {
                var farmManures = _ud.GetFarmManures();
                var managedManure = _ud.GetAllManagedManures().Where(mm => !mm.AssignedWithNutrientAnalysis).ToList();

                var model = new Model
                {
                    ManureAnalytics = _mapper.Map<List<Model.ManureNutrientAnalysis>>(farmManures),
                    RanchManures = _mapper.Map<List<Model.RanchManure>>(managedManure)
                };

                return await Task.FromResult(model);
            }
        }
    }
}
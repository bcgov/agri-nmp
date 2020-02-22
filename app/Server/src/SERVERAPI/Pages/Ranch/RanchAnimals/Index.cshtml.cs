using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Agri.Models;
using Agri.Models.Farm;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SERVERAPI.Filters;
using SERVERAPI.Models.Impl;
using Agri.Data;

namespace SERVERAPI.Pages.Ranch.RanchAnimals
{
    public class Index : BasePageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public Model Data { get; private set; }

        public Index(IMediator mediator) => _mediator = mediator;

        public async Task<IActionResult> OnGetAsync(Query query)
        {
            var data = await _mediator.Send(query);

            //If no animals exist redirect to the CreateEdit PAGE
            if (!data.Animals.Any())
            {
                return RedirectToPage("CreateEdit", "Create", new { ismodal = false });
            }

            Data = data;

            return Page();
        }

        public class Query : IRequest<Model>
        {
        }

        public class Model
        {
            public List<FarmAnimal> Animals { get; set; }
            public string RanchAnimalGroupsMessage { get; set; }

            public class FarmAnimal
            {
                public int Id { get; set; }
                public string AnimalName { get; set; }
                public string AnimalSubTypeName { get; set; }
                public ManureMaterialType ManureMaterialType { get; set; }
                public string AverageAnimalNumber { get; set; }
                public bool IsManureCollected { get; set; }
                public string ManureCollected => IsManureCollected ? "Yes" : "No";
                public int DurationDays { get; set; }
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<FarmAnimal, Model.FarmAnimal>();
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

            public Task<Model> Handle(Query request, CancellationToken cancellationToken)
            {
                var animals = _ud.GetAnimals();
                var message = _db.UserPrompts
                    .Single(p => p.UserPromptPage == UserPromptPage.NutrientsAnalysisList.ToString() &&
                                p.UserJourney == UserJourney.Ranch.ToString()).Text;

                return Task.FromResult(new Model
                {
                    Animals = _mapper.Map<List<FarmAnimal>, List<Model.FarmAnimal>>(animals),
                    RanchAnimalGroupsMessage = message
                });
            }
        }
    }
}
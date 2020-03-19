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
using SERVERAPI.Models.Impl;

namespace SERVERAPI.Pages.Poultry.PoultryManure
{
    public class Index : BasePageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public Model Data { get; private set; }

        public Index(IMediator mediator) => _mediator = mediator;

        public async Task<IActionResult> OnGetAsync(Query query)
        {
            Data = await _mediator.Send(query);
            return Page();
        }

        public class Query : IRequest<Model>
        {
        }

        public class Model
        {
            public List<FarmAnimal> FarmAnimals { get; set; }

            public List<ImportedManure> ImportedManures { get; set; }

            public class FarmAnimal
            {
                public int Id { get; set; }
                public int AnimalId { get; set; }
                public string AnimalName { get; set; }
                public string AnimalSubTypeName { get; set; }
                public int AnimalSubTypeId { get; set; }
                public ManureMaterialType ManureMaterialType { get; set; }
                public string AverageAnimalNumber { get; set; }
                public bool IsManureCollected { get; set; }
                public string ManureCollected => IsManureCollected ? "Yes" : "No";
                public int DurationDays { get; set; }
                public int? ManureGeneratedTonsPerYear { get; set; }
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

            public Handler(UserData ud, IMapper mapper)
            {
                _ud = ud;
                _mapper = mapper;
            }

            public async Task<Model> Handle(Query request, CancellationToken cancellationToken)
            {
                var model = new Model
                {
                    FarmAnimals = _mapper.Map<List<FarmAnimal>, List<Model.FarmAnimal>>(_ud.GetAnimals()),
                    ImportedManures = _ud.GetImportedManures()
                };

                return await Task.FromResult(model);
            }
        }
    }
}
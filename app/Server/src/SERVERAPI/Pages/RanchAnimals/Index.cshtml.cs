﻿using System;
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

namespace SERVERAPI.Pages.RanchAnimals
{
    public class Index : PageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public Model Data { get; private set; }

        public Index(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync(Query query)
            => Data = await _mediator.Send(query);

        public class Query : IRequest<Model>
        {
        }

        public class Model
        {
            public List<FarmAnimal> Animals { get; set; }

            public class FarmAnimal
            {
                public int Id { get; set; }
                public string AnimalTypeName { get; set; }
                public string SelectSubTypeOption { get; set; }
                public string SubTypeName { get; set; }
                public ManureMaterialType ManureMaterialType { get; set; }
                public string AverageAnimalNumber { get; set; }
                public string ManureCollected { get; set; }
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

            public Handler(UserData ud, IMapper mapper)
            {
                _ud = ud;
                _mapper = mapper;
            }

            public Task<Model> Handle(Query request, CancellationToken cancellationToken)
            {
                var animals = _ud.GetAnimals();

                return Task.FromResult(new Model
                {
                    Animals = _mapper.Map<List<FarmAnimal>, List<Model.FarmAnimal>>(animals)
                });
            }
        }
    }
}
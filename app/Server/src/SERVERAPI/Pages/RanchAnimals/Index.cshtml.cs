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
using SERVERAPI.Filters;
using SERVERAPI.Models.Impl;

namespace SERVERAPI.Pages.RanchAnimals
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

            public class FarmAnimal
            {
                public int Id { get; set; }
                public string AnimalName { get; set; }
                public string SubTypeName { get; set; }
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
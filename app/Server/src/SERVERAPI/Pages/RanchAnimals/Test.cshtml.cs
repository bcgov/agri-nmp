using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Agri.Data;
using Agri.Models;
using Agri.Models.Farm;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SERVERAPI.Models.Impl;
using ModelConfig = Agri.Models.Configuration;

namespace SERVERAPI.Pages.RanchAnimals
{
    public class TestCreateEdit : PageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public Command Data { get; set; }

        public TestCreateEdit(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync(Query query) => Data = await _mediator.Send(query);

        public class Query : IRequest<Command>
        {
            public int? Id { get; set; }
        }

        public class Command : IRequest<int>
        {
            public int? Id { get; set; }

            public string SubTypeOption { get; set; }

            public string SubTypeName { get; set; }

            public List<SelectListItem> SubTypeOptions { get; set; }

            public ManureMaterialType SelectManureMaterialTypeOption { get; set; }

            public string AverageAnimalNumber { get; set; }

            public string ButtonPressed { get; set; }

            public string Placehldr { get; set; }
            public string Target { get; set; }

            public bool IsManureCollected { get; set; }
            public string ManureCollected { get; set; }
            public int DurationDays { get; set; }
            public bool ShowDurationDays { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<FarmAnimal, Command>();
            }
        }

        public class Handler : IRequestHandler<Query, Command>
        {
            private readonly UserData _ud;
            private readonly IMapper _mapper;
            private readonly IAgriConfigurationRepository _sd;

            public Handler(UserData ud, IMapper mapper, IAgriConfigurationRepository sd)
            {
                _ud = ud;
                _mapper = mapper;
                _sd = sd;
            }

            public Task<Command> Handle(Query request, CancellationToken cancellationToken)
            {
                var command = new Command();
                if (request.Id.HasValue)
                {
                    command = _mapper.Map<FarmAnimal, Command>(_ud.GetAnimalDetail(request.Id.Value));
                }

                var cattleAnimalType = 1;
                command.SelectManureMaterialTypeOption = ManureMaterialType.Solid;

                command.SubTypeOptions = _sd.GetSubtypesDll(cattleAnimalType)
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Value })
                    .ToList();

                //if (command.SubTypeOptions.Count() > 1)
                //{
                //    command.SubTypeOptions.Insert(0, new ModelConfig.SelectListItem() { Id = 0, Value = "select type" });
                //}

                if (command.SubTypeOptions.Count() == 1)
                {
                    command.SubTypeOption = command.SubTypeOptions[0].Value.ToString();
                }
                //command.SubTypeOptions = new SelectList(subTypeOptions, "Id", "Value");

                return Task.FromResult(command);
            }
        }
    }
}
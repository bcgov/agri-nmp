using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Agri.Data;
using Agri.Models;
using Agri.Models.Configuration;
using Agri.Models.Farm;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SERVERAPI.Models.Impl;

namespace SERVERAPI.Pages.RanchAnimals
{
    public class CreateEdit : PageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public Command Data { get; private set; }

        public CreateEdit(IMediator mediator) => _mediator = mediator;

        public async Task OnGetCreateAsync() => Data = await _mediator.Send(new Query());

        public class Query : IRequest<Command>
        {
            public int? Id { get; set; }
        }

        public class Command
        {
            public int? Id { get; set; }
            public string Act { get; set; }
            public string Actn { get; set; }
            public string Cntl { get; set; }
            public string Title { get; set; }
            public string BtnText { get; set; }

            //[Required(ErrorMessage = "Required")]
            //[Range(1, 9999, ErrorMessage = "Required")]
            public string SelectAnimalTypeOption { get; set; }

            public List<SelectListItem> AnimalTypeOptions { get; set; } = new List<SelectListItem>();

            [Required(ErrorMessage = "Required")]
            [Range(1, 9999, ErrorMessage = "Required")]
            public string SubTypeOption { get; set; }

            public string SubTypeName { get; set; }
            public List<SelectListItem> SubTypeOptions { get; set; } = new List<SelectListItem>();

            public ManureMaterialType SelectManureMaterialTypeOption { get; set; }

            [Required(ErrorMessage = "Required")]
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

                command.AnimalTypeOptions = _sd.GetAnimalTypesDll().ToList();
                command.SelectAnimalTypeOption = "1";
                command.SelectManureMaterialTypeOption = ManureMaterialType.Solid;
                if (!string.IsNullOrEmpty(command.SelectAnimalTypeOption) &&
                        command.SelectAnimalTypeOption != "select animal")
                {
                    command.SubTypeOptions = _sd.GetSubtypesDll(Convert.ToInt32(command.SelectAnimalTypeOption)).ToList();
                    if (command.SubTypeOptions.Count() > 1)
                    {
                        command.SubTypeOptions.Insert(0, new SelectListItem() { Id = 0, Value = "select type" });
                    }

                    if (command.SubTypeOptions.Count() == 1)
                    {
                        command.SubTypeOption = command.SubTypeOptions[0].Id.ToString();
                    }
                }

                return Task.FromResult(command);
            }
        }
    }
}
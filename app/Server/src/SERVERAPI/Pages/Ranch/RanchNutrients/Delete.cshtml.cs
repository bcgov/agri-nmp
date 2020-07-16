using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Agri.Data;
using Agri.Models.Farm;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SERVERAPI.Models.Impl;

namespace SERVERAPI.Pages.Ranch.RanchNutrients
{
    public class Delete : PageModel
    {
        private readonly IMediator _mediator;

        public Delete(IMediator mediator) => _mediator = mediator;

        [BindProperty]
        public Command Data { get; set; }

        public async Task OnGetAsync(Query query)
            => Data = await _mediator.Send(query);

        public async Task<ActionResult> OnPostAsync()
        {
            await _mediator.Send(Data);

            return this.RedirectToPageJson(nameof(Index));
        }

        public class Query : IRequest<Command>
        {
            public int? Id { get; set; }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(m => m.Id).NotNull();
            }
        }

        public class Command : IRequest<Unit>
        {
            public int Id { get; set; }

            [Display(Name = "Compost/Manure")]
            public string ManureName { get; set; }

            public string Warning { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<FarmManure, Command>()
                    .ForMember(m => m.ManureName, opts => opts.MapFrom(s => s.Name));
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

            public async Task<Command> Handle(Query request, CancellationToken cancellationToken)
            {
                var nutrientManure = _ud.GetFarmManure(request.Id.Value);
                var command = _mapper.Map<Command>(nutrientManure);

                // determine if the selected manure is currently being used on any of the fields
                List<Field> flds = _ud.GetFields();

                foreach (var fld in flds)
                {
                    List<NutrientManure> mans = _ud.GetFieldNutrientsManures(fld.FieldName);

                    foreach (var man in mans)
                    {
                        if (request.Id.Value.ToString() == man.manureId)
                        {
                            command.Warning = _sd.GetUserPrompt("manuredeletewarning");
                        }
                    }
                }

                return await Task.FromResult(command);
            }

            public class CommandHandler : IRequestHandler<Command, Unit>
            {
                private readonly UserData _ud;

                public CommandHandler(UserData ud)
                {
                    _ud = ud;
                }

                public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
                {
                    _ud.DeleteFarmManure(request.Id);
                    _ud.UpdateManagedFarmAnimalsAllocationToNutrientAnalysis();
                    _ud.UpdateManagedImportedManuresAllocationToNutrientAnalysis();

                    return await Task.FromResult(new Unit());
                }
            }
        }
    }
}
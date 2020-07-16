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

namespace SERVERAPI.Pages.Mixed.MixedManure
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
            public int ImportedManureId { get; set; }
            public string manureId { get; set; }
            public string ImportManureName { get; set; }
            public string Target { get; set; }
            public bool AppliedToAField { get; set; }
            public string DeleteWarningForUnstorableMaterial { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ImportedManure, Command>()
                    .ForMember(m => m.ImportedManureId, opts => opts.MapFrom(src => src.Id))
                    .ForMember(m => m.ImportManureName, opts => opts.MapFrom(src => src.ManagedManureName));
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
                var manure = _ud.GetImportedManure(request.Id.Value);
                var command = _mapper.Map<Command>(manure);

                if (_ud.GetYearData().GetFieldsAppliedWithManure(manure).Any())
                {
                    command.AppliedToAField = true;
                    command.DeleteWarningForUnstorableMaterial = _sd.GetUserPrompt("ImportMaterialNotStoredDeleteWarning");
                }

                return await Task.FromResult(command);
            }
        }

        public class CommandHandler : IRequestHandler<Command, Unit>
        {
            private readonly UserData _ud;

            public CommandHandler(UserData ud)
            {
                _ud = ud;
            }

            public async Task<Unit> Handle(Command message, CancellationToken cancellationToken)
            {
                _ud.DeleteImportedManure(message.ImportedManureId);

                return await Task.FromResult(new Unit());
            }
        }
    }
}
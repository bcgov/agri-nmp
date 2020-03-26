using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Agri.Models.Farm;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SERVERAPI.Models.Impl;

namespace SERVERAPI.Pages.Mixed.MixedAnimals
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

        public class Command : IRequest
        {
            public int? Id { get; set; }
            public int CattleSubTypeId { get; set; }
            public string CattleSubTypeName { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<FarmAnimal, Command>()
                    .ForMember(m => m.CattleSubTypeId, opts => opts.MapFrom(src => src.AnimalSubTypeId))
                    .ForMember(m => m.CattleSubTypeName, opts => opts.MapFrom(src => src.AnimalSubTypeName))
                    .ReverseMap();
            }
        }

        public class Handler : IRequestHandler<Query, Command>
        {
            private readonly UserData _ud;
            private readonly IMapper _mapper;

            public Handler(UserData ud, IMapper mapper)
            {
                _ud = ud;
                _mapper = mapper;
            }

            public async Task<Command> Handle(Query request, CancellationToken cancellationToken)
            {
                var command = new Command();
                if (request.Id.HasValue)
                {
                    command = _mapper.Map<FarmAnimal, Command>(_ud.GetAnimalDetail(request.Id.Value));
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
                _ud.DeleteAnimal(message.Id.Value);

                return await Task.FromResult(new Unit());
            }
        }
    }
}
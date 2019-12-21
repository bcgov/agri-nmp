using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Agri.Data;
using Agri.Models;
using Agri.Models.Farm;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SERVERAPI.Models.Impl;

namespace SERVERAPI.Pages.RanchAnimals
{
    public class CreateEdit : PageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public Command Data { get; set; }

        public CreateEdit(IMediator mediator) => _mediator = mediator;

        public async Task OnGetCreateAsync() => await PopulateData();

        private async Task PopulateData()
        {
            Data = await _mediator.Send(new Query());
            Data = await _mediator.Send(new LookupDataQuery { PopulatedData = Data });
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(Data);

                return this.RedirectToPageJson(nameof(Index));
            }
            Data = await _mediator.Send(new LookupDataQuery { PopulatedData = Data });
            return Page();
        }

        public class Query : IRequest<Command>
        {
            public int? Id { get; set; }
        }

        public class LookupDataQuery : IRequest<Command>
        {
            public Command PopulatedData { get; set; }
        }

        [BindProperties]
        public class Command : IRequest<int>
        {
            public int? Id { get; set; }

            public int CattleSubType { get; set; }

            public string CattleSubTypeName { get; set; }

            public SelectList CattleSubTypeOptions { get; set; }

            public ManureMaterialType ManureMaterialType => ManureMaterialType.Solid;

            public int AverageAnimalNumber { get; set; }

            public string ButtonPressed { get; set; }

            public string Placehldr { get; set; }
            public bool IsManureCollected { get; set; }
            public int DurationDays { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(m => m.CattleSubType).GreaterThan(0).WithMessage("Cattle Type must be selected");
                RuleFor(m => m.AverageAnimalNumber).NotNull().NotEmpty().GreaterThan(0);
                When(m => m.IsManureCollected, () =>
                {
                    RuleFor(m => m.DurationDays).NotNull().NotEmpty().GreaterThan(0)
                        .WithMessage("Duration must be greater than 0");
                });
            }
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

            public Handler(UserData ud, IMapper mapper)
            {
                _ud = ud;
                _mapper = mapper;
            }

            public Task<Command> Handle(Query request, CancellationToken cancellationToken)
            {
                var command = new Command();
                if (request.Id.HasValue)
                {
                    command = _mapper.Map<FarmAnimal, Command>(_ud.GetAnimalDetail(request.Id.Value));
                }

                return Task.FromResult(command);
            }
        }

        public class LookupDataHandler : IRequestHandler<LookupDataQuery, Command>
        {
            private readonly IAgriConfigurationRepository _sd;

            public LookupDataHandler(IAgriConfigurationRepository sd)
            {
                _sd = sd;
            }

            public Task<Command> Handle(LookupDataQuery request, CancellationToken cancellationToken)
            {
                var command = request.PopulatedData;

                var cattleAnimalType = 1;

                var subTypeOptions = _sd.GetSubtypesDll(cattleAnimalType).ToList();

                if (subTypeOptions.Count() == 1)
                {
                    command.CattleSubType = subTypeOptions[0].Id;
                }
                command.CattleSubTypeOptions = new SelectList(subTypeOptions, "Id", "Value");

                return Task.FromResult(command);
            }
        }
    }
}
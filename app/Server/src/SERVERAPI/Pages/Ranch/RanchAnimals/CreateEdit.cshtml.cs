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

namespace SERVERAPI.Pages.Ranch.RanchAnimals
{
    public class CreateEdit : BasePageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public Command Data { get; set; }

        [BindProperty]
        public bool IsModal { get; set; }

        public string PageLayout => IsModal ? null : PageConstants.PageLayout;

        public CreateEdit(IMediator mediator) => _mediator = mediator;

        public async Task OnGetCreateAsync(bool ismodal)
        {
            IsModal = ismodal;
            Title = "Add Animal";
            await PopulateData(new Query());
        }

        public async Task OnGetEditAsync(bool ismodal, Query query)
        {
            IsModal = ismodal;
            Title = "Edit Animal";
            await PopulateData(query);
        }

        private async Task PopulateData(Query query)
        {
            Data = await _mediator.Send(query);
            Data = await _mediator.Send(new LookupDataQuery { PopulatedData = Data });
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            return await ProcessPost();
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            return await ProcessPost();
        }

        private async Task<IActionResult> ProcessPost()
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(Data);

                if (IsModal)
                {
                    return this.RedirectToPageJson(nameof(Index));
                }
                return RedirectToPage(nameof(Index));
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
        public class Command : IRequest<Unit>
        {
            public int? Id { get; set; }
            public int AnimalId { get; set; }
            public string AnimalName { get; set; }
            public int CattleSubTypeId { get; set; }
            public string CattleSubTypeName { get; set; }
            public SelectList CattleSubTypeOptions { get; set; }
            public ManureMaterialType ManureMaterialType => ManureMaterialType.Solid;
            public int AverageAnimalNumber { get; set; }
            public string Placehldr { get; set; }
            public bool IsManureCollected { get; set; }
            public int DurationDays { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(m => m.CattleSubTypeId).GreaterThan(0).WithMessage("Cattle Type must be selected");
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

        public class LookupDataHandler : IRequestHandler<LookupDataQuery, Command>
        {
            private readonly IAgriConfigurationRepository _sd;

            public LookupDataHandler(IAgriConfigurationRepository sd)
            {
                _sd = sd;
            }

            public async Task<Command> Handle(LookupDataQuery request, CancellationToken cancellationToken)
            {
                var command = request.PopulatedData;

                var beefCattle = _sd.GetAnimal(1);
                command.AnimalId = beefCattle.Id;
                command.AnimalName = beefCattle.Name;

                var subTypeOptions = _sd.GetSubtypesDll(beefCattle.Id).ToList();

                if (subTypeOptions.Count() == 1)
                {
                    command.CattleSubTypeId = subTypeOptions[0].Id;
                }
                command.CattleSubTypeOptions = new SelectList(subTypeOptions, "Id", "Value");

                return await Task.FromResult(command);
            }
        }

        public class CommandHandler : IRequestHandler<Command, Unit>
        {
            private readonly UserData _ud;
            private readonly IMapper _mapper;

            public CommandHandler(UserData ud, IMapper mapper)
            {
                _ud = ud;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(Command message, CancellationToken cancellationToken)
            {
                var farmAnimal = _mapper.Map<Command, FarmAnimal>(message);

                if (farmAnimal.Id.GetValueOrDefault(0) == 0)
                {
                    _ud.AddAnimal(farmAnimal);
                }
                else
                {
                    _ud.UpdateAnimal(farmAnimal);
                }

                return await Task.FromResult(new Unit());
            }
        }
    }
}
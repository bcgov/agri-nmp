using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Agri.CalculateService;
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

namespace SERVERAPI.Pages.Poultry.PoultryAnimals
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
            if (!Data.ShowMaterialType)
            {
                Data.ManureType = ManureMaterialType.Solid;
            }
            if (Data.PostedElementEvent == ElementEvent.AnimalSubTypeChanged)
            {
                Data.PostedElementEvent = ElementEvent.None;
                ModelState.Clear();
            }
            else
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
            public string AnimalTypeName { get; set; }
            public SelectList AnimalTypeOptions { get; set; }
            public int AnimalSubTypeId { get; set; }
            public string AnimalSubTypeName { get; set; }
            public SelectList AnimalSubTypeOptions { get; set; }
            public string ManureTypeName { get; set; }
            public SelectList ManureMaterialTypeOptions { get; set; }
            public ManureMaterialType ManureType { get; set; }
            public int? BirdsPerFlock { get; set; }
            public decimal? FlocksPerYear { get; set; }
            public int? DaysPerFlock { get; set; }
            public bool ShowMaterialType => AnimalSubTypeId == 15;   //Only show option for Layers
            public ElementEvent PostedElementEvent { get; set; }
        }

        public enum ElementEvent
        {
            None,
            AnimalSubTypeChanged
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                //RuleFor(m => m.AnimalId).GreaterThan(0).WithMessage("Animal Type must be selected");
                RuleFor(m => m.AnimalSubTypeId).GreaterThan(0).WithMessage("Animal Sub Type must be selected");
                RuleFor(m => m.ManureType).Must(m => m > 0)
                    .When(m => m.ShowMaterialType)
                    .WithMessage("Manure Material Type must be selected");
                RuleFor(m => m.BirdsPerFlock).NotEmpty().GreaterThan(0);
                RuleFor(m => m.FlocksPerYear).NotEmpty().GreaterThan(0);
                RuleFor(m => m.DaysPerFlock).NotEmpty().GreaterThan(0);
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<FarmAnimal, Command>()

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

                var animalOptions = _sd.GetAnimalTypesDll();
                command.AnimalTypeOptions = new SelectList(animalOptions, "Id", "Value");

                SelectList subTypeOptions;
                if (command.AnimalId == 0)
                {
                    var poultryId = _sd.GetAnimal(6);
                    command.AnimalId = poultryId.Id;
                    command.AnimalName = poultryId.Name;
                    command.ManureType = ManureMaterialType.Solid;
                    subTypeOptions = new SelectList(_sd.GetSubtypesDll(poultryId.Id).ToList(), "Id", "Value");
                }
                else
                {
                    subTypeOptions = new SelectList(_sd.GetSubtypesDll(command.AnimalId), "Id", "Value");
                }

                command.AnimalSubTypeOptions = subTypeOptions;

                return await Task.FromResult(command);
            }
        }

        public class CommandHandler : IRequestHandler<Command, Unit>
        {
            private readonly IAgriConfigurationRepository _sd;
            private readonly UserData _ud;
            private readonly IMapper _mapper;
            private readonly ICalculateManureGeneration _calculateManureGeneration;

            public CommandHandler(UserData ud,
                IMapper mapper,
                ICalculateManureGeneration calculateManureGeneration,
                IAgriConfigurationRepository sd)
            {
                _sd = sd;
                _ud = ud;
                _mapper = mapper;
                _calculateManureGeneration = calculateManureGeneration;
            }

            public async Task<Unit> Handle(Command message, CancellationToken cancellationToken)
            {
                var farmAnimal = _mapper.Map<Command, FarmAnimal>(message);

                farmAnimal.IsManureCollected = true;
                farmAnimal.IsPoultry = true;

                if (farmAnimal.ManureType == ManureMaterialType.Solid)
                {
                    farmAnimal.ManureGeneratedTonsPerYear = _calculateManureGeneration
                        .GetTonsGeneratedForPoultrySubType(farmAnimal.AnimalSubTypeId,
                            farmAnimal.BirdsPerFlock.Value, farmAnimal.FlocksPerYear.Value, farmAnimal.DaysPerFlock.Value);
                }
                else
                {
                    farmAnimal.ManureGeneratedGallonsPerYear = _calculateManureGeneration
                        .GetGallonsGeneratedForPoultrySubType(farmAnimal.AnimalSubTypeId,
                            farmAnimal.BirdsPerFlock.Value, farmAnimal.FlocksPerYear.Value, farmAnimal.DaysPerFlock.Value);
                }

                if (farmAnimal.AnimalSubTypeId > 0 && string.IsNullOrEmpty(farmAnimal.AnimalSubTypeName))
                {
                    farmAnimal.AnimalSubTypeName = _sd.GetAnimalSubType(farmAnimal.AnimalSubTypeId).Name;
                }

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
using System;
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
using Microsoft.AspNetCore.Mvc.Rendering;
using SERVERAPI.Models.Impl;

namespace SERVERAPI.Pages.Mixed.MixedAnimals
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

            if (Data.PostedElementEvent == ElementEvent.AnimalTypeChanged)
            {
                Data.PostedElementEvent = ElementEvent.None;
                ModelState.Clear();

                Data.AnimalSubTypeId = 0;
                Data.ShowAnimalNumbers = false;
                Data.ShowFlockFields = false;
                Data.ShowMaterialType = false;
            }
            else if (Data.PostedElementEvent == ElementEvent.AnimalSubTypeChanged)
            {
                Data.PostedElementEvent = ElementEvent.None;
                ModelState.Clear();

                Data.ShowAnimalNumbers = false;
                Data.ShowFlockFields = false;
                Data.ShowMaterialType = false;
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
            public int? AverageAnimalNumber { get; set; }
            public int? BirdsPerFlock { get; set; }
            public decimal? FlocksPerYear { get; set; }
            public int? DaysPerFlock { get; set; }
            public bool IsManureCollected { get; set; }
            public int? DurationDays { get; set; }
            public bool ShowSubType { get; set; }
            public bool ShowAnimalNumbers { get; set; }
            public bool ShowMaterialType { get; set; }
            public bool ShowFlockFields { get; set; }
            public string Placehldr { get; set; }
            public ElementEvent PostedElementEvent { get; set; }
        }

        public enum ElementEvent
        {
            None,
            AnimalTypeChanged,
            AnimalSubTypeChanged
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(m => m.AnimalId).GreaterThan(0).WithMessage("Animal Type must be selected");
                RuleFor(m => m.AnimalSubTypeId)
                    .GreaterThan(0)
                    .When(m => m.ShowSubType)
                    .WithMessage("Animal Sub Type must be selected");
                RuleFor(m => m.ManureType).Must(m => m > 0)
                    .When(m => m.ShowMaterialType)
                    .WithMessage("Manure Material Type must be selected");
                When(m => m.ShowFlockFields, () =>
                    {
                        RuleFor(m => m.BirdsPerFlock).NotEmpty().GreaterThan(0);
                        RuleFor(m => m.FlocksPerYear).NotEmpty().GreaterThan(0);
                        RuleFor(m => m.DaysPerFlock).NotEmpty().GreaterThan(0);
                    });
                When(m => m.ShowAnimalNumbers, () =>
                {
                    RuleFor(m => m.AverageAnimalNumber).NotEmpty().GreaterThan(0);
                    When(m => m.IsManureCollected, () =>
                    {
                        RuleFor(m => m.DurationDays).NotEmpty().WithMessage("Duration must be greater than 0")
                            .GreaterThan(0)
                            .WithMessage("Duration must be greater than 0");
                    });
                });
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
                    var animal = _ud.GetAnimalDetail(request.Id.Value);
                    command = _mapper.Map<FarmAnimal, Command>(animal);
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

                var animalOptions = _sd.GetAnimalTypesDll().Where(a => a.Id != 2);
                command.AnimalTypeOptions = new SelectList(animalOptions, "Id", "Value");

                if (command.AnimalId > 0)
                {
                    var subTypeOptions = new SelectList(_sd.GetSubtypesDll(command.AnimalId), "Id", "Value");

                    command.ShowSubType = true;
                    command.AnimalSubTypeOptions = subTypeOptions;
                    command.ShowFlockFields = command.AnimalId == 3 || command.AnimalId == 6;
                    command.ShowAnimalNumbers = command.AnimalId != 3 && command.AnimalId != 6;

                    if (subTypeOptions.Count() == 1)
                    {
                        command.AnimalSubTypeId = Convert.ToInt32(subTypeOptions.First().Value);
                        command.AnimalSubTypeName = subTypeOptions.First().Text;
                        command.ShowSubType = false;
                    }
                }

                if (command.ShowFlockFields)
                {
                    command.IsManureCollected = false;
                    command.DurationDays = null;
                }
                else
                {
                    command.BirdsPerFlock = null;
                    command.FlocksPerYear = null;
                    command.DaysPerFlock = null;
                }

                if (command.AnimalSubTypeId > 0)
                {
                    var subType = _sd.GetAnimalSubType(command.AnimalSubTypeId);

                    command.ShowMaterialType = subType.LiquidPerGalPerAnimalPerDay.GetValueOrDefault(0) > 0;
                    if (!command.ShowMaterialType)
                    {
                        command.ManureType = ManureMaterialType.Solid;
                    }
                }

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

                if (farmAnimal.AnimalId == 3 || farmAnimal.AnimalId == 6)
                {
                    farmAnimal.IsManureCollected = true;
                    farmAnimal.IsPoultry = true;
                }

                if (farmAnimal.ManureType == ManureMaterialType.Solid)
                {
                    if (farmAnimal.IsPoultry)
                    {
                        farmAnimal.ManureGeneratedTonsPerYear = _calculateManureGeneration
                            .GetTonsGeneratedForPoultrySubType(farmAnimal.AnimalSubTypeId,
                                farmAnimal.BirdsPerFlock.Value, farmAnimal.FlocksPerYear.Value, farmAnimal.DaysPerFlock.Value);
                    }
                    else
                    {
                        farmAnimal.ManureGeneratedTonsPerYear = _calculateManureGeneration
                            .GetSolidTonsGeneratedForAnimalSubType(farmAnimal.AnimalSubTypeId, farmAnimal.AverageAnimalNumber, farmAnimal.DurationDays);
                    }
                }
                else
                {
                    if (farmAnimal.IsPoultry)
                    {
                        farmAnimal.ManureGeneratedGallonsPerYear = _calculateManureGeneration
                        .GetGallonsGeneratedForPoultrySubType(farmAnimal.AnimalSubTypeId,
                            farmAnimal.BirdsPerFlock.Value, farmAnimal.FlocksPerYear.Value, farmAnimal.DaysPerFlock.Value);
                    }
                    else
                    {
                        farmAnimal.ManureGeneratedTonsPerYear = _calculateManureGeneration
                            .GetGallonsGeneratedForAnimalSubType(farmAnimal.AnimalSubTypeId, farmAnimal.AverageAnimalNumber, farmAnimal.DurationDays);
                    }
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
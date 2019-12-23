using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Agri.Data;
using Agri.Models.Configuration;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SERVERAPI.Models.Impl;

namespace SERVERAPI.Pages.MiniApps
{
    public class SoilTestConvertorModel : PageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public Command Data { get; set; }

        public SoilTestConvertorModel(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync()
        {
            await PopulateData();
        }

        private async Task PopulateData()
        {
            Data = await _mediator.Send(new Query());
            Data = await _mediator.Send(new LookupDataQuery { PopulatedData = Data });
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                Data = await _mediator.Send(new LookupDataQuery { PopulatedData = Data });
            }
            return Page();
        }

        public class Query : IRequest<Command>
        {
            public string laboratory { get; set; }
        }

        public class LookupDataQuery : IRequest<Command>
        {
            public Command PopulatedData { get; set; }
        }

        [BindProperties]
        public class Command : IRequest<MediatR.Unit>
        {
            public decimal pH { get; set; }
            public decimal phosphorous { get; set; }
            public decimal kelowna { get; set; }
            public List<SelectListItem> laboratoryOptions { get; set; }
            public string selLaboratoryOption { get; set; }
            public string SoilTestConverterUserInstruction1 { get; set; }
            public string SoilTestConverterUserInstruction2 { get; set; }
            public string SoilTestingInformation { get; set; }
            public string BCNutrientManagementCalculator { get; set; }
            public string SoilTestInformationButtonLink { get; set; }
            public string BCNutrientManagementCalculatorButtonLink { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(m => m.selLaboratoryOption).NotNull().NotEmpty().WithMessage("Laboratory must be selected");
                RuleFor(m => m.pH).NotEqual(0).WithMessage("PH Field is required");
                RuleFor(m => m.phosphorous).NotEqual(0).WithMessage("PhosPhorous Field is required");
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                //CreateMap<FarmAnimal, Command>()
                //    .ForMember(m => m.CattleSubTypeId, opts => opts.MapFrom(src => src.SubTypeId))
                //    .ForMember(m => m.CattleSubTypeName, opts => opts.MapFrom(src => src.SubTypeName))
                //    .ReverseMap();
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
                //if (request.Id.HasValue)
                //{
                //    command = _mapper.Map<FarmAnimal, Command>(_ud.GetAnimalDetail(request.Id.Value));
                //}

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

                //var beefCattle = _sd.GetAnimal(1);
                //command.AnimalId = beefCattle.Id;
                //command.AnimalName = beefCattle.Name;

                //var subTypeOptions = _sd.GetSubtypesDll(beefCattle.Id).ToList();

                //if (subTypeOptions.Count() == 1)
                //{
                //    command.CattleSubTypeId = subTypeOptions[0].Id;
                //}
                //command.CattleSubTypeOptions = new SelectList(subTypeOptions, "Id", "Value");
                command.laboratoryOptions = _sd.GetSoilTestMethodsDll().ToList();
                var details = _sd.GetSoilConvertorDetails();
                command.SoilTestConverterUserInstruction1 = details.Where(x => x.Key == "SoilTestConverterUserInstruction1").Select(x => x.Value).FirstOrDefault();
                command.SoilTestConverterUserInstruction2 = details.Where(x => x.Key == "SoilTestConverterUserInstruction2").Select(x => x.Value).FirstOrDefault();
                command.SoilTestingInformation = details.Where(x => x.Key == "SoilTestingInformation").Select(x => x.Value).FirstOrDefault();
                command.BCNutrientManagementCalculator = details.Where(x => x.Key == "BCNutrientManagementCalculator").Select(x => x.Value).FirstOrDefault();
                command.SoilTestInformationButtonLink = details.Where(x => x.Key == "SoilTestInformationButtonLink").Select(x => x.Value).FirstOrDefault();
                command.BCNutrientManagementCalculatorButtonLink = details.Where(x => x.Key == "BCNutrientManagementCalculatorButtonLink").Select(x => x.Value).FirstOrDefault();

                return await Task.FromResult(command);
            }
        }

        //public class CommandHandler : IRequestHandler<Command, MediatR.Unit>
        //{
        //    private readonly UserData _ud;
        //    private readonly IMapper _mapper;

        //    public CommandHandler(UserData ud, IMapper mapper)
        //    {
        //        _ud = ud;
        //        _mapper = mapper;
        //    }

        //    public async Task<Unit> Handle(Command message, CancellationToken cancellationToken)
        //    {
        //        var farmAnimal = _mapper.Map<Command, FarmAnimal>(message);
        //        _ud.AddAnimal(farmAnimal);

        //        return default;
        //    }
        //}
    }
}
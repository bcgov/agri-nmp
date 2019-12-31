using Agri.CalculateService;
using Agri.Data;
using Agri.Models.Configuration;
using Agri.Models.Farm;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SERVERAPI.Pages.MiniApps
{
    public class SoilTestConvertor : PageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public ConverterQuery Data { get; set; }

        [BindProperty]
        public ResultModel Result { get; set; }

        public SoilTestConvertor(IMediator mediator) => _mediator = mediator;

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
                //Send the Data to Receive conversion result
                Result = await _mediator.Send(Data);
            }
            Data = await _mediator.Send(new LookupDataQuery { PopulatedData = Data });
            return Page();
        }

        public class Query : IRequest<ConverterQuery>
        {
            public string laboratory { get; set; }
        }

        public class LookupDataQuery : IRequest<ConverterQuery>
        {
            public ConverterQuery PopulatedData { get; set; }
        }

        public class ConverterQuery : IRequest<ResultModel>
        {
            public decimal pH { get; set; }
            public decimal phosphorous { get; set; }
            public int kelowna { get; set; }
            public bool isShowKelowna { get; set; }
            public List<SelectListItem> laboratoryOptions { get; set; }
            public string selLaboratoryOption { get; set; }
            public string SoilTestConverterUserInstruction1 { get; set; }
            public string SoilTestConverterUserInstruction2 { get; set; }
            public string SoilTestingInformation { get; set; }
            public string BCNutrientManagementCalculator { get; set; }
            public string SoilTestInformationButtonLink { get; set; }
            public string BCNutrientManagementCalculatorButtonLink { get; set; }
        }

        public class ResultModel
        {
            public string ExampleResultField { get; set; }
        }

        public class ModelValidator : AbstractValidator<ConverterQuery>
        {
            public ModelValidator()
            {
                RuleFor(m => m.selLaboratoryOption).NotNull().Must(m => !m.Equals("0"))
                    .WithMessage("Laboratory must be selected");
                RuleFor(m => m.pH).GreaterThan(0).WithMessage("PH Field is required");
                RuleFor(m => m.phosphorous).GreaterThan(0).WithMessage("PhosPhorous Field is required");
            }
        }

        public class Handler : IRequestHandler<LookupDataQuery, ConverterQuery>,
            IRequestHandler<ConverterQuery, ResultModel>
        {
            private readonly IAgriConfigurationRepository _sd;
            private readonly ISoilTestConverter _soilTestConversions;

            public Handler(IAgriConfigurationRepository sd, ISoilTestConverter soilTestConversions)
            {
                _sd = sd;
                _soilTestConversions = soilTestConversions;
            }

            public async Task<ConverterQuery> Handle(LookupDataQuery request, CancellationToken cancellationToken)
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
                if (!string.IsNullOrEmpty(command.selLaboratoryOption) && !command.selLaboratoryOption.Equals("0"))
                {
                    var soilTest = new SoilTest();
                    soilTest.ValP = command.phosphorous;
                    soilTest.valPH = command.pH;
                    command.kelowna = _soilTestConversions.GetConvertedSTP(command.selLaboratoryOption, soilTest);
                    command.isShowKelowna = true;
                }
                return await Task.FromResult(command);
            }

            public Task<ResultModel> Handle(ConverterQuery request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
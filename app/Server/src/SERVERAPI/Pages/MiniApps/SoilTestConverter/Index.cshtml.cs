using Agri.CalculateService;
using Agri.Data;
using Agri.Interfaces;
using Agri.Models.Configuration;
using Agri.Models.Farm;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SERVERAPI.Pages.MiniApps
{
    public class Index : PageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public ConverterQuery Data { get; set; }

        [BindProperty]
        public ResultModel Result { get; set; }

        public Index(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query { PopulatedData = new ConverterQuery() });
            Result = new ResultModel();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                //Send the Data to Receive conversion result
                Result = await _mediator.Send(Data);
                ModelState.Remove("Result.KelownaConversion");
            }
            Data = await _mediator.Send(new Query { PopulatedData = Data });
            return Page();
        }

        public class Query : IRequest<ConverterQuery>
        {
            public ConverterQuery PopulatedData { get; set; }
        }

        public class ConverterQuery : IRequest<ResultModel>
        {
            public decimal PH { get; set; }
            public decimal Phosphorous { get; set; }
            public SelectList LaboratoryOptions { get; set; }
            public string SelectLaboratoryOption { get; set; }
            public string SoilTestConverterUserInstruction1 { get; set; }
            public string SoilTestConverterUserInstruction2 { get; set; }
            public string SoilTestingInformation { get; set; }
            public string BCNutrientManagementCalculator { get; set; }
            public string SoilTestInformationButtonLink { get; set; }
            public string BCNutrientManagementCalculatorButtonLink { get; set; }
        }

        public class ResultModel
        {
            public int KelownaConversion { get; set; }
            public bool ShowKelowna { get; set; }
        }

        public class ModelValidator : AbstractValidator<ConverterQuery>
        {
            public ModelValidator()
            {
                RuleFor(m => m.SelectLaboratoryOption).NotNull()
                    .Must(m => !m.Equals("0"))
                    .WithMessage("Laboratory must be selected");
                RuleFor(m => m.PH).GreaterThan(0).WithMessage("pH Field is required");
                RuleFor(m => m.Phosphorous).GreaterThan(0).WithMessage("Phosphorous Field is required");
            }
        }

        public class Handler :
            IRequestHandler<Query, ConverterQuery>,
            IRequestHandler<ConverterQuery, ResultModel>
        {
            private readonly IAgriConfigurationRepository _sd;
            private readonly ISoilTestConverter _soilTestConversions;

            public Handler(IAgriConfigurationRepository sd, ISoilTestConverter soilTestConversions)
            {
                _sd = sd;
                _soilTestConversions = soilTestConversions;
            }

            public async Task<ConverterQuery> Handle(Query request, CancellationToken cancellationToken)
            {
                var command = request.PopulatedData;

                command.LaboratoryOptions = new SelectList(_sd.GetSoilTestMethodsDll(), "Id", "Value");
                var details = _sd.GetSoilConverterDetails();
                command.SoilTestConverterUserInstruction1 = details.Where(x => x.Key == "SoilTestConverterUserInstruction1").Select(x => x.Value).FirstOrDefault();
                command.SoilTestConverterUserInstruction2 = details.Where(x => x.Key == "SoilTestConverterUserInstruction2").Select(x => x.Value).FirstOrDefault();
                command.SoilTestingInformation = details.Where(x => x.Key == "SoilTestingInformation").Select(x => x.Value).FirstOrDefault();
                command.BCNutrientManagementCalculator = details.Where(x => x.Key == "BCNutrientManagementCalculator").Select(x => x.Value).FirstOrDefault();
                command.SoilTestInformationButtonLink = details.Where(x => x.Key == "SoilTestInformationButtonLink").Select(x => x.Value).FirstOrDefault();
                command.BCNutrientManagementCalculatorButtonLink = details.Where(x => x.Key == "BCNutrientManagementCalculatorButtonLink").Select(x => x.Value).FirstOrDefault();

                return await Task.FromResult(command);
            }

            public Task<ResultModel> Handle(ConverterQuery request, CancellationToken cancellationToken)
            {
                var soilTest = new SoilTest();
                soilTest.ValP = request.Phosphorous;
                soilTest.valPH = request.PH;

                var result = new ResultModel
                {
                    KelownaConversion = _soilTestConversions.GetConvertedSTP(request.SelectLaboratoryOption, soilTest),
                    ShowKelowna = true
                };

                return Task.FromResult(result);
            }
        }
    }
}
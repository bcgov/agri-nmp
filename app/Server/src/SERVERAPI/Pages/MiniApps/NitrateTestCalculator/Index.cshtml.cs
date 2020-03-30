using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Agri.CalculateService;
using Agri.Data;
using Agri.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SERVERAPI.Pages.MiniApps.NitrateTestCalculator
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
            if (Data.PostedElementEvent == "ResetClicked")
            {
                ModelState.Clear();
                Data.PostedElementEvent = "none";
                Data.IsBasic = false;
                if (Data.nitrateTestAnalysis.Count > 0)
                {
                    Data.nitrateTestAnalysis = null;
                }
            }
            else if (Data.PostedElementEvent == "DepthChange")
            {
                ModelState.Clear();
                Data.PostedElementEvent = "none";
                if (Data.nitrateTestAnalysis[0].SelectDepthOption == "3")
                {
                    Data.isNotShowButton = true;
                    if (Data.nitrateTestAnalysis.Count == 2)
                    {
                        Data.nitrateTestAnalysis.RemoveAt(1);
                    }
                }
                else
                {
                    Data.isNotShowButton = Data.nitrateTestAnalysis.Count != 2 ? false : true;
                }
            }
            else if (Data.PostedElementEvent == "BasicChange")
            {
                ModelState.Clear();
                Data.PostedElementEvent = "none";
                Data.IsBasic = false;
                foreach (var nitrateTest in Data.nitrateTestAnalysis)
                {
                    nitrateTest.BulkDensity = 1300;
                }
            }
            else if (Data.PostedElementEvent == "AdvancedChange")
            {
                ModelState.Clear();
                Data.PostedElementEvent = "none";
                Data.IsBasic = true;
            }
            else if (Data.PostedElementEvent == "CalculateClicked")
            {
                if (ModelState.IsValid)
                {
                    ModelState.Clear();
                    Data.PostedElementEvent = "none";
                    Data.IsCalculate = true;
                    Data.isNotShowButton = true;
                }
            }
            else if (Data.PostedElementEvent == "AddDepth")
            {
                ModelState.Clear();
                Data.PostedElementEvent = "none";
                if (Data.nitrateTestAnalysis.Count() != 2)
                {
                    var newId = Data.nitrateTestAnalysis.Count + 1;
                    Data.nitrateTestAnalysis.Add(new ConverterQuery.NitrateTest { Id = newId, BulkDensity = 1300 });
                    Data.isNotShowButton = true;
                }
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
            public List<NitrateTest> nitrateTestAnalysis { get; set; }

            public bool isNotShowButton { get; set; }
            public string SoilTestingInformation { get; set; }
            public string NitrateCalculatorUserInstruction1 { get; set; }
            public string NitrateCalculatorUserInstruction2 { get; set; }
            public string NitrateCalculatorUserInstruction3 { get; set; }
            public string BCNutrientManagementCalculator { get; set; }
            public string SoilTestInformationButtonLink { get; set; }
            public string BCNutrientManagementCalculatorButtonLink { get; set; }
            public string SampleDepthMessage { get; set; }
            public string SoilBulkDensityMessage { get; set; }

            public string PostedElementEvent { get; set; }
            public double TotalResult { get; set; }
            public bool IsBasic { get; set; }
            public bool BulkDensity { get; set; }
            public bool IsCalculate { get; set; }

            public class NitrateTest
            {
                public int Id { get; set; }
                public SelectList DepthOptions { get; set; }
                public string SelectDepthOption { get; set; }
                public double? Nitrate { get; set; }
                public double? BulkDensity { get; set; }
                public double? Result { get; set; }
                public string SelectDepthOptionName { get; set; }
            }
        }

        public class ResultModel
        {
        }

        public class ModelValidator : AbstractValidator<ConverterQuery>
        {
            public ModelValidator()
            {
                RuleForEach(m => m.nitrateTestAnalysis)
                    .SetValidator(new CommandNitrateTestValidator());
            }
        }

        public class CommandNitrateTestValidator : AbstractValidator<ConverterQuery.NitrateTest>
        {
            public CommandNitrateTestValidator()
            {
                RuleFor(m => m.SelectDepthOption).NotEqual("0" +
                    "").WithMessage("Sample Depth must be selected");
                RuleFor(x => x.Nitrate).NotNull().NotEqual(0).WithMessage("Required");
            }
        }

        public class Handler :
           IRequestHandler<Query, ConverterQuery>,
           IRequestHandler<ConverterQuery, ResultModel>
        {
            private readonly IAgriConfigurationRepository _sd;
            private readonly INitrateTestCalculator _nitrateTestCalculator;

            public Handler(IAgriConfigurationRepository sd, INitrateTestCalculator nitrateTestCalculator)
            {
                _sd = sd;
                _nitrateTestCalculator = nitrateTestCalculator;
            }

            public async Task<ConverterQuery> Handle(Query request, CancellationToken cancellationToken)
            {
                var command = request.PopulatedData;
                if (command.nitrateTestAnalysis == null)
                {
                    command.nitrateTestAnalysis = new List<ConverterQuery.NitrateTest>();
                    command.nitrateTestAnalysis.Add(new ConverterQuery.NitrateTest
                    {
                        Id = 1,
                        BulkDensity = 1300
                    });
                }

                foreach (var nitrateTest in command.nitrateTestAnalysis)
                {
                    if (nitrateTest.Id != 2)
                    {
                        nitrateTest.DepthOptions = new SelectList(_sd.GetDepths().Where(x => x.Id != 2), "Id", "Value");
                    }
                    else
                    {
                        nitrateTest.DepthOptions = new SelectList(_sd.GetDepths().Where(x => x.Id == 2), "Id", "Value");
                        nitrateTest.SelectDepthOption = "2";
                        nitrateTest.SelectDepthOptionName = _sd.GetDepths().Where(x => x.Id == 2).Select(x => x.Value).FirstOrDefault();
                    }

                    if (nitrateTest.Nitrate != 0 && (!string.IsNullOrEmpty(nitrateTest.SelectDepthOption) && nitrateTest.SelectDepthOption != "0" && command.IsCalculate))
                    {
                        nitrateTest.Result = _nitrateTestCalculator.CalculateResult(nitrateTest.SelectDepthOption, nitrateTest.Nitrate.GetValueOrDefault(0), nitrateTest.BulkDensity.GetValueOrDefault(0));
                    }
                }

                if (command.IsCalculate)
                {
                    command.TotalResult = command.nitrateTestAnalysis.Select(x => x.Result.GetValueOrDefault(0)).Sum();
                }

                if (command.nitrateTestAnalysis.Count() == 1 && command.nitrateTestAnalysis[0].SelectDepthOption == null)
                {
                    command.isNotShowButton = true;
                }
                command.isNotShowButton = command.isNotShowButton ? command.isNotShowButton : false;
                command.IsBasic = command.IsBasic ? command.IsBasic : false;
                var details = _sd.GetNitrateCalculatorDetails();
                command.NitrateCalculatorUserInstruction1 = details.Where(x => x.Key == "NitrateCalculatorUserInstruction1").Select(x => x.Value).FirstOrDefault();
                command.NitrateCalculatorUserInstruction2 = details.Where(x => x.Key == "NitrateCalculatorUserInstruction2").Select(x => x.Value).FirstOrDefault();
                command.NitrateCalculatorUserInstruction3 = details.Where(x => x.Key == "NitrateCalculatorUserInstruction3").Select(x => x.Value).FirstOrDefault();
                command.SoilTestingInformation = details.Where(x => x.Key == "SoilTestingInformation").Select(x => x.Value).FirstOrDefault();
                command.BCNutrientManagementCalculator = details.Where(x => x.Key == "BCNutrientManagementCalculator").Select(x => x.Value).FirstOrDefault();
                command.SoilTestInformationButtonLink = details.Where(x => x.Key == "SoilTestInformationButtonLink").Select(x => x.Value).FirstOrDefault();
                command.BCNutrientManagementCalculatorButtonLink = details.Where(x => x.Key == "BCNutrientManagementCalculatorButtonLink").Select(x => x.Value).FirstOrDefault();
                command.SampleDepthMessage = details.Where(x => x.Key == "SampleDepthMessage").Select(x => x.Value).FirstOrDefault();
                command.SoilBulkDensityMessage = details.Where(x => x.Key == "SoilBulkDensityMessage").Select(x => x.Value).FirstOrDefault();

                return await Task.FromResult(command);
            }

            public Task<ResultModel> Handle(ConverterQuery request, CancellationToken cancellationToken)
            {
                var result = new ResultModel();

                return Task.FromResult(result);
            }
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Agri.CalculateService;
using Agri.Data;
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

            if (Data.PostedElementEvent == "DepthChange" && ModelState.IsValid)
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
                Data.isBasic = false;
                foreach (var nitrateTest in Data.nitrateTestAnalysis)
                {
                    nitrateTest.bulkDensity = 1300;
                }
            }
            else if (Data.PostedElementEvent == "AdvancedChange")
            {
                ModelState.Clear();
                Data.PostedElementEvent = "none";
                Data.isBasic = true;
            }
            else if (ModelState.IsValid)
            {
                ModelState.Clear();
                Data.PostedElementEvent = "none";
                if (Data.nitrateTestAnalysis.Count() != 2)
                {
                    var newId = Data.nitrateTestAnalysis.Count + 1;
                    Data.nitrateTestAnalysis.Add(new ConverterQuery.NitrateTest { Id = newId, bulkDensity = 1300 });
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

            public string PostedElementEvent { get; set; }
            public double totalResult { get; set; }
            public bool isBasic { get; set; }

            public class NitrateTest
            {
                public int Id { get; set; }
                public SelectList DepthOptions { get; set; }
                public string SelectDepthOption { get; set; }
                public double nitrate { get; set; }
                public double bulkDensity { get; set; }
                public double result { get; set; }
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
                    "").WithMessage("Sample must be selected");
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
                        bulkDensity = 1300
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
                    }
                    if (nitrateTest.nitrate != 0 && (!string.IsNullOrEmpty(nitrateTest.SelectDepthOption) && nitrateTest.SelectDepthOption!="0"))
                    {
                        nitrateTest.result = _nitrateTestCalculator.CalculateResult(nitrateTest.SelectDepthOption,nitrateTest.nitrate, nitrateTest.bulkDensity);
                    }
                }
                command.totalResult = command.nitrateTestAnalysis.Select(x => x.result).Sum();
                command.isNotShowButton = command.isNotShowButton ? command.isNotShowButton : false;
                command.isBasic = command.isBasic ? command.isBasic : false;
                var details = _sd.GetNitrateCalculatorDetails();
                command.NitrateCalculatorUserInstruction1 = details.Where(x => x.Key == "NitrateCalculatorUserInstruction1").Select(x => x.Value).FirstOrDefault();
                command.NitrateCalculatorUserInstruction2 = details.Where(x => x.Key == "NitrateCalculatorUserInstruction2").Select(x => x.Value).FirstOrDefault();
                command.NitrateCalculatorUserInstruction3 = details.Where(x => x.Key == "NitrateCalculatorUserInstruction3").Select(x => x.Value).FirstOrDefault();
                command.SoilTestingInformation = details.Where(x => x.Key == "SoilTestingInformation").Select(x => x.Value).FirstOrDefault();
                command.BCNutrientManagementCalculator = details.Where(x => x.Key == "BCNutrientManagementCalculator").Select(x => x.Value).FirstOrDefault();
                command.SoilTestInformationButtonLink = details.Where(x => x.Key == "SoilTestInformationButtonLink").Select(x => x.Value).FirstOrDefault();
                command.BCNutrientManagementCalculatorButtonLink = details.Where(x => x.Key == "BCNutrientManagementCalculatorButtonLink").Select(x => x.Value).FirstOrDefault();
                command.SampleDepthMessage = details.Where(x => x.Key == "SampleDepthMessage").Select(x => x.Value).FirstOrDefault();

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
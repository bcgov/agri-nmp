﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            if (Data.PostedElementEvent == "DepthChange")
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
            }
            else
            {
                ModelState.Clear();
                Data.PostedElementEvent = "none";
                if (Data.nitrateTestAnalysis.Count() != 2)
                {
                    var newId = Data.nitrateTestAnalysis.Count + 1;
                    Data.nitrateTestAnalysis.Add(new NitrateTest { Id = newId });
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
            public string NitrateTestCalculatorUserInstruction1 { get; set; }
            public string NitrateTestCalculatorUserInstruction2 { get; set; }
            public string BCNutrientManagementCalculator { get; set; }
            public string SoilTestInformationButtonLink { get; set; }
            public string BCNutrientManagementCalculatorButtonLink { get; set; }

            public string PostedElementEvent { get; set; }
        }

        public class NitrateTest
        {
            public int Id { get; set; }
            public SelectList DepthOptions { get; set; }
            public string SelectDepthOption { get; set; }
        }

        public class ResultModel
        {
        }

        public class ModelValidator : AbstractValidator<ConverterQuery>
        {
            public ModelValidator()
            {
            }
        }

        public class Handler :
           IRequestHandler<Query, ConverterQuery>,
           IRequestHandler<ConverterQuery, ResultModel>
        {
            private readonly IAgriConfigurationRepository _sd;

            public Handler(IAgriConfigurationRepository sd)
            {
                _sd = sd;
            }

            public async Task<ConverterQuery> Handle(Query request, CancellationToken cancellationToken)
            {
                var command = request.PopulatedData;
                if (command.nitrateTestAnalysis == null)
                {
                    command.nitrateTestAnalysis = new List<NitrateTest>();
                    command.nitrateTestAnalysis.Add(new NitrateTest
                    {
                        Id = 1
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
                }
                command.isNotShowButton = command.isNotShowButton ? command.isNotShowButton : false;
                var details = _sd.GetNitrateCalculatorDetails();
                command.NitrateTestCalculatorUserInstruction1 = details.Where(x => x.Key == "NitrateTestCalculatorUserInstruction1").Select(x => x.Value).FirstOrDefault();
                command.NitrateTestCalculatorUserInstruction2 = details.Where(x => x.Key == "NitrateTestCalculatorUserInstruction2").Select(x => x.Value).FirstOrDefault();
                command.SoilTestingInformation = details.Where(x => x.Key == "SoilTestingInformation").Select(x => x.Value).FirstOrDefault();
                command.BCNutrientManagementCalculator = details.Where(x => x.Key == "BCNutrientManagementCalculator").Select(x => x.Value).FirstOrDefault();
                command.SoilTestInformationButtonLink = details.Where(x => x.Key == "SoilTestInformationButtonLink").Select(x => x.Value).FirstOrDefault();
                command.BCNutrientManagementCalculatorButtonLink = details.Where(x => x.Key == "BCNutrientManagementCalculatorButtonLink").Select(x => x.Value).FirstOrDefault();

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
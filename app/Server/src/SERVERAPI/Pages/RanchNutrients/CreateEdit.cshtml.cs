using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Agri.Data;
using Agri.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SERVERAPI.Models.Impl;

namespace SERVERAPI.Pages.RanchNutrients
{
    public class CreateEdit : BasePageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public Command Data { get; set; }

        [BindProperty]
        public bool IsModal { get; set; }

        public CreateEdit(IMediator mediator)
        {
            Title = "Nutrient Analysis";
            _mediator = mediator;
        }

        public async Task OnGetCreateAsync()
        {
            Data = await _mediator.Send(new Query());
        }

        public async Task OnGetEditAsync(Query query)
        {
            Data = await _mediator.Send(query);
        }

        protected async Task<IActionResult> OnPostCreateAsync()
        {
            return await HandlePostBack();
        }

        protected async Task<IActionResult> OnPostEditAsync()
        {
            return await HandlePostBack();
        }

        private async Task<IActionResult> HandlePostBack()
        {
            if (Data.PostedElementEvent == ElementEvent.UseCustomAnalysis)
            {
                Data.UseBookValue = false;
            }
            Data = await _mediator.Send(new Query());
            return Page();
        }

        public class Query : IRequest<Command>
        {
            public int? Id { get; set; }
        }

        public class Command : IRequest<Unit>
        {
            public int? Id { get; set; }
            public SelectList RanchManures { get; set; }
            public int SelectedNutrientAnalysis { get; set; }
            public SelectList BeefNutrientAnalysisOptions { get; set; }

            public string Action { get; set; }
            public string SourceOfMaterialName { get; set; }

            [Display(Name = "Material Type")]
            public string ManureName { get; set; }

            public ManureMaterialType MaterialType { get; set; }

            [Display(Name = "Moisture (%)")]
            public string Moisture { get; set; }

            [Display(Name = "N (%)")]
            public string Nitrogen { get; set; }

            [Display(Name = "NH<sub>4</sub>-N (ppm)")]
            public string Ammonia { get; set; }

            [Display(Name = "P (%)")]
            public string Phosphorous { get; set; }

            [Display(Name = "K (%)")]
            public string Potassium { get; set; }

            [Display(Name = "NO<sub>3</sub>-N (ppm)")]
            public string Nitrate { get; set; }

            public bool UseBookValue { get; set; } = true;
            public bool Compost { get; set; }
            public string MoistureBook { get; set; }
            public string NitrogenBook { get; set; }
            public string AmmoniaBook { get; set; }
            public string NitrateBook { get; set; }
            public string PhosphorousBook { get; set; }
            public string PotassiumBook { get; set; }
            public bool ShowNitrate { get; set; }
            public NutrientAnalysisTypes StoredImported { get; set; }
            public string ExplainNutrientAnalysisMoisture { get; set; }
            public string ExplainNutrientAnalysisNitrogen { get; set; }
            public string ExplainNutrientAnlalysisAmmonia { get; set; }
            public string ExplainNutrientAnlalysisPhosphorous { get; set; }
            public string ExplainNutrientAnlalysisPotassium { get; set; }
            public bool IsLegacyNMPReleaseVersion { get; set; }
            public int? LegacyNMPReleaseVersionManureId { get; set; }
            public ElementEvent PostedElementEvent { get; set; }
        }

        public enum ElementEvent
        {
            None,
            UseCustomAnalysis
        }

        public class Handler : IRequestHandler<Query, Command>
        {
            private readonly UserData _ud;
            private readonly IMapper _mapper;
            private readonly IAgriConfigurationRepository _sd;

            public Handler(UserData ud, IMapper mapper, IAgriConfigurationRepository sd)
            {
                _ud = ud;
                _mapper = mapper;
                _sd = sd;
            }

            public Task<Command> Handle(Query request, CancellationToken cancellationToken)
            {
                //var ranchManures = _ud.
                var beefManuresNutrients = _sd.GetManures()
                    .Where(m => m.ManureClass.Contains("Beef"))
                    .Select(m => new { Id = m.Id, Name = m.Name })
                    .ToList();

                //command.CattleSubTypeOptions = new SelectList(subTypeOptions, "Id", "Value");
                var command = new Command
                {
                    BeefNutrientAnalysisOptions = new SelectList(beefManuresNutrients, "Id", "Name")
                };
                return Task.FromResult(command);
            }
        }
    }
}
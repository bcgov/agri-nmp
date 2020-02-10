using Agri.Data;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading;
using System.Threading.Tasks;

namespace SERVERAPI.Pages.MiniApps.ManureNutrientConvertor
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
            if (Data.PostedElementEvent == "TypeChanged")
            {
                ModelState.Clear();
                Data.PostedElementEvent = "None";
            }
            else if (ModelState.IsValid)
            {
                //Send the Data to Receive conversion result
                Result = await _mediator.Send(Data);
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
            public int SelectedManureType { get; set; }
            public string SelectedUnit { get; set; }
            public int SelectedApplication { get; set; }
            public int SelectRegion { get; set; }
            public SelectList ManureTypeOptions { get; set; }
            public SelectList UnitOptions { get; set; }
            public SelectList ApplicationOptions { get; set; }
            public SelectList RegionOptions { get; set; }
            public string PostedElementEvent { get; set; }
            public string Moisture { get; set; }
            public decimal Nitrogen { get; set; }
            public decimal Ammonia { get; set; }
            public decimal Phosphorous { get; set; }
            public decimal Potassium { get; set; }
            public int DMId { get; set; }
            public decimal AmmoniaRention { get; set; }
            public decimal OrganicN_FirstYear { get; set; }
            public int NMinerizationId { get; set; }
            public bool isShowValue { get; set; }
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
                command.isShowValue = false;
                command.ManureTypeOptions = new SelectList(_sd.GetManures(), "Id", "Name");
                command.UnitOptions = new SelectList(_sd.GetUnits(), "Id", "Name");
                command.RegionOptions = new SelectList(_sd.GetRegions(), "Id", "Name");
                if (command.SelectedManureType == 0)
                {
                    command.ApplicationOptions = new SelectList(_sd.GetApplications(), "Id", "Name");
                }
                else
                {
                    command.ApplicationOptions = new SelectList(_sd.GetApplicationsDll(_sd.GetManure(command.SelectedManureType).SolidLiquid), "Id", "Value");
                    var man = _sd.GetManure(command.SelectedManureType);
                    command.Ammonia = man.Ammonia;
                    command.DMId = man.DryMatterId;
                    //command.ManureClass = man.ManureClass;
                    command.Moisture = man.Moisture;
                    //command.Name = man.Name;
                    //command.Nitrate = man.Nitrate;
                    command.Nitrogen = man.Nitrogen;
                    command.NMinerizationId = man.NMineralizationId;
                    command.Phosphorous = man.Phosphorous;
                    command.Potassium = man.Potassium;
                }
                if (command.SelectedApplication != 0 && command.SelectedManureType != 0 && command.SelectRegion != 0)
                {
                    var myAmmoniaRetention = _sd.GetAmmoniaRetention(command.SelectedApplication, command.DMId);
                    command.AmmoniaRention = myAmmoniaRetention.Value ?? 0;
                    var myNMineralization = _sd.GetNMineralization(command.NMinerizationId, command.SelectRegion);
                    command.OrganicN_FirstYear = 0;
                }
                return await Task.FromResult(command);
            }

            public Task<ResultModel> Handle(ConverterQuery request, CancellationToken cancellationToken)
            {
                var result = new ResultModel
                {
                };
                return Task.FromResult(result);
            }
        }
    }
}
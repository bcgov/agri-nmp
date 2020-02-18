using Agri.Data;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System;
using Agri.Models.Calculate;

namespace SERVERAPI.Pages.MiniApps.ManureNutrientCalculator
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
                Data.isShowValue = Data.isShowValue ? Data.isShowValue : false;
            }
            else if (Data.PostedElementEvent == "ResetN")
            {
                ModelState.Clear();
                Data.PostedElementEvent = "None";
                Data.isShowValue = false;
                Data.stdN = false;
            }
            else if (Data.PostedElementEvent == "ResetA")
            {
                ModelState.Clear();
                Data.PostedElementEvent = "None";
                Data.isShowValue = false;
                Data.stdAvail = false;
            }
            else if (ModelState.IsValid)
            {
                Data.isShowValue = true;
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
            public decimal ApplicationRate { get; set; }
            public string PostedElementEvent { get; set; }
            public string Moisture { get; set; }
            public decimal Nitrogen { get; set; }
            public decimal Ammonia { get; set; }
            public decimal Phosphorous { get; set; }
            public decimal Potassium { get; set; }
            public int DMId { get; set; }
            public decimal AmmoniaRention { get; set; }
            public decimal OrganicN_FirstYear { get; set; }
            public decimal OrganicN_LongTerm { get; set; }
            public int NMinerizationId { get; set; }
            public decimal? Nitrate { get; set; }
            public bool isShowValue { get; set; }
            public bool ToggleElementState { get; set; }
            public string ManurenNutrientCalculatorUserInstruction1 { get; set; }
            public string ManurenNutrientCalculatorUserInstruction2 { get; set; }
            public string NutrientManagementInformation { get; set; }
            public string BCNutrientManagementCalculator { get; set; }
            public string NutrientManagementInformationButtonLink { get; set; }
            public string BCNutrientManagementCalculatorButtonLink { get; set; }
            public bool stdAvail { get; set; }
            public bool stdN { get; set; }
        }

        public class ResultModel
        {
            public int K2O_FirstYear { get; set; }
            public int K2O_LongTerm { get; set; }
            public int P2O5_FirstYear { get; set; }
            public int P2O5_LongTerm { get; set; }
            public int N_FirstYear { get; set; }
            public int N_LongTerm { get; set; }
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
                var details = _sd.GetManureNutrientCalculatorDetails();
                command.ManurenNutrientCalculatorUserInstruction1 = details.Where(x => x.Key == "ManurenNutrientCalculatorUserInstruction1").Select(x => x.Value).FirstOrDefault();
                command.ManurenNutrientCalculatorUserInstruction2 = details.Where(x => x.Key == "ManurenNutrientCalculatorUserInstruction2").Select(x => x.Value).FirstOrDefault();
                command.NutrientManagementInformation = details.Where(x => x.Key == "NutrientManagementInformation").Select(x => x.Value).FirstOrDefault();
                command.BCNutrientManagementCalculator = details.Where(x => x.Key == "BCNutrientManagementCalculator").Select(x => x.Value).FirstOrDefault();
                command.NutrientManagementInformationButtonLink = details.Where(x => x.Key == "NutrientManagementInformationButtonLink").Select(x => x.Value).FirstOrDefault();
                command.BCNutrientManagementCalculatorButtonLink = details.Where(x => x.Key == "BCNutrientManagementCalculatorButtonLink").Select(x => x.Value).FirstOrDefault();

                command.isShowValue = command.isShowValue ? command.isShowValue : false;
                command.ManureTypeOptions = new SelectList(_sd.GetManures(), "Id", "Name");
                command.RegionOptions = new SelectList(_sd.GetRegions(), "Id", "Name");
                if (command.SelectedManureType == 0)
                {
                    command.ApplicationOptions = new SelectList(_sd.GetApplications(), "Id", "Name");
                    command.UnitOptions = new SelectList(_sd.GetUnits(), "Id", "Name");
                }
                else
                {
                    command.ApplicationOptions = new SelectList(_sd.GetApplicationsDll(_sd.GetManure(command.SelectedManureType).SolidLiquid), "Id", "Value");
                    command.UnitOptions = new SelectList(_sd.GetUnitsDll(_sd.GetManure(command.SelectedManureType).SolidLiquid), "Id", "Value");
                    var man = _sd.GetManure(command.SelectedManureType);
                    command.Ammonia = man.Ammonia;
                    command.DMId = man.DryMatterId;
                    //command.ManureClass = man.ManureClass;
                    command.Moisture = man.Moisture;
                    //command.Name = man.Name;
                    command.Nitrate = man.Nitrate;
                    command.Nitrogen = man.Nitrogen;
                    command.NMinerizationId = man.NMineralizationId;
                    command.Phosphorous = man.Phosphorous;
                    command.Potassium = man.Potassium;
                    if (command.Potassium == 0 && command.Phosphorous == 0 && command.Nitrogen == 0 && command.Ammonia == 0)
                    {
                        command.ToggleElementState = true;
                    }
                }
                if (command.SelectedApplication != 0 && command.SelectedManureType != 0 && command.SelectRegion != 0)
                {
                    var myAmmoniaRetention = _sd.GetAmmoniaRetention(command.SelectedApplication, command.DMId);
                    var ammoniaRention = (myAmmoniaRetention.Value ?? 0) * 100;
                    if (command.isShowValue && command.AmmoniaRention != 0 && command.AmmoniaRention != ammoniaRention)
                    {
                        command.stdN = true;
                    }
                    else
                    {
                        command.AmmoniaRention = ammoniaRention;
                    }

                    var region = _sd.GetRegion(command.SelectRegion);
                    var myNMineralization = _sd.GetNMineralization(command.NMinerizationId, region.LocationId);
                    var firstYearOrganic = myNMineralization.FirstYearValue * 100;
                    if (command.isShowValue && command.OrganicN_FirstYear != 0 && command.OrganicN_FirstYear != firstYearOrganic)
                    {
                        command.stdAvail = true;
                    }
                    else
                    {
                        command.OrganicN_FirstYear = firstYearOrganic;
                    }
                    command.OrganicN_LongTerm = myNMineralization.LongTermValue;
                }

                return await Task.FromResult(command);
            }

            public Task<ResultModel> Handle(ConverterQuery request, CancellationToken cancellationToken)
            {
                var result = new ResultModel();
                var _cf = _sd.GetConversionFactor();

                decimal potassiumAvailabilityFirstYear = _cf.PotassiumAvailabilityFirstYear;
                decimal potassiumAvailabilityLongTerm = _cf.PotassiumAvailabilityLongTerm;
                decimal potassiumKtoK2Oconversion = _cf.PotassiumKtoK2OConversion;
                decimal phosphorousAvailabilityFirstYear = _cf.PhosphorousAvailabilityFirstYear;
                decimal phosphorousAvailabilityLongTerm = _cf.PhosphorousAvailabilityLongTerm;
                decimal phosphorousPtoP2O5Kconversion = _cf.PhosphorousPtoP2O5Conversion;
                decimal lbPerTonConversion = _cf.PoundPerTonConversion;
                decimal tenThousand = 10000;
                decimal applicationRate = request.ApplicationRate;

                // get conversion factor for selected units to lb/ac
                var myunit = _sd.GetUnit(request.SelectedUnit);
                decimal conversion = myunit.ConversionlbTon;

                // for solid manures specified in cubic yards per ac, convert application rate to tons/ac
                if (myunit.Id == 6)//&& farmManure.SolidLiquid.ToUpper() == "SOLID"
                {
                    var manure = _sd.GetManure(request.SelectedManureType);
                    applicationRate = applicationRate * manure.CubicYardConversion;
                }

                // get potassium first year
                result.K2O_FirstYear = Convert.ToInt32(decimal.Multiply(applicationRate, request.Potassium)
                                                * lbPerTonConversion
                                                * potassiumKtoK2Oconversion
                                                * potassiumAvailabilityFirstYear
                                                * conversion);

                // get potassium long term
                result.K2O_LongTerm = Convert.ToInt32(decimal.Multiply(applicationRate, request.Potassium)
                                                * lbPerTonConversion
                                                * potassiumKtoK2Oconversion
                                                * potassiumAvailabilityLongTerm
                                                * conversion);

                // get phosphorous first year
                result.P2O5_FirstYear = Convert.ToInt32(decimal.Multiply(applicationRate, request.Phosphorous)
                                                * lbPerTonConversion
                                                * phosphorousPtoP2O5Kconversion
                                                * phosphorousAvailabilityFirstYear
                                                * conversion);

                // get phosphorous long term
                result.P2O5_LongTerm = Convert.ToInt32(decimal.Multiply(applicationRate, request.Phosphorous)
                                                * lbPerTonConversion
                                                * phosphorousPtoP2O5Kconversion
                                                * phosphorousAvailabilityLongTerm
                                                * conversion);

                decimal organicN = request.Nitrogen - Convert.ToDecimal(request.Ammonia) / tenThousand;

                var OrganicN_FirstYear = request.OrganicN_FirstYear / 100; // get data from screen

                //decimal ammoniaRetention = GetAmmoniaRetention(mymanure.id, Convert.ToInt32(applicationSeason));
                decimal ammoniaRetention = request.AmmoniaRention / 100; // get data from screen

                // N 1st year lb/ton = [NH4-N ppm/10,000 * NH4 retention + NO3-N/10,000 + Organic N %  * 1st yr Mineralization] * 20

                decimal a = decimal.Divide(request.Ammonia, tenThousand) * ammoniaRetention;

                decimal b1 = decimal.Multiply(organicN, OrganicN_FirstYear);
                //E07US20
                decimal c1 = a + b1 + Convert.ToDecimal(request.Nitrate) / tenThousand;
                decimal N_Firstyear = decimal.Multiply(c1, lbPerTonConversion);
                result.N_FirstYear = Convert.ToInt32(applicationRate * N_Firstyear * conversion);

                // same for long term
                decimal b2 = decimal.Multiply(organicN, request.OrganicN_LongTerm);
                //E07US20
                decimal c2 = a + b2 + Convert.ToDecimal(request.Nitrate) / tenThousand;
                decimal N_LongTerm = decimal.Multiply(c2, lbPerTonConversion);
                result.N_LongTerm = Convert.ToInt32(applicationRate * N_LongTerm * conversion);
                return Task.FromResult(result);
            }
        }
    }
}
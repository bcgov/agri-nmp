using Agri.CalculateService;
using Agri.Data;
using Agri.Models.Farm;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Models.Impl;
using System.Linq;
using System.Threading.Tasks;

namespace SERVERAPI.ViewComponents
{
    public class CalcPrevYearManure : ViewComponent
    {
        private readonly IAgriConfigurationRepository _sd;
        private readonly UserData _ud;
        private readonly IChemicalBalanceMessage _chemicalBalanceMessage;

        public CalcPrevYearManure(IAgriConfigurationRepository sd, UserData ud, IChemicalBalanceMessage chemicalBalanceMessage)
        {
            _sd = sd;
            _ud = ud;
            _chemicalBalanceMessage = chemicalBalanceMessage;
        }

        public async Task<IViewComponentResult> InvokeAsync(string fldName)
        {
            return View(await GetPrevYearManureAsync(fldName));
        }

        private Task<PrevYearManureApplViewModel> GetPrevYearManureAsync(string fldName)
        {
            PrevYearManureApplViewModel manureVM = new PrevYearManureApplViewModel();

            // get the current associated value for nitrogen credit.  Note, can be null

            Field fld = _ud.GetFieldDetails(fldName);
            manureVM.display = false;
            if (fld.Crops != null)
            {
                if (fld.Crops.Count() > 0)
                {
                    manureVM.display = _sd.WasManureAddedInPreviousYear(fld.PreviousYearManureApplicationFrequency);
                    if (manureVM.display)
                    {
                        manureVM.fldName = fldName;
                        if (fld.PreviousYearManureApplicationNitrogenCredit != null)
                            manureVM.nitrogen = fld.PreviousYearManureApplicationNitrogenCredit;
                        else
                        {
                            // lookup default Nitrogen credit
                            manureVM.nitrogen = _chemicalBalanceMessage.CalcPrevYearManureApplDefault(fld);
                        }
                    }
                    else
                    {
                        fld.PreviousYearManureApplicationNitrogenCredit = null;
                        _ud.UpdateField(fld);
                    }
                }
                else
                {
                    fld.PreviousYearManureApplicationNitrogenCredit = null;
                    _ud.UpdateField(fld);
                }
            }
            else
            {
                //reset the nitrogen credit to null
                fld.PreviousYearManureApplicationNitrogenCredit = null;
                _ud.UpdateField(fld);
            }
            return Task.FromResult(manureVM);
        }
    }

    public class PrevYearManureApplViewModel
    {
        public string fldName { get; set; }
        public bool display { get; set; }
        public int? nitrogen { get; set; }
        public string url { get; set; }
        public string urlText { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agri.Interfaces;
using Agri.Models.Farm;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Controllers;
using SERVERAPI.Models;
using static SERVERAPI.Models.StaticData;
using Agri.LegacyData.Models.Impl;

namespace SERVERAPI.ViewComponents
{
    public class CalcPrevYearManure : ViewComponent
    {

        private IAgriConfigurationRepository _sd;
        private Models.Impl.UserData _ud;

        public CalcPrevYearManure(IAgriConfigurationRepository sd, Models.Impl.UserData ud)
        {
            _sd = sd;
            _ud = ud;
        }


        public async Task<IViewComponentResult> InvokeAsync(string fldName)
        {
            return View(await GetPrevYearManureAsync(fldName));
        }


        private Task<PrevYearManureApplViewModel> GetPrevYearManureAsync(string fldName)
        {
            PrevYearManureApplViewModel manureVM = new PrevYearManureApplViewModel();

            // get the current associated value for nitrogen credit.  Note, can be null
            SERVERAPI.Utility.ChemicalBalanceMessage calculator = new Utility.ChemicalBalanceMessage(_ud, _sd);

            Field fld = _ud.GetFieldDetails(fldName);
            manureVM.display = false;
            if (fld.crops != null) 
            {
                if (fld.crops.Count() > 0)  
                {
                    manureVM.display = _sd.WasManureAddedInPreviousYear(fld.prevYearManureApplicationFrequency);
                    if (manureVM.display)
                    {
                        manureVM.fldName = fldName;
                        if (fld.prevYearManureApplicationNitrogenCredit != null)
                            manureVM.nitrogen = fld.prevYearManureApplicationNitrogenCredit;
                        else
                        {
                            // lookup default Nitrogen credit 
                            manureVM.nitrogen = calculator.calcPrevYearManureApplDefault(fldName);
                        }
                    }
                    else
                    {
                        fld.prevYearManureApplicationNitrogenCredit = null;
                        _ud.UpdateField(fld);
                    }

                }
                else
                {
                    fld.prevYearManureApplicationNitrogenCredit = null;
                    _ud.UpdateField(fld);
                }
            }
            else
            {
                //reset the nitrogen credit to null
                fld.prevYearManureApplicationNitrogenCredit = null;
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

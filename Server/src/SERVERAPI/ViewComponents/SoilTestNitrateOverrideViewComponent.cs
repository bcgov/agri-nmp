using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Controllers;
using SERVERAPI.Models;
using static SERVERAPI.Models.StaticData;

namespace SERVERAPI.ViewComponents
{
    public class SoilTestNitrateOverride : ViewComponent
    {

        private Models.Impl.StaticData _sd;
        private Models.Impl.UserData _ud;

        public SoilTestNitrateOverride(Models.Impl.StaticData sd, Models.Impl.UserData ud)
        {
            _sd = sd;
            _ud = ud;
        }


        public async Task<IViewComponentResult> InvokeAsync(string fldName)
        {
            return View(await GetSoilTestNitrateOverrideAsync(fldName));
        }


        private Task<GetSoilTestNitrateOverrideViewModel> GetSoilTestNitrateOverrideAsync(string fldName)
        {
            GetSoilTestNitrateOverrideViewModel soilvm = new GetSoilTestNitrateOverrideViewModel();
            // get the current associated value for nitrogen credit.  Note, can be null
            SERVERAPI.Utility.ChemicalBalanceMessage calculator = new Utility.ChemicalBalanceMessage(_ud, _sd);

            FarmDetails farmdtl = _ud.FarmDetails();
            Field fld = _ud.GetFieldDetails(fldName);
            soilvm.display = false;
            
            if ( (fld.crops != null)  && (fld.soilTest != null) )
            {  
                if  (fld.crops.Count() > 0)  
                {
                    soilvm.display = _sd.IsNitrateCreditApplicable(farmdtl.farmRegion, fld.soilTest.sampleDate, Convert.ToInt16(farmdtl.year));
                    if (soilvm.display)
                    {
                        soilvm.fldName = fldName;
                        if (fld.SoilTestNitrateOverrideNitrogenCredit != null)
                            soilvm.nitrogen = fld.SoilTestNitrateOverrideNitrogenCredit;
                        else
                        {
                            // lookup default Nitrogen credit 
                            soilvm.nitrogen = Convert.ToInt32(Math.Round(fld.soilTest.valNO3H * _sd.GetSoilTestNitratePPMToPoundPerAcreConversionFactor()));
                        }
                    }
                    else
                    {
                        fld.SoilTestNitrateOverrideNitrogenCredit = null;
                        _ud.UpdateField(fld);
                    }

                }
                else
                {
                    fld.SoilTestNitrateOverrideNitrogenCredit = null;
                    _ud.UpdateField(fld);
                }
            }
            else
            {
                fld.SoilTestNitrateOverrideNitrogenCredit = null;
                _ud.UpdateField(fld);
            }
            return Task.FromResult(soilvm);
        }
    }

    public class GetSoilTestNitrateOverrideViewModel
    {
        public string fldName { get; set; }
        public bool display { get; set; }
        public int? nitrogen { get; set; }
    }


}

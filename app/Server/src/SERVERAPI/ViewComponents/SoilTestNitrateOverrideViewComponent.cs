using Agri.Interfaces;
using Agri.Models.Farm;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Models.Impl;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SERVERAPI.ViewComponents
{
    public class SoilTestNitrateOverride : ViewComponent
    {
        private readonly IAgriConfigurationRepository _sd;
        private readonly UserData _ud;

        public SoilTestNitrateOverride(IAgriConfigurationRepository sd, UserData ud)
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

            FarmDetails farmdtl = _ud.FarmDetails();
            Field fld = _ud.GetFieldDetails(fldName);
            soilvm.display = false;

            if ((fld.crops != null) && (fld.soilTest != null))
            {
                if (fld.crops.Count() > 0)
                {
                    soilvm.display = _sd.IsNitrateCreditApplicable(farmdtl.FarmRegion, fld.soilTest.sampleDate, Convert.ToInt16(farmdtl.Year));
                    if (soilvm.display)
                    {
                        soilvm.fldName = fldName;
                        if (fld.SoilTestNitrateOverrideNitrogenCredit != null)
                            soilvm.nitrogen = Math.Round(Convert.ToDecimal(fld.SoilTestNitrateOverrideNitrogenCredit));
                        else
                        {
                            // lookup default Nitrogen credit
                            soilvm.nitrogen = Math.Round(fld.soilTest.valNO3H * _sd.GetSoilTestNitratePPMToPoundPerAcreConversionFactor());
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
        public decimal? nitrogen { get; set; }
    }
}
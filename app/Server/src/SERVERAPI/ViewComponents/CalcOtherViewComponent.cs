﻿using Agri.Data;
using Agri.Models.Farm;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SERVERAPI.ViewComponents
{
    public class CalcOther : ViewComponent
    {
        private IAgriConfigurationRepository _sd;
        private Models.Impl.UserData _ud;

        public CalcOther(IAgriConfigurationRepository sd, Models.Impl.UserData ud)
        {
            _sd = sd;
            _ud = ud;
        }

        public async Task<IViewComponentResult> InvokeAsync(string fldName)
        {
            return View(await GetOtherAsync(fldName));
        }

        private Task<CalcOtherViewModel> GetOtherAsync(string fldName)
        {
            CalcOtherViewModel ovm = new CalcOtherViewModel();
            ovm.fldName = fldName;

            ovm.others = new List<DisplayNutrientOther>();

            List<NutrientOther> fldOthers = _ud.GetFieldNutrientsOthers(fldName);
            foreach (var m in fldOthers)
            {
                DisplayNutrientOther no = new DisplayNutrientOther()
                {
                    otherId = m.id,
                    description = m.description,
                    ltN = Convert.ToDecimal((m.ltN).ToString("G29")),
                    ltP = Convert.ToDecimal((m.ltP2o5).ToString("G29")),
                    ltK = Convert.ToDecimal((m.ltK).ToString("G29")),
                    yrN = Convert.ToDecimal((m.yrN).ToString("G29")),
                    yrP = Convert.ToDecimal((m.yrP2o5).ToString("G29")),
                    yrK = Convert.ToDecimal((m.yrK).ToString("G29")),
                };
                ovm.others.Add(no);
            }

            return Task.FromResult(ovm);
        }
    }

    public class CalcOtherViewModel
    {
        public string fldName { get; set; }
        public List<DisplayNutrientOther> others { get; set; }
    }

    public class DisplayNutrientOther
    {
        public int otherId { get; set; }
        public string description { get; set; }
        public decimal yrN { get; set; }
        public decimal yrP { get; set; }
        public decimal yrK { get; set; }
        public decimal ltN { get; set; }
        public decimal ltP { get; set; }
        public decimal ltK { get; set; }
    }
}
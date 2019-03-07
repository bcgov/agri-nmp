﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agri.Interfaces;
using Agri.Models.Farm;
using SERVERAPI.Models;
using SERVERAPI.Controllers;
using SERVERAPI.Models.Impl;
using Microsoft.AspNetCore.Hosting;

namespace SERVERAPI.ViewComponents
{
    public class Fields : ViewComponent
    {
        private IHostingEnvironment _env;
        private UserData _ud;
        private IAgriConfigurationRepository _sd;

        public Fields(IHostingEnvironment env, UserData ud, IAgriConfigurationRepository sd)
        {
            _env = env;
            _ud = ud;
            _sd = sd;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await GetFieldsAsync());
        }

        private Task<FieldsViewModel> GetFieldsAsync()
        {
            FieldsViewModel fvm = new FieldsViewModel();
            FarmDetails fd = _ud.FarmDetails();

            if(fd.farmRegion.HasValue)
            {
                fvm.regionFnd = true;
            }
            else
            {
                fvm.regionFnd = false;
                fvm.noRegion = _sd.GetUserPrompt("missingregion");
            }

            fvm.fields = new List<Field>();

            List<Field> fldList = _ud.GetFields();

            foreach (var f in fldList)
            {
                Field nf = new Field();
                nf.fieldName = f.fieldName;
                nf.area = Convert.ToDecimal((f.area).ToString("G29"));
                nf.comment = f.comment;
                fvm.fields.Add(nf);
            }

            return Task.FromResult(fvm);
        }
    }

    public class FieldsViewModel
    {
        public bool regionFnd { get; set; }
        public string noRegion { get; set; }
        public List<Field> fields { get; set; }
    }
}

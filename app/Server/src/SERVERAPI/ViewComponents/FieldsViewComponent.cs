using Agri.Data;
using Agri.Models.Farm;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Models.Impl;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SERVERAPI.ViewComponents
{
    public class Fields : ViewComponent
    {
        private readonly UserData _ud;
        private readonly IAgriConfigurationRepository _sd;

        public Fields(UserData ud, IAgriConfigurationRepository sd)
        {
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

            if (fd.FarmRegion.HasValue)
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
                nf.FieldName = f.FieldName;
                nf.Area = Convert.ToDecimal((f.Area).ToString("G29"));
                nf.Comment = f.Comment;
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
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SERVERAPI.Models;
using SERVERAPI.Controllers;
using SERVERAPI.Models.Impl;
using Microsoft.AspNetCore.Hosting;

namespace SERVERAPI.ViewComponents
{
    public class Compost : ViewComponent
    {
        private IHostingEnvironment _env;
        private UserData _ud;

        public Compost(IHostingEnvironment env, UserData ud)
        {
            _env = env;
            _ud = ud;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await GetCompostAsync());
        }

        private Task<FieldsViewModel> GetCompostAsync()
        {
            FieldsViewModel fvm = new FieldsViewModel();
            fvm.fields = new List<Field>();

            //List<FarmManure> compostList = _ud.GetFields();

            //foreach (var f in fldList)
            //{
            //    Field nf = new Field();
            //    nf.fieldName = f.fieldName;
            //    nf.area = f.area;
            //    nf.comment = f.comment;
            //    fvm.fields.Add(nf);
            //}

            return Task.FromResult(fvm);
        }
    }

    public class CompostViewModel
    {
        public List<FarmManure> composts { get; set; }
    }
}

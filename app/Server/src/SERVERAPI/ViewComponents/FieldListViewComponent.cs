using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Models;
using SERVERAPI.Models.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agri.Models.Farm;

namespace SERVERAPI.ViewComponents
{
    public class FieldList : ViewComponent
    {
        private UserData _ud;

        public FieldList(UserData ud)
        {
            _ud = ud;
        }

        public async Task<IViewComponentResult> InvokeAsync(string cntl, string actn)
        {
            var itemsTask = GetFieldsAsync(cntl, actn);
            var items = await itemsTask;
            return View(items);
        }

        private Task<FieldListViewModel> GetFieldsAsync(string cntl, string actn)
        {
            FieldListViewModel fvm = new FieldListViewModel();
            fvm.actn = actn;
            fvm.cntl = cntl;
            fvm.noCrops = false;
            fvm.fields = new List<Field>();

            List<Field> fldList = _ud.GetFields();

            foreach (var f in fldList)
            {
                Field nf = new Field();
                nf.FieldName = f.FieldName;
                nf.Area = f.Area;
                nf.Comment = f.Comment;
                nf.crops = f.crops;

                fvm.fields.Add(nf);
                if (f.crops == null)
                {
                    fvm.noCrops = true;
                }
            }

            return Task.FromResult(fvm);
        }
    }

    public class FieldListViewModel
    {
        public string cntl { get; set; }
        public string actn { get; set; }
        public bool noCrops { get; set; }
        public List<Field> fields { get; set; }
    }
}
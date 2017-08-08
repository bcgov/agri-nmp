using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SERVERAPI.Models;
using SERVERAPI.Controllers;

namespace SERVERAPI.ViewComponents
{
    public class Fields : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await GetFieldsAsync());
        }

        private Task<FieldsViewModel> GetFieldsAsync()
        {
            FieldsViewModel fvm = new FieldsViewModel();
            fvm.fields = new List<Field>();
            var farmData = HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");

            if (farmData.years != null)
            {
                YearData yd = farmData.years.FirstOrDefault(y => y.year == farmData.year);
                if (yd.fields == null)
                {
                    yd.fields = new List<Field>();
                }


                foreach (var f in yd.fields)
                {
                    Field nf = new Field();
                    nf.fieldName = f.fieldName;
                    nf.area = f.area;
                    nf.comment = f.comment;
                    fvm.fields.Add(nf);
                }
            }

            return Task.FromResult(fvm);
        }
    }

    public class FieldsViewModel
    {
        public List<Field> fields { get; set; }
    }
}

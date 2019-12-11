using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVERAPI.ViewComponents
{
    public class RanchManureViewComponent : ViewComponent
    {
        public RanchManureViewComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }

        //private Task GetManureImportedAsync()
        //{
        //}
    }
}
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVERAPI.Controllers
{
    [SessionTimeout]
    public class RanchManureController : BaseController
    {
        [HttpGet]
        public IActionResult RanchManure()
        {
            return View();
        }
    }
}
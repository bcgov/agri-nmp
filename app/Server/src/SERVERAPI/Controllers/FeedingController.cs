using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SERVERAPI.Controllers
{
    public class FeedingController : Controller
    {
        [HttpGet]
        public IActionResult Feeding()
        {
            return View();
        }
    }
}
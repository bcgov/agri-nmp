using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Filters;
using SERVERAPI.Models.Impl;
using SERVERAPI.ViewModels;
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
        private readonly UserData _userData;

        public RanchManureController(UserData userData)
        {
            _userData = userData;
        }

        [HttpGet]
        public IActionResult RanchManure()
        {
            //var viewModel = new RanchManureViewModel
            //{
            //    Animals = _userData.GetAnimals()
            //};

            return View();
        }
    }
}
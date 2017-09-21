using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Models;
using SERVERAPI.Models.Impl;
using SERVERAPI.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERVERAPI.Controllers
{
    public class BaseController : Controller
    {
        public IHostingEnvironment _env { get; set; }
        public UserData _ud { get; set; }
        public Models.Impl.StaticData _sd { get; set; }
        public IViewRenderService _viewRenderService { get; set; }
        public AppSettings _settings;

        public BaseController()
        {
        }

        public List<string> GetChemicalBalanceMessages(string fldName)
        {
            ChemicalBalanceMessage cbm = new ChemicalBalanceMessage(_env, _ud, _sd);
            List<string> messages = null;

            cbm.balance_AgrN = 11;
            cbm.balance_AgrK2O = 22;
            cbm.balance_AgrP2O5 = 33;
            cbm.balance_CropK2O = 44;
            cbm.balance_CropP2O5 = 55;

            messages = cbm.DetermineBalanceMessages(fldName);

            return messages;
        }
    }
}

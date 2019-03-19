using Agri.Models.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVERAPI.ViewModels
{
    public class FarmIncompleteViewModel : FarmViewModelBase
    {
        public string Target { get; set; }
        public string Message { get; set; }
    }
}

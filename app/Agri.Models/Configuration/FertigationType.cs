using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class FertigationType : SelectOption
    {
        public string LiquidSolid { get; set; }
        public bool Custom { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERVERAPI.ViewModels
{
    public class AnimalDeleteViewModel
    {
        public int id { get; set; }
        public string subTypeName { get; set; }
        public string act { get; set; }
        public string target { get; set; }
    }
}
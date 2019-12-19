using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERVERAPI.ViewModels
{
    public class AnimalDeleteViewModel
    {
        public int Id { get; set; }
        public string SubTypeName { get; set; }
        public string Act { get; set; }
        public string Target { get; set; }
    }
}
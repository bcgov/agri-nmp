using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SERVERAPI.ViewModels
{
    public class ManureGeneratedObtainedDeleteViewModel
    {
        public string title { get; set; }
        public int id { get; set; }
        [Display(Name = "SubType")]
        public string subTypeName { get; set; }
        public string target { get; set; }
        public string warning { get; set; }
    }
}

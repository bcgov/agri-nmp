using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVERAPI.ViewModels
{
    public class ReportPrintViewModel
    {
        public string Header { get; set; }
        public string Body { get; set; }
        public string RepeatHeader { get; set; }
        public string RepeatFooter { get; set; }
    }
}
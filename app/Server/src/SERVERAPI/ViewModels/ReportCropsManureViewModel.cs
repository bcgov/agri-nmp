using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVERAPI.ViewModels
{
    public class ReportCropsManureViewModel
    {
        public List<ReportManures> Manures { get; set; } = new List<ReportManures>();
        public List<ReportFieldFootnote> Footnotes { get; set; } = new List<ReportFieldFootnote>();
    }
}
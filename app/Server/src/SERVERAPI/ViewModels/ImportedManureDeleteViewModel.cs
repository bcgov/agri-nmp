using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVERAPI.ViewModels
{
    public class ManureImportedDeleteViewModel
    {
        public string Title { get; set; }
        public int ImportedManureId { get; set; }
        public string manureId { get; set; }
        public string ImportManureName { get; set; }
        public string Target { get; set; }
        public bool AppliedToAField { get; set; }
        public string DeleteWarningForUnstorableMaterial { get; set; }
    }
}

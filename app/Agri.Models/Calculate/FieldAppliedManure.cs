using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri.Models.Calculate
{
    public class FieldAppliedManure
    {
        public int FieldId { get; set; }
        public decimal? TonsApplied { get; set; }
        public decimal? USGallonsApplied { get; set; }
    }
}
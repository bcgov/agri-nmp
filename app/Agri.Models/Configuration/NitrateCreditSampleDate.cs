using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri.Models.Configuration
{
    public class NitrateCreditSampleDate
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string FromDateMonth { get; set; }
        public string ToDateMonth { get; set; }
    }
}

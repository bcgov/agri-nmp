using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri.Models.Configuration
{
    public class ApplicationPerAcreConversion : ConfigurationBase
    {
        [Key]
        public int Id { get; set; }
        public ApplicationRateUnits ApplicationRateUnit { get; set; }
        public string ApplicationRateUnitName { get; set; }
    }
}

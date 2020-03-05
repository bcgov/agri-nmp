using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Agri.Models.Configuration
{
    public class ApplicationPerAcreConversion : Versionable
    {
        [Key]
        public int Id { get; set; }

        //[JsonIgnore]
        //[IgnoreDataMember]
        public ApplicationRateUnits ApplicationRateUnit { get; set; }

        public string ApplicationRateUnitName { get; set; }
    }
}
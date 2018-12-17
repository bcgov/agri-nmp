using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri.Models.Configuration
{
    public class PhosphorusSoilTestRange : SoilTestRange
    {
        [Key]
        public int Id { get; set; }
    }
}

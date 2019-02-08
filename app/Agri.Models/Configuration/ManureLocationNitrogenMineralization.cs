using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri.Models.Configuration
{
    public class ManureLocationNitrogenMineralization : ConfigurationBase
    {
        [Key]
        public int ManureId { get; set; }
        [Key]
        public int LocationId { get; set; }
        [Key]
        public int NitrogenMineralizationId { get; set; }

        public List<Manure> Manures { get; set; }
        public List<NitrogenMineralization> NitrogenMineralizations { get; set; }
    }
}

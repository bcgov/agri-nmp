using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class PreviousManureApplicationYear : ConfigurationBase
    {
        public PreviousManureApplicationYear()
        {
            PreviousYearManureApplicationNitrogenDefaults = new List<PreviousYearManureApplicationNitrogenDefault>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int FieldManureApplicationHistory { get; set; }

        public List<PreviousYearManureApplicationNitrogenDefault> PreviousYearManureApplicationNitrogenDefaults { get; set; }
    }
}
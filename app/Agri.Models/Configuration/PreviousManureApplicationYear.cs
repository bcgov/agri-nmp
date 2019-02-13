using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agri.Models.Configuration
{
    public class PreviousManureApplicationYear : ConfigurationBase
    {
        public PreviousManureApplicationYear()
        {
            PreviousYearManureApplicationNitrogenDefaults = new List<PreviousYearManureApplicationNitrogenDefault>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int FieldManureApplicationHistory { get; set; }

        public List<PreviousYearManureApplicationNitrogenDefault> PreviousYearManureApplicationNitrogenDefaults { get; set; }
    }
}
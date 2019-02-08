using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class PreviousYearManureApplicationNitrogenDefault : ConfigurationBase
    {
        public PreviousYearManureApplicationNitrogenDefault()
        {
            Crops = new List<Crop>();
        }

        [Key]
        public int Id { get; set; }
        public int FieldManureApplicationHistory { get; set; }
        public int[] DefaultNitrogenCredit { get; set; }
        public string PreviousYearManureAplicationFrequency { get; set; }

        public PreviousManureApplicationYear PreviousManureApplicationYear { get; set; }
        public List<Crop> Crops { get; set; }
    }
}
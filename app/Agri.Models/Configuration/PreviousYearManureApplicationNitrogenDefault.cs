using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agri.Models.Configuration
{
    public class PreviousYearManureApplicationNitrogenDefault
    {
        public PreviousYearManureApplicationNitrogenDefault()
        {
            Crops = new List<Crop>();
        }

        public int Id { get; set; }
        public int FieldManureApplicationHistory { get; set; }
        public int[] DefaultNitrogenCredit { get; set; }

        public PreviousManureApplicationYear PreviousManureApplicationYear { get; set; }
        public List<Crop> Crops { get; set; }
    }
}
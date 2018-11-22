using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agri.Models.Configuration
{
    public class PreviousManureApplicationYear
    {
        public PreviousManureApplicationYear()
        {
            PreviousYearManureApplicationNitrogenDefaults = new List<PreviousYearManureApplicationNitrogenDefault>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int FieldManureApplicationHistory { get; set; }

        public List<PreviousYearManureApplicationNitrogenDefault> PreviousYearManureApplicationNitrogenDefaults { get; set; }
    }
}
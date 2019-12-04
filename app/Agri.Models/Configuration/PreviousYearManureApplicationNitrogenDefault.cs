using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Agri.Models.Configuration
{
    public class PreviousYearManureApplicationNitrogenDefault : Versionable
    {
        public PreviousYearManureApplicationNitrogenDefault()
        {
            Crops = new List<Crop>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int FieldManureApplicationHistory { get; set; }
        public int[] DefaultNitrogenCredit { get; set; }
        public string PreviousYearManureAplicationFrequency { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public PreviousManureApplicationYear PreviousManureApplicationYear { get; set; }

        public List<Crop> Crops { get; set; }
    }
}
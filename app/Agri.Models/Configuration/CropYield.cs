using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Agri.Models.Configuration
{
    public class CropYield : Versionable
    {
        [Key]
        public int CropId { get; set; }

        [Key]
        public int LocationId { get; set; }

        public decimal? Amount { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public Crop Crop { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public Location Location { get; set; }
    }
}
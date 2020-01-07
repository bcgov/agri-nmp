using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Agri.Models.Configuration
{
    public class CropSoilTestPhosphorousRegion : Versionable
    {
        [Key]
        public int CropId { get; set; }

        [Key]
        public int SoilTestPhosphorousRegionCode { get; set; }

        public int? PhosphorousCropGroupRegionCode { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public Crop Crop { get; set; }
    }
}
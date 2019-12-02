using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Agri.Models.Configuration
{
    public class CropSoilTestPotassiumRegion : Versionable
    {
        [Key]
        public int CropId { get; set; }

        [Key]
        public int SoilTestPotassiumRegionCode { get; set; }

        public int? PotassiumCropGroupRegionCode { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public Crop Crop { get; set; }
    }
}
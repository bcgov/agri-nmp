using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Agri.Models.Configuration
{
    public class SoilTestPotassiumRecommendation : Versionable
    {
        [Key]
        public int SoilTestPotassiumKelownaRangeId { get; set; }

        [Key]
        public int SoilTestPotassiumRegionCode { get; set; }

        [Key]
        public int PotassiumCropGroupRegionCode { get; set; }

        public int K2ORecommendationKilogramPerHectare { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public SoilTestPotassiumKelownaRange SoilTestPotassiumKelownaRange { get; set; }
    }
}
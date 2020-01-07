using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Agri.Models.Configuration
{
    public class SoilTestPhosphorousRecommendation : Versionable
    {
        [Key]
        public int SoilTestPhosphorousKelownaRangeId { get; set; }

        [Key]
        public int SoilTestPhosphorousRegionCode { get; set; }

        [Key]
        public int PhosphorousCropGroupRegionCode { get; set; }

        public int P2O5RecommendationKilogramPerHectare { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public SoilTestPhosphorousKelownaRange SoilTestPhosphorousKelownaRange { get; set; }
    }
}
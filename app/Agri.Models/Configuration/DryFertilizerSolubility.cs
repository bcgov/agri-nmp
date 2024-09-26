using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Agri.Models.Configuration
{
    public class DryFertilizerSolubility : Versionable
    {
        [Key]
        public int Id { get; set; }

        public int FertilizerId { get; set; }
        public int SolubilityUnitId { get; set; }
        public decimal Value { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public Fertilizer Fertilizer { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public SolubilityUnit SolubilityUnit { get; set; }
    }
}
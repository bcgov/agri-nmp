using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Agri.Models.Configuration
{
    public class SubRegion : Versionable
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public int AnnualPrecipitation { get; set; }
        public int AnnualPrecipitationOctToMar { get; set; }
        public int RegionId { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public Region Region { get; set; }
    }
}
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Agri.Models.Configuration
{
    public class FeedForageType : Versionable
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public int FeedId { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public Feed Feed { get; set; }
    }
}
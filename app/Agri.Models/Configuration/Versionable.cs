using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Agri.Models.Configuration
{
    public class Versionable : IVersionable
    {
        [Key]
        public int StaticDataVersionId { get; private set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public StaticDataVersion Version { get; private set; }

        public void SetVersion(StaticDataVersion version)
        {
            StaticDataVersionId = version.Id;
            Version = version;
        }
    }
}
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class Versionable
    {
        [Key]
        public int StaticDataVersionId { get; private set; }
        public StaticDataVersion Version { get; private set; }

        public void SetVersion(StaticDataVersion version)
        {
            StaticDataVersionId = version.Id;
            Version = version;
        }
    }
}

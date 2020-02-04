using System.Collections.Generic;
using Newtonsoft.Json;
using System.Collections.Generic;
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
        public List<Feed> Feeds { get; set; }
    }
}
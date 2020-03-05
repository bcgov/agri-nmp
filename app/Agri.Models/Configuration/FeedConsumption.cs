using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class FeedConsumption : Versionable
    {
        [Key]
        public int Id { get; set; }

        public string Label { get; set; }
        public decimal Value { get; set; }
    }
}
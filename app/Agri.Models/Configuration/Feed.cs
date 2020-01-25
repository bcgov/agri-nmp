using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agri.Models.Configuration
{
    public class Feed : Versionable
    {
        public Feed()
        {
            FeedForageTypes = new List<FeedForageType>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [Column(TypeName = "decimal(16,4)")]
        public decimal? CPPercent { get; set; }

        [Column(TypeName = "decimal(16,4)")]
        public decimal? PhosphorousPercent { get; set; }

        [Column(TypeName = "decimal(16,4)")]
        public decimal? PotassiumPercent { get; set; }

        public List<FeedForageType> FeedForageTypes { get; set; }
    }
}
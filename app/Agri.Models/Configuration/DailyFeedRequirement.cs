using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class DailyFeedRequirement : Versionable
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public decimal? Value { get; set; }
    }
}
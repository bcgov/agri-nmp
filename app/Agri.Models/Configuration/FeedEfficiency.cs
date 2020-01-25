using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class FeedEfficiency : Versionable
    {
        [Key]
        public int Id { get; set; }

        public string AnimalType { get; set; }
        public decimal Nitrogen { get; set; }
        public decimal Phosphorous { get; set; }
        public decimal Potassium { get; set; }
    }
}
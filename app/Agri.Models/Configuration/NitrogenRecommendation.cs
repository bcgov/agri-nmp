using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class NitrogenRecommendation : ConfigurationBase
    {
        [Key]
        public int Id { get; set; }
        public string RecommendationDesc { get; set; }
    }
}
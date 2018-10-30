using System.ComponentModel.DataAnnotations;

namespace Agri.Models.StaticData
{
    public class AmmoniaRetention
    {
        [Key]
        public int SeasonApplicationId { get; set; }
        public int DM { get; set; }
        public decimal? Value { get; set; }
    }
}
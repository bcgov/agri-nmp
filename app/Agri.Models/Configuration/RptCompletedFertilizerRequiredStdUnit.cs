using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class RptCompletedFertilizerRequiredStdUnit : Versionable
    {
        [Key]
        public int Id { get; set; }
        public int SolidUnitId { get; set; }
        public int LiquidUnitId { get; set; }
    }
}

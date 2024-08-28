using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class FertilizerType : SelectOption
    {
        public string DryLiquid { get; set; }
        public bool Custom { get; set; }
    }
}
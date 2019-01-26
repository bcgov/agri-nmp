using System.ComponentModel.DataAnnotations.Schema;

namespace Agri.Models.Configuration
{
    public class Menu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        [NotMapped]
        public bool GreyOutText { get; set; }
    }
}
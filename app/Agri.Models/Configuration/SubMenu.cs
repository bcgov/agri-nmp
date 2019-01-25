using System.ComponentModel.DataAnnotations.Schema;

namespace Agri.Models.Configuration
{
    public class SubMenu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        [NotMapped]
        public bool GreyOutText { get; set; }
        public int MainMenuId { get; set; }
        public MainMenu MainMenu { get; set; }

        public bool IsSubMenuCurrent(string currentAction)
        {
            var isCurrent = Action.Equals(currentAction, StringComparison.CurrentCulture);
            return isCurrent;
        }
    }
}
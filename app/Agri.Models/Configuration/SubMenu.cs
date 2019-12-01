using System;

namespace Agri.Models.Configuration
{
    public class SubMenu : Menu
    {
        public int MainMenuId { get; set; }
        public MainMenu MainMenu { get; set; }

        public bool IsSubMenuCurrent(string currentAction)
        {
            var isCurrent = Action.Equals(currentAction, StringComparison.OrdinalIgnoreCase);
            return isCurrent;
        }
    }
}
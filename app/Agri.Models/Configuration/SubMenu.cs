using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agri.Models.Configuration
{
    public class SubMenu : Menu
    {
        public int MainMenuId { get; set; }
        public MainMenu MainMenu { get; set; }

        public bool IsSubMenuCurrent(string currentAction)
        {
            var isCurrent = Action.Equals(currentAction, StringComparison.CurrentCulture);
            return isCurrent;
        }
    }
}
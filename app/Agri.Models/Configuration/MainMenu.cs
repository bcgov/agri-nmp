using System;
using System.Collections.Generic;
using System.Linq;

namespace Agri.Models.Configuration
{
    public class MainMenu : Menu
    {
        public MainMenu()
        {
            SubMenus = new List<SubMenu>();
        }

        public int JourneyId { get; set; }
        public Journey Journey { get; set; }

        public List<SubMenu> SubMenus { get; set; }

        public bool IsCurrentMainMenu(string currentAction)
        {
            var submenu = SubMenus.SingleOrDefault(sm => sm.Action == currentAction);
            if (submenu != null)
            {
                return true;
            }
            var isCurrent = Action.Equals(currentAction, StringComparison.CurrentCulture);
            return isCurrent;
        }

        public bool IsCurrentMainMenu(CoreSiteActions currentAction)
        {
            return IsCurrentMainMenu(currentAction.ToString());
        }
    }
}
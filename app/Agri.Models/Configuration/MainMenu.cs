using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri.Models.Configuration
{
    public class MainMenu : Menu
    {
        public MainMenu()
        {
            SubMenus = new List<SubMenu>();
        }
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
    }
}

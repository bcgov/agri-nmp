using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri.Models.Configuration
{
    public class MainMenu
    {
        public MainMenu()
        {
            SubMenus = new List<SubMenu>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Controller { get; set; }

        public string Action { get; set; }
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

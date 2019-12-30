using Common;
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

        public bool IsCurrentMainMenu(string currentActionOrPage)
        {
            var submenu = SubMenus.
                SingleOrDefault(sm =>
                    (!string.IsNullOrEmpty(sm.Action) &&
                        sm.Action.Equals(currentActionOrPage, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(sm.Page) &&
                        sm.Page.Equals(currentActionOrPage, StringComparison.OrdinalIgnoreCase)));

            if (submenu != null)
            {
                return true;
            }

            var isCurrent =
                (!string.IsNullOrEmpty(Action) &&
                    Action.Equals(currentActionOrPage, StringComparison.CurrentCulture)) ||
                (!string.IsNullOrEmpty(Page) &&
                    Page.Equals(currentActionOrPage, StringComparison.OrdinalIgnoreCase));

            return isCurrent;
        }

        public bool IsCurrentMainMenu(CoreSiteActions currentAction)
        {
            return IsCurrentMainMenu(currentAction.ToString());
        }

        public bool IsCurrentMainMenu(FeaturePages currentPage)
        {
            return IsCurrentMainMenu(currentPage.GetDescription());
        }
    }
}
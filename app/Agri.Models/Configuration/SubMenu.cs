using Common;
using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Agri.Models.Configuration
{
    public class SubMenu : Menu
    {
        public int MainMenuId { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public MainMenu MainMenu { get; set; }

        public bool IsSubMenuCurrent(string currentActionorPage)
        {
            var isCurrent =
                (!string.IsNullOrEmpty(Action) && Action.Equals(currentActionorPage, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(Page) && Page.Equals(currentActionorPage, StringComparison.OrdinalIgnoreCase));
            return isCurrent;
        }

        public bool IsSubMenuCurrent(CoreSiteActions currentAction)
        {
            return IsSubMenuCurrent(currentAction.ToString());
        }

        public bool IsSubMenuCurrent(FeaturePages currentPage)
        {
            return IsSubMenuCurrent(currentPage.GetDescription());
        }
    }
}
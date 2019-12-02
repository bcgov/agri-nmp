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

        public bool IsSubMenuCurrent(string currentAction)
        {
            var isCurrent = Action.Equals(currentAction, StringComparison.CurrentCulture);
            return isCurrent;
        }
    }
}
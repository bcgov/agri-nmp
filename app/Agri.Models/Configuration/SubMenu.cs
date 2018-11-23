namespace Agri.Models.Configuration
{
    public class SubMenu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public int MainMenuId { get; set; }
        public MainMenu MainMenu { get; set; }
    }
}
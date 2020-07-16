using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri.Models.Configuration
{
    public class Journey
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<MainMenu> MainMenus { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class FertigationType : Versionable
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string LiquidSolid { get; set; }
        public bool Custom { get; set; }
    }
}
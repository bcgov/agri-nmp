using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class SelectOption : Versionable
    {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

    }
}
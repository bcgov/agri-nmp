using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class Version
    {
        public Version()
        {
            AmmoniaRetentions = new List<AmmoniaRetention>();
        }
        [Key]
        public int Id { get; set; }
        public string StaticDataVersion { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public List<AmmoniaRetention> AmmoniaRetentions { get; set; }
    }
}

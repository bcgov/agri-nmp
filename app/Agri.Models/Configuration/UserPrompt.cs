﻿using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class UserPrompt : ConfigurationBase
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
    }
}

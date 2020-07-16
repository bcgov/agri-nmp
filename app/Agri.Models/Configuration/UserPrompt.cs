﻿using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class UserPrompt
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Text { get; set; }
        public string UserPromptPage { get; set; }
        public string UserJourney { get; set; }
    }
}
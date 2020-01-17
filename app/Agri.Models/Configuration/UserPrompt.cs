using System.ComponentModel.DataAnnotations;

namespace Agri.Models.Configuration
{
    public class UserPrompt
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Text { get; set; }
        public string PageName { get; set; }
        public string UserJourneyName { get; set; }
        public UserPromptPage UserPromptPage { get; set; }
        public UserJourney UserJourney { get; set; }
    }
}
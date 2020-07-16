using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agri.Models.Configuration
{
    public class Menu
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Page { get; set; }
        public int SortNumber { get; set; }
        public bool UseJavaScriptInterceptMethod { get; set; }

        public string PreviousController { get; set; }
        public string PreviousAction { get; set; }
        public string NextController { get; set; }
        public string NextAction { get; set; }
        public string PreviousPage { get; set; }
        public string NextPage { get; set; }

        public bool UsesFeaturePages => !string.IsNullOrEmpty(Page);

        private string _elementId;

        [NotMapped]
        public string ElementId
        {
            get
            {
                if (!string.IsNullOrEmpty(_elementId))
                {
                    return _elementId;
                }
                return string.Empty;
            }
            set { _elementId = value; }
        }
    }
}
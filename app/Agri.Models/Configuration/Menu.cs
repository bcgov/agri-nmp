using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agri.Models.Configuration
{
    public class Menu : ConfigurationBase
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }

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

        private string _greyOutClass;
        [NotMapped]
        public string GreyOutClass
        {
            get
            {
                if (!string.IsNullOrEmpty(_greyOutClass))
                {
                    return _greyOutClass;
                }
                return string.Empty;
            }
            set { _greyOutClass = value; }
        }
    }
}
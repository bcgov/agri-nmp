namespace Agri.Models.Configuration
{
    public class SelectListItem
    {
        public SelectListItem()
        {
        }
        public SelectListItem(SelectOption selectOption)
        {
            Id = selectOption.Id;
            Value = selectOption.Name;

        }
        public int Id { get; set; }
        public string Value { get; set; }
    }
}
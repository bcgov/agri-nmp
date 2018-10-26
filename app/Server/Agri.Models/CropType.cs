namespace Agri.Models
{
    public class CropType
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool covercrop { get; set; }
        public bool crudeproteinrequired { get; set; }
        public bool customcrop { get; set; }
        public bool modifynitrogen { get; set; }
    }
}
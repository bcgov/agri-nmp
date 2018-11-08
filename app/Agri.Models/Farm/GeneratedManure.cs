using Agri.Models.Configuration;

namespace Agri.Models.Farm
{
    public class GeneratedManure
    {
        public int id { get; set; }
        public int animalId { get; set; }
        public int animalSubTypeId { get; set; }
        public int averageAnimalNumber { get; set; }
        public ManureMaterialType manureType { get; set; }
        public decimal washWaterGallons { get; set; }
    }
}
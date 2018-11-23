using Agri.Models.Configuration;

namespace Agri.Models.Farm
{
    public class GeneratedManure
    {
        public int? id { get; set; }
        public int animalId { get; set; }
        public string animalName { get; set; }
        public int animalSubTypeId { get; set; }
        public string animalSubTypeName { get; set; }
        public int averageAnimalNumber { get; set; }
        public ManureMaterialType manureType { get; set; }
        public string manureTypeName { get; set; }
        public string annualAmount { get; set; }
        public decimal washWaterGallons { get; set; }
        public decimal washWater { get; set; }
        public decimal milkProduction { get; set; }
        public bool AssignedToStoredSystem { get; set; }
    }
}
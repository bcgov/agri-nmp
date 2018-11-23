namespace Agri.Models.Configuration
{
    public class AnimalSubType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal LiquidPerGalPerAnimalPerDay { get; set; }
        public decimal SolidPerGalPerAnimalPerDay { get; set; }
        public decimal SolidPerPoundPerAnimalPerDay { get; set; }
        public decimal SolidLiquidSeparationPercentage { get; set; }
        public int AnimalId { get; set; }
        public Animal Animal { get; set; }
    }
}
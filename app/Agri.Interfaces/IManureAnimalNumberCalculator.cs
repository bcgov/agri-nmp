using Agri.Models.Calculate;

namespace Agri.Interfaces
{
    public interface IManureAnimalNumberCalculator
    {
        string CalculateAverageAnimalNumber(int milkingCowAnimalNumber, string subType);
    }
}
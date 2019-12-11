using Agri.Models;
using System;

namespace Agri.CalculateService
{
    public interface IManureAnimalNumberCalculator
    {
        string CalculateAverageAnimalNumber(int milkingCowAnimalNumber, string subType);
    }

    public class ManureAnimalNumberCalculator : IManureAnimalNumberCalculator
    {
        public ManureAnimalNumberCalculator()
        {
        }

        public string CalculateAverageAnimalNumber(int milkingCowAnimalNumber, string subType)
        {
            var placehldr = "e.g., ";
            if (subType == ((int)DairyCattleAnimalSubTypes.Calves0To3Months).ToString())
            {
                placehldr += Math.Round((milkingCowAnimalNumber * 0.10)).ToString();
            }
            else if (subType == ((int)DairyCattleAnimalSubTypes.Calves3To6Months).ToString())
            {
                placehldr += Math.Round((milkingCowAnimalNumber * 0.10)).ToString();
            }
            else if (subType == ((int)DairyCattleAnimalSubTypes.Heifers6To15Months).ToString())
            {
                placehldr += Math.Round((milkingCowAnimalNumber * 0.28)).ToString();
            }
            else if (subType == ((int)DairyCattleAnimalSubTypes.Heifers15To26Months).ToString())
            {
                placehldr += Math.Round((milkingCowAnimalNumber * 0.33)).ToString();
            }
            else if (subType == ((int)DairyCattleAnimalSubTypes.DryCows).ToString())
            {
                placehldr += Math.Round((milkingCowAnimalNumber * 0.20)).ToString();
            }

            return placehldr;
        }
    }
}
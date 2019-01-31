using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agri.Interfaces;
using Agri.Models;
using Agri.Models.Calculate;
using Agri.Models.Farm;

namespace Agri.CalculateService
{
    public class ManureAnimalNumberCalculator : IManureAnimalNumberCalculator
    {
        public ManureAnimalNumberCalculator()
        {
        }

        public string CalculateAverageAnimalNumber(int milkingCowAnimalNumber, string subType)
        {
            var placehldr = "";
            if (subType == ((int)DairyCattleAnimalSubTypes.Calves0To3Months).ToString())
            {
                placehldr = Math.Round((milkingCowAnimalNumber * 0.10)).ToString();
            }
            else if (subType == ((int)DairyCattleAnimalSubTypes.Calves3To6Months).ToString())
            {
                placehldr = Math.Round((milkingCowAnimalNumber * 0.10)).ToString();
            }
            else if (subType == ((int)DairyCattleAnimalSubTypes.Heifers6To15Months).ToString())
            {
                placehldr = Math.Round((milkingCowAnimalNumber * 0.28)).ToString();
            }
            else if (subType == ((int)DairyCattleAnimalSubTypes.Heifers15To26Months).ToString())
            {
                placehldr = Math.Round((milkingCowAnimalNumber * 0.33)).ToString();
            }
            else if (subType == ((int)DairyCattleAnimalSubTypes.DryCows).ToString())
            {
                placehldr = Math.Round((milkingCowAnimalNumber * 0.20)).ToString();
            }

            return placehldr ;
        }
    }
}

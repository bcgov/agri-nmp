using Agri.Data;
using Agri.Models;
using Agri.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agri.CalculateService
{
    public interface ICalculateManureGeneration
    {
        int GetSolidTonsGeneratedForAnimalSubType(int animalSubTypeId, int animalCount, int daysCollected);
    }

    public class CalculateManureGeneration : ICalculateManureGeneration
    {
        private readonly IAgriConfigurationRepository _sd;

        public CalculateManureGeneration(IAgriConfigurationRepository sd)
        {
            _sd = sd;
        }

        public int GetSolidTonsGeneratedForAnimalSubType(int animalSubTypeId,
            int animalCount,
            int daysCollected)
        {
            var poundPerDay = _sd.GetAnimalSubType(animalSubTypeId).SolidPerPoundPerAnimalPerDay;

            var result = (animalCount * poundPerDay * daysCollected / 2000);

            return Convert.ToInt32(result ?? 0);
        }
    }
}
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

        int GetTonsGeneratedForPoultrySubType(int animalSubTypeId, int birdsPerFlock, decimal flocksPerYear, int daysPerFlock);

        int GetGallonsGeneratedForAnimalSubType(int animalSubTypeId, int animalCount, int daysCollected);

        int GetGallonsGeneratedForPoultrySubType(int animalSubTypeId, int birdsPerFlock, decimal flocksPerYear, int daysPerFlock);
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

        public int GetTonsGeneratedForPoultrySubType(int animalSubTypeId, int birdsPerFlock, decimal flocksPerYear, int daysPerFlock)
        {
            var poundsPerDay = _sd.GetAnimalSubType(animalSubTypeId).SolidPerPoundPerAnimalPerDay;

            var result = poundsPerDay * birdsPerFlock * flocksPerYear * daysPerFlock / 2000;

            return Convert.ToInt32(result ?? 0);
        }

        public int GetGallonsGeneratedForPoultrySubType(int animalSubTypeId, int birdsPerFlock, decimal flocksPerYear, int daysPerFlock)
        {
            var gallonsPerDay = _sd.GetAnimalSubType(animalSubTypeId).LiquidPerGalPerAnimalPerDay;

            var result = gallonsPerDay * birdsPerFlock * flocksPerYear * daysPerFlock;

            return Convert.ToInt32(result ?? 0);
        }

        public int GetGallonsGeneratedForAnimalSubType(int animalSubTypeId, int animalCount, int daysCollected)
        {
            var gallonsPerDay = _sd.GetAnimalSubType(animalSubTypeId).LiquidPerGalPerAnimalPerDay;

            var result = gallonsPerDay * animalCount * daysCollected;

            return Convert.ToInt32(result ?? 0);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agri.Interfaces;
using Agri.Models;
using Agri.Models.Configuration;

namespace Agri.CalculateService
{
    public class StorageVolumeCalculator : IStorageVolumeCalculator
    {
        private IAgriConfigurationRepository _repository;

        public StorageVolumeCalculator(IAgriConfigurationRepository repository)
        {
            _repository = repository;
        }

        public int GetSurfaceAreaOfRectangle(decimal length, decimal width, decimal height)
        {
            var surfaceArea = (int)Math.Round(length * width);
            return surfaceArea;
        }
        public int GetSurfaceAreaOfCircle(decimal diameter)
        {
            var surfaceArea = (int)Math.Round((22 / 7) * Math.Pow(Convert.ToDouble(diameter) / 2, 2));
            return surfaceArea;
        }
        public int GetSurfaceAreaOfSlopedWall(decimal topLength, decimal topWidth)
        {
            var surfaceArea = (int)Math.Round(topLength * topWidth);
            return surfaceArea;
        }
    }
}

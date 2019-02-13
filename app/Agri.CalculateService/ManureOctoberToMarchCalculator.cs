using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agri.Interfaces;
using Agri.Models;
using Agri.Models.Calculate;

namespace Agri.CalculateService
{
    public class ManureOctoberToMarchCalculator : IManureOctoberToMarchCalculator
    {
        public decimal CalculateOctoberToMarchSeparatedLiquid(decimal separatedLiquidsUSGallons)
        {
            var daysInOctoberToMarch = 180;
            var totalDays = 365;
            var octoberToMarchSeparatedLiquid = Math.Round((separatedLiquidsUSGallons / totalDays) * daysInOctoberToMarch);
            return octoberToMarchSeparatedLiquid;
        }
    }
}

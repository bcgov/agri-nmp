using System;

namespace Agri.CalculateService
{
    public interface IManureOctoberToMarchCalculator
    {
        decimal CalculateOctoberToMarchSeparatedLiquid(decimal separatedLiquidsUSGallons);
    }

    public class ManureOctoberToMarchCalculator : IManureOctoberToMarchCalculator
    {
        public decimal CalculateOctoberToMarchSeparatedLiquid(decimal separatedLiquidsUSGallons)
        {
            var daysInOctoberToMarch = 182;
            var totalDays = 365;
            var octoberToMarchSeparatedLiquid = Math.Round((separatedLiquidsUSGallons / totalDays) * daysInOctoberToMarch);
            return octoberToMarchSeparatedLiquid;
        }
    }
}
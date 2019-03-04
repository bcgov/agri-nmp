using Agri.Models.Calculate;
using Agri.Models.Configuration;

namespace Agri.Interfaces
{
    public interface IManureOctoberToMarchCalculator
    {
        decimal CalculateOctoberToMarchSeparatedLiquid(decimal separatedLiquidsUSGallons);
    }
}
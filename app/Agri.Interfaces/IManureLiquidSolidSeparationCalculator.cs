using Agri.Models.Calculate;

namespace Agri.Interfaces
{
    public interface IManureLiquidSolidSeparationCalculator
    {
        SeparatedManure CalculateSeparatedManure(int liquidVolumeGallons, int wholePercentLiquidSeparated);
    }
}
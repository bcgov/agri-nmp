using Agri.Models.Calculate;

namespace Agri.Interfaces
{
    public interface IManureLiquidSolidSeparationCalculator
    {
        SeparatedManure CalculateSeparatedManure(decimal liquidVolumeGallons, int wholePercentLiquidSeparated);
    }
}
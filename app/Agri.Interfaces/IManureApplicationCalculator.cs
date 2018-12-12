using Agri.Models.Calculate;
using Agri.Models.Farm;

namespace Agri.Interfaces
{
    public interface IManureApplicationCalculator
    {
        AppliedStoredManure GetAppliedStoredManure(YearData yearData, int manureStorageSystemId);
        AppliedImportedManure GetAppliedImportedManure(YearData yearData, int importedManureId);
    }
}
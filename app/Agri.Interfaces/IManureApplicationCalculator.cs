using Agri.Models.Calculate;
using Agri.Models.Farm;

namespace Agri.Interfaces
{
    public interface IManureApplicationCalculator
    {
        AppliedManure GetAppliedManure(YearData yearData, FarmManure farmManure);
        AppliedStoredManure GetAppliedManureFromStorageSystem(YearData yearData, ManureStorageSystem manureStorageSystem);
        AppliedStoredManure GetAppliedStoredManure(YearData yearData, string managedManureId);
        AppliedImportedManure GetAppliedImportedManure(YearData yearData, string managedManureId);
    }
}
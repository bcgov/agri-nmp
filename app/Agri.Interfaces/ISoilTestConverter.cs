using Agri.Models.Farm;

namespace Agri.Interfaces
{
    public interface ISoilTestConverter
    {
        int GetConvertedSTK(string testingMethod, SoilTest soilTest);
        int GetConvertedSTP(string testingMethod, SoilTest soilTest);
    }
}
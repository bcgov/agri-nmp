namespace Agri.Models.Configuration
{
    public interface IVersionable
    {
        int StaticDataVersionId { get; }
        StaticDataVersion Version { get; }

        void SetVersion(StaticDataVersion version);
    }
}
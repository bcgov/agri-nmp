namespace Agri.Models.Farm
{
    public abstract class ManagedManure
    {
        public int? Id { get; set; }
        public ManureMaterialType ManureType { get; set; }
        public bool AssignedToStoredSystem { get; set; }
        public abstract string ManureId { get; }
    }
}

namespace Agri.Models.Farm
{
    public abstract class ManagedManure
    {
        public int? Id { get; set; }
        public virtual ManureMaterialType ManureType { get; set; }
        public virtual bool AssignedToStoredSystem { get; set; }
        public abstract string ManureId { get; }
        public abstract string ManagedManureName { get; }
        public bool AssignedWithNutrientAnalysis { get; set; }
    }
}
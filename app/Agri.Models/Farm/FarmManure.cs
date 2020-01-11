using System;

namespace Agri.Models.Farm
{
    public class FarmManure
    {
        public int Id { get; set; }
        public bool Customized { get; set; }
        public string SourceOfMaterialId { get; set; }

        public int? SourceOfMaterialStoredSystemId =>
            StoredImported == NutrientAnalysisTypes.Stored
                ? Convert.ToInt32(SourceOfMaterialId.Split(",")[1])
                : new int?();

        public int? SourceOfMaterialImportedManureId =>
            StoredImported == NutrientAnalysisTypes.Imported
                ? Convert.ToInt32(SourceOfMaterialId.Split(",")[1])
                : new int?();

        public string SourceOfMaterialName { get; set; }
        public int ManureId { get; set; }
        public string Name { get; set; }
        public string ManureClass { get; set; }
        public string SolidLiquid { get; set; }
        public string Moisture { get; set; }
        public decimal Nitrogen { get; set; }
        public decimal Ammonia { get; set; }
        public decimal Phosphorous { get; set; }
        public decimal Potassium { get; set; }
        public int DMId { get; set; }
        public int NMinerizationId { get; set; }
        public decimal? Nitrate { get; set; }
        public NutrientAnalysisTypes StoredImported { get; set; }
        public bool IsAssignedToStorage { get; set; }
    }
}
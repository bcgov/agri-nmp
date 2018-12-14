using System;

namespace Agri.Models.Farm
{
    public class FarmManure
    {
        public int id { get; set; }
        public bool customized { get; set; }
        public string sourceOfMaterialId { get; set; }
        public int? sourceOfMaterialGeneratedManureId =>
            sourceOfMaterialId.Split(",")[0].Contains("Generated", StringComparison.CurrentCultureIgnoreCase)
                ? Convert.ToInt32(sourceOfMaterialId.Split(",")[1])
                : new int?();
        public int? sourceOfMaterialImportedManureId =>
            sourceOfMaterialId.Split(",")[0].Contains("Imported", StringComparison.CurrentCultureIgnoreCase)
                ? Convert.ToInt32(sourceOfMaterialId.Split(",")[1])
                : new int?();

        public string managedManureId => sourceOfMaterialId.Split(",")[0];
        public string sourceOfMaterialName { get; set; }
        public int manureId { get; set; }
        public string name { get; set; }
        public string manure_class { get; set; }
        public string solid_liquid { get; set; }
        public string moisture { get; set; }
        public decimal nitrogen { get; set; }
        public decimal ammonia { get; set; }
        public decimal phosphorous { get; set; }
        public decimal potassium { get; set; }
        public int dmid { get; set; }
        public int nminerizationid { get; set; }
        public decimal? nitrate { get; set; }
        public NutrientAnalysisTypes stored_imported { get; set; }
    }
}
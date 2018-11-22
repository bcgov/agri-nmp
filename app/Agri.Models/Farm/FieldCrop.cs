namespace Agri.Models.Farm
{
    public class FieldCrop
    {
        public int id { get; set; }
        public string cropId { get; set; }
        public string cropOther { get; set; }
        public decimal yield { get; set; } // tons/acre
        public decimal reqN { get; set; }
        public decimal stdN { get; set; }
        public decimal reqP2o5 { get; set; }
        public decimal reqK2o { get; set; }
        public decimal remN { get; set; }
        public decimal remP2o5 { get; set; }
        public decimal remK2o { get; set; }
        public decimal? crudeProtien { get; set; }
        public int prevCropId { get; set; }
        public bool? coverCropHarvested { get; set; }
        public int prevYearManureAppl_volCatCd { get; set; }
        public int? yieldHarvestUnit { get; set; }
        public decimal yieldByHarvestUnit { get; set; }
    }
}
namespace Agri.Models.Farm
{
    public class RancherFieldCrop
    {
        public int Id { get; set; }
        public string CropId { get; set; }
        public string CropOther { get; set; }
        public decimal Field { get; set; } // tons/acre
        public decimal ReqN { get; set; }
        public decimal StdN { get; set; }
        public decimal ReqP2o5 { get; set; }
        public decimal ReqK2o { get; set; }
        public decimal RemN { get; set; }
        public decimal RemP2o5 { get; set; }
        public decimal RemK2o { get; set; }
        public decimal? CrudeProtien { get; set; }
        public int PrevCropId { get; set; }
        public bool? CoverCropHarvested { get; set; }
        public int PrevYearManureAppl_volCatCd { get; set; }
        public int? YieldHarvestUnit { get; set; }
        public decimal YieldByHarvestUnit { get; set; }
    }
}
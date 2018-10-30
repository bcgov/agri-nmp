namespace Agri.Models.StaticData
{
    public class PrevCropType
    {
        public int Id { get; set; }
        public int PrevCropCd { get; set; }
        public string Name { get; set; }
        public int nCreditMetric { get; set; }
        public int nCreditImperial { get; set; }

        public PrevCropType CropType { get; set; }
    }
}
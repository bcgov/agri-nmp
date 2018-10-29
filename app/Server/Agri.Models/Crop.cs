using System.Collections.Generic;

namespace Agri.Models
{
    public class Crop
    {
        public int Id { get; set; }
        public string CropName { get; set; }
        public int CropTypeId { get; set; }
        public int YieldCd { get; set; }
        public decimal? CropRemovalFactor_N { get; set; }
        public decimal? CropRemovalFactorP2O5 { get; set; }
        public decimal? CropRemovalFactorK2O { get; set; }
        public decimal N_RecommCd { get; set; }
        public decimal? N_Recomm_lbPerAc { get; set; }
        public decimal? N_High_lbPerAc { get; set; }
        public int PrevCropCd { get; set; }
        public int SortNum { get; set; }
        public int PrevYearManureAppl_VolCatCd { get; set; }
        public decimal? HarvestBushelsPerTon { get; set; }

        public CropType CropType { get; set; }
        public List<CropYield> CropYields { get; set; }
        public List<CropSTKRegionCd> CropSTKRegionCds { get; set; }
        public List<CropSTPRegionCd> CropSTPRegionCds { get; set; }
    }
}
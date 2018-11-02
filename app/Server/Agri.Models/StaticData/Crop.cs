using System.Collections.Generic;

namespace Agri.Models.StaticData
{
    public class Crop
    {
        public Crop()
        {
            CropYields = new List<CropYield>();
            CropSoilTestPotassiumRegions = new List<CropSoilTestPotassiumRegion>();
            CropSoilTestPhosphorousRegions = new List<CropSoilTestPhosphorousRegion>();
            PreviousCropTypes = new List<PreviousCropType>();
        }
        public int Id { get; set; }
        public string CropName { get; set; }
        public int CropTypeId { get; set; }
        public int YieldCd { get; set; }
        public decimal? CropRemovalFactorNitrogen { get; set; }
        public decimal? CropRemovalFactorP2O5 { get; set; }
        public decimal? CropRemovalFactorK2O { get; set; }
        public decimal NitrogenRecommendationId { get; set; }
        public decimal? NitrogenRecommendationPoundPerAcre { get; set; }
        public decimal? NitrogenRecommendationUpperLimitPoundPerAcre { get; set; } //Upper limit for Nitrogen Recommendation
        public int PreviousCropCode { get; set; }
        public int SortNumber { get; set; }
        public int ManureApplicationHistory { get; set; }   //was PrevYearManureAppl_VolCatCd
        public decimal? HarvestBushelsPerTon { get; set; }

        public CropType CropType { get; set; }
        public List<CropYield> CropYields { get; set; }
        public List<CropSoilTestPotassiumRegion> CropSoilTestPotassiumRegions { get; set; }
        public List<CropSoilTestPhosphorousRegion> CropSoilTestPhosphorousRegions { get; set; }
        public PreviousYearManureApplicationNitrogenDefault PreviousYearManureApplicationNitrogenDefault { get; set; }
        public List<PreviousCropType> PreviousCropTypes { get; set; }
    }
}
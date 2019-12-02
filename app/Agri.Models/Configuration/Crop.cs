using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Agri.Models.Configuration
{
    public class Crop : Versionable
    {
        public Crop()
        {
            CropYields = new List<CropYield>();
            CropSoilTestPotassiumRegions = new List<CropSoilTestPotassiumRegion>();
            CropSoilTestPhosphorousRegions = new List<CropSoilTestPhosphorousRegion>();
            PreviousCropTypes = new List<PreviousCropType>();
        }

        [Key]
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

        [JsonIgnore]
        [IgnoreDataMember]
        public CropType CropType { get; set; }

        public List<CropYield> CropYields { get; set; }
        public List<CropSoilTestPotassiumRegion> CropSoilTestPotassiumRegions { get; set; }
        public List<CropSoilTestPhosphorousRegion> CropSoilTestPhosphorousRegions { get; set; }
        public PreviousYearManureApplicationNitrogenDefault PreviousYearManureApplicationNitrogenDefault { get; set; }
        public List<PreviousCropType> PreviousCropTypes { get; set; }
    }
}
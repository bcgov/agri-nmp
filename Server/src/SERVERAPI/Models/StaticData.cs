using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERVERAPI.Models
{
    public class StaticData
    {
        public class Version
        {
            public string staticDataVersion { get; set; }            
        }

        public class ConversionFactor
        {
            public decimal n_protein_conversion { get; set; }
            public decimal unit_conversion { get; set; }
            public int defaultSoilTestKelownaP { get; set; }
            public int defaultSoilTestKelownaK { get; set; }
            public decimal kgperha_lbperac_conversion { get; set; }
            public decimal potassiumAvailabilityFirstYear { get; set; }
            public decimal potassiumAvailabilityLongTerm { get; set; }
            public decimal potassiumKtoK2Oconversion { get; set; }
            public decimal phosphorousAvailabilityFirstYear { get; set; }
            public decimal phosphorousAvailabilityLongTerm { get; set; }
            public decimal phosphorousPtoP2O5Kconversion { get; set; }
            public decimal lbPerTonConversion { get; set; }            
        }

        public class Regions
        {
            public List<Region> regions { get; set; }
        }

        public class Region
        {
            public int id { get; set; }
            public string name { get; set; }            
            public int soil_test_phospherous_region_cd { get; set; }
            public int soil_test_potassium_region_cd { get; set; }
            public int locationid { get; set; }
            public int sortNum { get; set; }
        }

        public class SelectListItem
        {
            public int Id { get; set; }
            public string Value { get; set; }
        }

        public class SelectCodeItem
        {
            public string Cd { get; set; }
            public string Value { get; set; }
        }

        public class Locations
        {
            public List<Location> locations { get; set; }
        }

        public class Location
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public class Manures
        {
            public List<Manure> manures { get; set; }
        }

        public class Manure
        {
            public int id { get; set; }
            public string name { get; set; }
            public string manure_class { get; set; }
            public string solid_liquid { get; set; }
            public string moisture { get; set; }
            public decimal nitrogen { get; set; }
            public int ammonia { get; set; }
            public decimal phosphorous { get; set; }
            public decimal potassium { get; set; }
            public int dmid { get; set; }
            public int nminerizationid { get; set; }
            public int sortNum { get; set; }
        }
        public class FertilizerTypes
        {
            public List<FertilizerType> fertilizerTypes { get; set; }
        }

        public class FertilizerType
        {
            public int id { get; set; }
            public string name { get; set; }
            public string dry_liquid { get; set; }
            public bool custom { get; set; }
        }

        public class Fertilizers
        {
            public List<Fertilizer> fertilizers { get; set; }
        }

        public class Fertilizer
        {
            public int id { get; set; }
            public string name { get; set; }
            public string dry_liquid { get; set; }
            public decimal nitrogen { get; set; }
            public decimal phosphorous { get; set; }
            public decimal potassium { get; set; }
            public int sortNum { get; set; }
        }

        public class LiquidFertilizerDensity
        {
            public int id { get; set; }
            public int fertilizerId { get; set; }
            public int densityUnitId { get; set; }
            public decimal value { get; set; }
        }

        public class Season_Applications
        {
            public List<Season_Application> season_applications { get; set; }
        }

        public class Season_Application
        {
            public int id { get; set; }
            public string name { get; set; }
            public string season { get; set; }
            public string application_method { get; set; }
            public string moisture { get; set; }
            public decimal dm_lt1 { get; set; }
            public decimal dm_1_5 { get; set; }
            public decimal dm_5_10 { get; set; }
            public decimal dm_gt10 { get; set; }            
            public string poultry_solid { get; set; }
            public string compost { get; set; }
            public int sortNum { get; set; }
            public string manure_type { get; set; }
        }

        public class Units
        {
            public List<Unit> units { get; set; }
        }

        public class Unit
        {
            public int id { get; set; }
            public string name { get; set; }
            public string nutrient_content_units { get; set; }
            public decimal conversion_lbton { get; set; }
            public string nutrient_rate_units { get; set; }
            public string cost_units { get; set; }
            public decimal cost_applications { get; set; }
            public string dollar_unit_area { get; set; }
            public string value_material_units { get; set; }
            public decimal value_N { get; set; }
            public decimal value_P2O5 { get; set; }
            public decimal value_K2O { get; set; }
            public string solid_liquid { get; set; }
        }
        public class FertilizerUnits
        {
            public List<FertilizerUnit> fertilizerUnits { get; set; }
        }

        public class FertilizerUnit
        {
            public int id { get; set; }
            public string name { get; set; }
            public string dry_liquid { get; set; }
            public decimal conv_to_impgalperac { get; set; }
        }

        public class DensityUnits
        {
            public List<DensityUnit> densityUnits { get; set; }
        }

        public class DensityUnit
        {
            public int id { get; set; }
            public string name { get; set; }
            public decimal convfactor { get; set; }
        }

        public class CropTypes
        {
            public List<CropType> cropTypes { get; set; }
        }

        public class CropType
        {
            public int id { get; set; }
            public string name { get; set; }
            public bool covercrop { get; set; }
            public bool crudeproteinrequired { get; set; }
            public bool customcrop { get; set; }
            public bool modifynitrogen { get; set; }
        }

        public class Crops
        {
            public List<Crop> crops { get; set; }
        }

        public class Crop
        {
            public int id { get; set; }
            public string cropname { get; set; }
            public int croptypeid { get; set; }
            public int yieldcd { get; set; }
            public decimal? cropremovalfactor_N { get; set; }
            public decimal? cropremovalfactor_P2O5 { get; set; }
            public decimal? cropremovalfactor_K2O { get; set; }
            public decimal n_recommcd { get; set; }
            public decimal? n_recomm_lbperac { get; set; }
            public decimal? n_high_lbperac { get; set; }
            public int prevcropcd { get; set; }
            public int sortNum { get; set; }
        }

        public class Yield
        {
            public int id { get; set; }
            public string yielddesc { get; set; }
        }

        public class DM
        {
            public int ID { get; set; }
            public string name { get; set; }            
        }

        public class AmmoniaRetention
        {
            public int seasonapplicatonid { get; set; }
            public int dm { get; set; }
            public decimal? value { get; set; }
        }

        public class NMineralization
        {
            public int id { get; set; }
            public string name { get; set; }
            public int locationid { get; set; }
            public decimal firstyearvalue { get; set; }
            public decimal longtermvalue { get; set; }
        }

        public class SoilTestMethods
        {
            public List<SoilTestMethod> methods { get; set; }
        }
        public class SoilTestMethod
        {
            public int id { get; set; }
            public string name { get; set; }
            public decimal ConvertToKelownaPlt72 { get; set; }
            public decimal ConvertToKelownaPge72 { get; set; }
            public decimal ConvertToKelownaK { get; set; }
            public int sortNum { get; set; }
        }

        public class PrevCropTypes
        {
            public List<PrevCropType> prevCropTypes { get; set; }
        }
        public class PrevCropType
        {
            public int id { get; set; }
            public int prevcropcd { get; set; }
            public string name { get; set; }
            public int nCreditMetric { get; set; }
            public int nCreditImperial { get; set; }
        }

        public class CropYields
        {
            public List<CropYield> cropYields { get; set; }
        }
        public class CropYield
        {
            public int cropid { get; set; }
            public int locationid { get; set; }
            public decimal? amt { get; set; }
        }

        public class CropSTPRegionCds
        {
            public List<CropSTPRegionCd> cropSTPRegionCds { get; set; }
        }
        public class CropSTPRegionCd
        {
            public int cropid { get; set; }
            public int soil_test_phosphorous_region_cd { get; set; }
            public int? phosphorous_crop_group_region_cd { get; set; }
        }

        public class CropSTKRegionCds
        {
            public List<CropSTKRegionCd> cropSTKRegionCds { get; set; }
        }
        public class CropSTKRegionCd
        {
            public int cropid { get; set; }
            public int soil_test_potassium_region_cd { get; set; }
            public int? potassium_crop_group_region_cd { get; set; }
        }

        public class STPRecommends
        {
            public List<STPRecommend> sTPRecommends { get; set; }
        }
        public class STPRecommend
        {
            public int stp_kelowna_rangeid { get; set; }
            public int soil_test_phosphorous_region_cd { get; set; }
            public int phosphorous_crop_group_region_cd { get; set; }
            public int p2o5_recommend_kgperha { get; set; }
        }

        public class STPKelownaRanges
        {
            public List<STPKelownaRange> sTPKelownaRanges { get; set; }
        }
        public class STPKelownaRange
        {
            public int id { get; set; }
            public string range { get; set; }
            public int range_low { get; set; }
            public int range_high { get; set; }
        }

        public class STKRecommends
        {
            public List<STKRecommend> sTKRecommends { get; set; }
        }
        public class STKRecommend
        {
            public int stk_kelowna_rangeid { get; set; }
            public int soil_test_potassium_region_cd { get; set; }
            public int potassium_crop_group_region_cd { get; set; }
            public int k2o_recommend_kgperha { get; set; }
        }

        public class STKKelownaRanges
        {
            public List<STKKelownaRange> sTKKelownaRanges { get; set; }
        }
        public class STKKelownaRange
        {
            public int id { get; set; }
            public string range { get; set; }
            public int range_low { get; set; }
            public int range_high { get; set; }
        }

        public class Messages
        {
            public List<Message> messages { get; set; }
        }
        public class Message
        {
            public int id { get; set; }
            public string text { get; set; }
            public string displayMessage { get; set; }
            public string icon { get; set; }
            public string balanceType { get; set; }
            public int balance_low { get; set; }
            public int balance_high { get; set; }
            public decimal soiltest_low { get; set; }
            public decimal soiltest_high { get; set; }
            public int balance1_low { get; set; }
            public int balance1_high { get; set; }
        }

        public class DefaultSoilTest
        {
            public decimal nitrogen { get; set; }
            public decimal phosphorous { get; set; }
            public decimal potassium { get; set; }
            public decimal pH { get; set; }
            public int convertedKelownaP { get; set; }
            public int convertedKelownaK { get; set; }
        }

        public class SoilTestRange
        {
            public int upperLimit { get; set; }
            public string rating  { get; set; }
        }

        public class FertilizerMethods
        {
            public List<FertilizerMethod> fertilizerMethods { get; set; }
        }
        public class FertilizerMethod
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public class ExternalLinks
        {
            public List<ExternalLink> externalLinks { get; set; }
        }
        public class ExternalLink
        {
            public int id { get; set; }
            public string name { get; set; }
            public string url { get; set; }
        }
    }
}

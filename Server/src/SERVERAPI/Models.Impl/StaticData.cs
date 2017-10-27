﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SERVERAPI.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.IO;
using System.Text;
using static SERVERAPI.Models.StaticData;

namespace SERVERAPI.Models.Impl
{
    public partial class StaticData
    {
        private readonly IHttpContextAccessor _ctx;
        private JObject rss;
        //Constants
        //public decimal CONST_N_PROTIEN_CONVERSION;

        public StaticData(IHttpContextAccessor ctx)
        {
            _ctx = ctx;
            var assembly = Assembly.GetEntryAssembly();
            var resourceStream = assembly.GetManifestResourceStream("SERVERAPI.Data.static.json");
            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                rss = JObject.Parse(reader.ReadToEnd());
            }
        }

        public Models.StaticData.Regions GetRegions()
        {
            Models.StaticData.Regions regs = new Models.StaticData.Regions();
            regs.regions = new List<Models.StaticData.Region>();

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray regions = (JArray)rss["agri"]["nmp"]["regions"]["region"];

            foreach (var r in regions)
            {
                Models.StaticData.Region reg = new Models.StaticData.Region();

                reg.id = Convert.ToInt32(r["id"].ToString());
                reg.name = r["name"].ToString();
                reg.locationid = Convert.ToInt32(r["locationid"].ToString());
                reg.soil_test_phospherous_region_cd = Convert.ToInt32(r["soil_test_phospherous_region_cd"].ToString());
                reg.soil_test_potassium_region_cd = Convert.ToInt32(r["soil_test_potassium_region_cd"].ToString());
                regs.regions.Add(reg);
            }

            return regs;
        }

        public List<Models.StaticData.SelectListItem> GetRegionsDll()
        {
            Models.StaticData.Regions regs = GetRegions();

            List<Models.StaticData.SelectListItem> regOptions = new List<Models.StaticData.SelectListItem>();

            foreach (var r in regs.regions)
            {
                Models.StaticData.SelectListItem li = new Models.StaticData.SelectListItem() { Id = r.id, Value = r.name };
                regOptions.Add(li);
            }

            return regOptions;
        }

        public Models.StaticData.Manure GetManure(string manId)
        {
            Models.StaticData.Manure man = new Models.StaticData.Manure();

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray manures = (JArray)rss["agri"]["nmp"]["manures"]["manure"];

            foreach (var r in manures)
            {
                if (r["id"].ToString() == manId)
                {
                    man.id = Convert.ToInt32(r["id"].ToString());
                    man.name = r["name"].ToString();
                    man.manure_class = r["manure_class"].ToString();
                    man.solid_liquid = r["solid_liquid"].ToString();
                    man.moisture = r["moisture"].ToString();
                    man.nitrogen = Convert.ToDecimal(r["nitrogen"].ToString());
                    man.ammonia = Convert.ToInt32(r["ammonia"].ToString());
                    man.phosphorous = Convert.ToDecimal(r["phosphorous"].ToString());
                    man.potassium = Convert.ToDecimal(r["potassium"].ToString());
                    man.dmid = Convert.ToInt32(r["dmid"].ToString());
                    man.nminerizationid = Convert.ToInt32(r["nminerizationid"].ToString());
                }
            }

            return man;
        }

        public Models.StaticData.Manure GetManureByName(string manureName)
        {
            Models.StaticData.Manure man = null;

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray manures = (JArray)rss["agri"]["nmp"]["manures"]["manure"];
            JObject r = manures.Children<JObject>().FirstOrDefault(o => o["name"].ToString() == manureName.Trim());

            if (r != null)
            {
                man = new Manure();
                man.id = Convert.ToInt32(r["id"].ToString());
                man.name = r["name"].ToString();
                man.manure_class = r["manure_class"].ToString();
                man.solid_liquid = r["solid_liquid"].ToString();
                man.moisture = r["moisture"].ToString();
                man.nitrogen = Convert.ToDecimal(r["nitrogen"].ToString());
                man.ammonia = Convert.ToInt32(r["ammonia"].ToString());
                man.phosphorous = Convert.ToDecimal(r["phosphorous"].ToString());
                man.potassium = Convert.ToDecimal(r["potassium"].ToString());
                man.dmid = Convert.ToInt32(r["dmid"].ToString());
                man.nminerizationid = Convert.ToInt32(r["nminerizationid"].ToString());
            }

            return man;
        }

        public Models.StaticData.Manures GetManures()
        {
            Models.StaticData.Manures mans = new Models.StaticData.Manures();
            mans.manures = new List<Models.StaticData.Manure>();

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray manures = (JArray)rss["agri"]["nmp"]["manures"]["manure"];

            foreach (var r in manures)
            {
                Models.StaticData.Manure man = new Models.StaticData.Manure();

                man.id = Convert.ToInt32(r["id"].ToString());
                man.name = r["name"].ToString();
                man.manure_class = r["manure_class"].ToString();
                man.solid_liquid = r["solid_liquid"].ToString();
                man.moisture = r["moisture"].ToString();
                man.nitrogen = Convert.ToDecimal(r["nitrogen"].ToString());
                man.ammonia = Convert.ToInt32(r["ammonia"].ToString());
                man.phosphorous = Convert.ToDecimal(r["phosphorous"].ToString());
                man.potassium = Convert.ToDecimal(r["potassium"].ToString());
                man.dmid = Convert.ToInt32(r["dmid"].ToString());
                man.nminerizationid = Convert.ToInt32(r["nminerizationid"].ToString());

                mans.manures.Add(man);
            }

            return mans;
        }

        public List<Models.StaticData.SelectListItem> GetManuresDll()
        {
            Models.StaticData.Manures mans = GetManures();

            List<Models.StaticData.SelectListItem> manOptions = new List<Models.StaticData.SelectListItem>();

            foreach (var r in mans.manures)
            {
                Models.StaticData.SelectListItem li = new Models.StaticData.SelectListItem() { Id = r.id, Value = r.name };
                manOptions.Add(li);
            }

            return manOptions;
        }

        public Models.StaticData.Season_Application GetApplication(string applId)
        {
            Models.StaticData.Season_Application appl = new Models.StaticData.Season_Application();

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray applications = (JArray)rss["agri"]["nmp"]["season-applications"]["season-applicaton"];

            foreach (var r in applications)
            {
                if (r["id"].ToString() == applId)
                {
                    appl.id = Convert.ToInt32(r["id"].ToString());
                    appl.name = r["name"].ToString();
                    appl.season = r["season"].ToString();
                    appl.application_method = r["application_method"].ToString();
                    appl.dm_lt1 = Convert.ToDecimal(r["dm_lt1"].ToString());
                    appl.dm_1_5 = Convert.ToDecimal(r["dm_1_5"].ToString());
                    appl.dm_5_10 = Convert.ToDecimal(r["dm_5_10"].ToString());
                    appl.dm_gt10 = Convert.ToDecimal(r["dm_gt10"].ToString());
                    appl.poultry_solid = r["poultry_solid"].ToString();
                    appl.compost = r["season"].ToString();
                }
            }

            return appl;
        }

        public Models.StaticData.Season_Applications GetApplications()
        {
            Models.StaticData.Season_Applications appls = new Models.StaticData.Season_Applications();
            appls.season_applications = new List<Models.StaticData.Season_Application>();

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray applications = (JArray)rss["agri"]["nmp"]["season-applications"]["season-applicaton"];

            foreach (var r in applications)
            {
                Models.StaticData.Season_Application appl = new Models.StaticData.Season_Application();

                appl.id = Convert.ToInt32(r["id"].ToString());
                appl.name = r["name"].ToString();
                appl.season = r["season"].ToString();
                appl.application_method = r["application_method"].ToString();
                appl.dm_lt1 = Convert.ToDecimal(r["dm_lt1"].ToString());
                appl.dm_1_5 = Convert.ToDecimal(r["dm_1_5"].ToString());
                appl.dm_5_10 = Convert.ToDecimal(r["dm_5_10"].ToString());
                appl.dm_gt10 = Convert.ToDecimal(r["dm_gt10"].ToString());
                appl.poultry_solid = r["poultry_solid"].ToString();
                appl.compost = r["season"].ToString();
                appls.season_applications.Add(appl);
            }

            return appls;
        }

        public List<Models.StaticData.SelectListItem> GetApplicationsDll()
        {
            Models.StaticData.Season_Applications appls = GetApplications();

            List<Models.StaticData.SelectListItem> applsOptions = new List<Models.StaticData.SelectListItem>();

            foreach (var r in appls.season_applications)
            {
                Models.StaticData.SelectListItem li = new Models.StaticData.SelectListItem() { Id = r.id, Value = r.name };
                applsOptions.Add(li);
            }

            return applsOptions;
        }

        public Models.StaticData.Unit GetUnit(string unitId)
        {
            Models.StaticData.Unit unit = new Models.StaticData.Unit();

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray units = (JArray)rss["agri"]["nmp"]["units"]["unit"];

            foreach (var r in units)
            {
                if (r["id"].ToString() == unitId)
                {
                    unit.id = Convert.ToInt32(r["id"].ToString());
                    unit.name = r["name"].ToString();
                    unit.nutrient_content_units = r["nutrient_content_units"].ToString();
                    unit.conversion_lbton = Convert.ToDecimal(r["conversion_lbton"].ToString());
                    unit.nutrient_rate_units = r["nutrient_rate_units"].ToString();
                    unit.cost_units = r["cost_units"].ToString();
                    unit.cost_applications = Convert.ToDecimal(r["cost_applications"].ToString());
                    unit.dollar_unit_area = r["dollar_unit_area"].ToString();
                    unit.value_material_units = r["value_material_units"].ToString();
                    unit.value_N = Convert.ToDecimal(r["value_N"].ToString());
                    unit.value_P2O5 = Convert.ToDecimal(r["value_P2O5"].ToString());
                    unit.value_K2O = Convert.ToDecimal(r["value_K2O"].ToString());
                    unit.solid_liquid = r["solid_liquid"].ToString();
                }
            }

            return unit;
        }

        public Models.StaticData.Units GetUnits()
        {
            Models.StaticData.Units units = new Models.StaticData.Units();
            units.units = new List<Models.StaticData.Unit>();

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray array = (JArray)rss["agri"]["nmp"]["units"]["unit"];

            foreach (var r in array)
            {
                Models.StaticData.Unit unit = new Models.StaticData.Unit();
                unit.id = Convert.ToInt32(r["id"].ToString());
                unit.name = r["name"].ToString();
                unit.nutrient_content_units = r["nutrient_content_units"].ToString();
                unit.conversion_lbton = Convert.ToDecimal(r["conversion_lbton"].ToString());
                unit.nutrient_rate_units = r["nutrient_rate_units"].ToString();
                unit.cost_units = r["cost_units"].ToString();
                unit.cost_applications = Convert.ToDecimal(r["cost_applications"].ToString());
                unit.dollar_unit_area = r["dollar_unit_area"].ToString();
                unit.value_material_units = r["value_material_units"].ToString();
                unit.value_N = Convert.ToDecimal(r["value_N"].ToString());
                unit.value_P2O5 = Convert.ToDecimal(r["value_P2O5"].ToString());
                unit.value_K2O = Convert.ToDecimal(r["value_K2O"].ToString());
                unit.solid_liquid = r["solid_liquid"].ToString();
                units.units.Add(unit);
            }

            return units;
        }

        public List<Models.StaticData.SelectListItem> GetUnitsDll(string unitType)
        {
            Models.StaticData.Units units = GetUnits();

            List<Models.StaticData.SelectListItem> unitsOptions = new List<Models.StaticData.SelectListItem>();

            foreach (var r in units.units)
            {
                if (r.solid_liquid == unitType)
                {
                    Models.StaticData.SelectListItem li = new Models.StaticData.SelectListItem() { Id = r.id, Value = r.name };
                    unitsOptions.Add(li);
                }
            }

            return unitsOptions;
        }

        public Models.StaticData.FertilizerUnits GetFertilizerUnits()
        {
            Models.StaticData.FertilizerUnits units = new Models.StaticData.FertilizerUnits();
            units.fertilizerUnits = new List<Models.StaticData.FertilizerUnit>();

            JArray array = (JArray)rss["agri"]["nmp"]["fertilizerunits"]["fertilizerunit"];

            foreach (var r in array)
            {
                Models.StaticData.FertilizerUnit unit = new Models.StaticData.FertilizerUnit();
                unit.id = Convert.ToInt32(r["id"].ToString());
                unit.name = r["name"].ToString();
                unit.dry_liquid = r["dry_liquid"].ToString();                
                if (r["conv_to_impgalperac"] != null)
                    unit.conv_to_impgalperac = Convert.ToDecimal(r["conv_to_impgalperac"].ToString());
                units.fertilizerUnits.Add(unit);
            }

            return units;
        }

        public List<Models.StaticData.SelectListItem> GetFertilizerUnitsDll(string unitType)
        {
            Models.StaticData.FertilizerUnits units = GetFertilizerUnits();

            List<Models.StaticData.SelectListItem> unitsOptions = new List<Models.StaticData.SelectListItem>();

            foreach (var r in units.fertilizerUnits)
            {
                if (r.dry_liquid == unitType)
                {
                    Models.StaticData.SelectListItem li = new Models.StaticData.SelectListItem() { Id = r.id, Value = r.name };
                    unitsOptions.Add(li);
                }
            }

            return unitsOptions;
        }

        public Models.StaticData.FertilizerUnit GetFertilizerUnit(int Id)
        {
            Models.StaticData.FertilizerUnit fertilizerUnit = new Models.StaticData.FertilizerUnit();
            
            JArray fertilizerUnits = (JArray)rss["agri"]["nmp"]["fertilizerunits"]["fertilizerunit"];

            foreach (var r in fertilizerUnits)
            {
                if (r["id"].ToString() == Id.ToString())
                {
                    fertilizerUnit.id = Convert.ToInt32(r["id"].ToString());
                    fertilizerUnit.name = r["name"].ToString();
                    fertilizerUnit.dry_liquid = r["dry_liquid"].ToString();
                    fertilizerUnit.conv_to_impgalperac = r["conv_to_impgalperac"] == null ? 0 : Convert.ToDecimal(r["conv_to_impgalperac"].ToString());
                }
            }

            return fertilizerUnit;
        }

        public Models.StaticData.DensityUnits GetDensityUnits()
        {
            Models.StaticData.DensityUnits units = new Models.StaticData.DensityUnits();
            units.densityUnits = new List<Models.StaticData.DensityUnit>();

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray array = (JArray)rss["agri"]["nmp"]["densityunits"]["densityunit"];

            foreach (var r in array)
            {
                Models.StaticData.DensityUnit unit = new Models.StaticData.DensityUnit();
                unit.id = Convert.ToInt32(r["id"].ToString());
                unit.name = r["name"].ToString();
                unit.convfactor = Convert.ToDecimal(r["convfactor"].ToString());
                units.densityUnits.Add(unit);
            }

            return units;
        }

        public List<Models.StaticData.SelectListItem> GetDensityUnitsDll()
        {
            Models.StaticData.DensityUnits units = GetDensityUnits();

            List<Models.StaticData.SelectListItem> unitsOptions = new List<Models.StaticData.SelectListItem>();

            foreach (var r in units.densityUnits)
            {
                Models.StaticData.SelectListItem li = new Models.StaticData.SelectListItem() { Id = r.id, Value = r.name };
                unitsOptions.Add(li);
            }

            return unitsOptions;
        }

        public Models.StaticData.DensityUnit GetDensityUnit(int Id)
        {
            Models.StaticData.DensityUnit densityUnit = new Models.StaticData.DensityUnit();

            JArray densityUnits = (JArray)rss["agri"]["nmp"]["densityunits"]["densityunit"];

            foreach (var r in densityUnits)
            {
                if (r["id"].ToString() == Id.ToString())
                {
                    densityUnit.id = Convert.ToInt32(r["id"].ToString());
                    densityUnit.name = r["name"].ToString();
                    densityUnit.convfactor = Convert.ToDecimal(r["convfactor"].ToString());
                }
            }

            return densityUnit;
        }

        public Models.StaticData.CropTypes GetCropTypes()
        {
            Models.StaticData.CropTypes types = new Models.StaticData.CropTypes();
            types.cropTypes = new List<Models.StaticData.CropType>();

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray array = (JArray)rss["agri"]["nmp"]["croptypes"]["croptype"];

            foreach (var r in array)
            {
                Models.StaticData.CropType type = new Models.StaticData.CropType();
                type.id = Convert.ToInt32(r["id"].ToString());
                type.name = r["name"].ToString();
                type.covercrop = r["covercrop"].ToString() == "true" ? true : false;
                type.crudeproteinrequired = r["crudeproteinrequired"].ToString() == "true" ? true : false;
                type.customcrop = r["customcrop"].ToString() == "true" ? true : false;
                type.modifynitrogen = r["modifynitrogen"].ToString() == "true" ? true : false;
                types.cropTypes.Add(type);
            }

            return types;
        }

        public Models.StaticData.CropType GetCropType(int id)
        {
            string x = id.ToString();
            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray array = (JArray)rss["agri"]["nmp"]["croptypes"]["croptype"];
            JObject rec = array.Children<JObject>().FirstOrDefault(o => o["id"] != null && o["id"].ToString() == id.ToString());

            Models.StaticData.CropType type = new Models.StaticData.CropType();
            type.id = Convert.ToInt32(rec["id"].ToString());
            type.name = rec["name"].ToString();
            type.covercrop = rec["covercrop"].ToString() == "true" ? true : false;
            type.crudeproteinrequired = rec["crudeproteinrequired"].ToString() == "true" ? true : false;
            type.customcrop = rec["customcrop"].ToString() == "true" ? true : false;
            type.modifynitrogen = rec["modifynitrogen"].ToString() == "true" ? true : false;

            return type;
        }

        public List<Models.StaticData.SelectListItem> GetCropTypesDll()
        {
            Models.StaticData.CropTypes types = GetCropTypes();

            List<Models.StaticData.SelectListItem> typesOptions = new List<Models.StaticData.SelectListItem>();

            foreach (var r in types.cropTypes)
            {
                Models.StaticData.SelectListItem li = new Models.StaticData.SelectListItem() { Id = r.id, Value = r.name };
                typesOptions.Add(li);
            }

            return typesOptions;
        }

        public Models.StaticData.Crops GetCrops()
        {
            Models.StaticData.Crops crops = new Models.StaticData.Crops();
            crops.crops = new List<Models.StaticData.Crop>();

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray array = (JArray)rss["agri"]["nmp"]["crops"]["crop"];

            foreach (var r in array)
            {

                Models.StaticData.Crop crop = new Models.StaticData.Crop();
                crop.id = Convert.ToInt32(r["id"].ToString());
                crop.cropname = r["cropname"].ToString();
                crop.croptypeid = Convert.ToInt32(r["croptypeid"].ToString());
                crop.yieldcd = Convert.ToInt32(r["yieldcd"].ToString());
                crop.cropremovalfactor_N = r["cropremovalfactor_N"].ToString() == "null" ? (decimal?)null : Convert.ToDecimal(r["cropremovalfactor_N"].ToString());
                crop.cropremovalfactor_P2O5 = r["cropremovalfactor_P2O5"].ToString() == "null" ? (decimal?)null : Convert.ToDecimal(r["cropremovalfactor_P2O5"].ToString());
                crop.cropremovalfactor_K2O = r["cropremovalfactor_K2O"].ToString() == "null" ? (decimal?)null : Convert.ToDecimal(r["cropremovalfactor_K2O"].ToString());
                crop.n_recommcd = Convert.ToDecimal(r["n_recommcd"].ToString());
                crop.n_recomm_lbperac = r["n_recomm_lbperac"].ToString() == "null" ? (decimal?)null : Convert.ToDecimal(r["n_recomm_lbperac"].ToString());
                crop.n_high_lbperac = r["n_high_lbperac"].ToString() == "null" ? (decimal?)null : Convert.ToDecimal(r["n_high_lbperac"].ToString());
                crop.prevcropcd = Convert.ToInt32(r["prevcropcd"].ToString());
                
                crops.crops.Add(crop);
            }

            return crops;
        }

        public List<Models.StaticData.SelectListItem> GetCropsDll(int cropType)
        {
            Models.StaticData.Crops crops = GetCrops();

            List<Models.StaticData.SelectListItem> cropsOptions = new List<Models.StaticData.SelectListItem>();

            foreach (var r in crops.crops)
            {
                if (r.croptypeid == cropType)
                {
                    Models.StaticData.SelectListItem li = new Models.StaticData.SelectListItem() { Id = r.id, Value = r.cropname };
                    cropsOptions.Add(li);
                }
            }

            return cropsOptions;
        }

        public Models.StaticData.Crops GetCrops(int cropType)
        {
            Models.StaticData.Crops crops = GetCrops();
            foreach (var r in crops.crops)
            {
                if (r.croptypeid != cropType)
                {
                    crops.crops.Remove(r);
                }
            }

            return crops;
        }

        public Models.StaticData.Crop GetCrop(int cropId)
        {
            Models.StaticData.Crops crops = new Models.StaticData.Crops();

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JObject r = (JObject)rss["agri"]["nmp"]["crops"]["crop"].FirstOrDefault(x => x["id"].ToString() == cropId.ToString());
            Models.StaticData.Crop crop = new Models.StaticData.Crop();

            crop.cropname = r["cropname"].ToString();
            crop.croptypeid = Convert.ToInt32(r["croptypeid"].ToString());
            crop.yieldcd = Convert.ToInt32(r["yieldcd"].ToString());
            crop.cropremovalfactor_N = r["cropremovalfactor_N"].ToString() == "null" ? (decimal?)null : Convert.ToDecimal(r["cropremovalfactor_N"].ToString());
            crop.cropremovalfactor_P2O5 = r["cropremovalfactor_P2O5"].ToString() == "null" ? (decimal?)null : Convert.ToDecimal(r["cropremovalfactor_P2O5"].ToString());
            crop.cropremovalfactor_K2O = r["cropremovalfactor_K2O"].ToString() == "null" ? (decimal?)null : Convert.ToDecimal(r["cropremovalfactor_K2O"].ToString());
            crop.n_recommcd = Convert.ToDecimal(r["n_recommcd"].ToString());
            crop.n_recomm_lbperac = r["n_recomm_lbperac"].ToString() == "null" ? (decimal?)null : Convert.ToDecimal(r["n_recomm_lbperac"].ToString());
            crop.n_high_lbperac = r["n_high_lbperac"].ToString() == "null" ? (decimal?)null : Convert.ToDecimal(r["n_high_lbperac"].ToString());
            crop.prevcropcd = Convert.ToInt32(r["prevcropcd"].ToString());

            return crop;
        }

        public Models.StaticData.Yield GetYield(int yieldId)
        {

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray array = (JArray)rss["agri"]["nmp"]["yields"]["yield"];
            Models.StaticData.Yield yield = new Models.StaticData.Yield();

            foreach (var r in array)
            {
                if (Convert.ToInt32(r["id"].ToString()) == yieldId)
                {
                    yield.id = Convert.ToInt32(r["id"].ToString());
                    yield.yielddesc = r["yielddesc"].ToString();
                }
            }

            return yield;
        }

        public Models.StaticData.CropSTPRegionCd GetCropSTPRegionCd(int cropid, int soil_test_phosphorous_region_cd)
        {

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray array = (JArray)rss["agri"]["nmp"]["crop_stp_regioncds"]["crop_stp_regioncd"];
            Models.StaticData.CropSTPRegionCd crop_stp_regioncd = new Models.StaticData.CropSTPRegionCd();

            foreach (var r in array)
            {
                if (Convert.ToInt32(r["cropid"].ToString()) == cropid &&
                    Convert.ToInt32(r["soil_test_phosphorous_region_cd"].ToString()) == soil_test_phosphorous_region_cd)
                {
                    crop_stp_regioncd.cropid = Convert.ToInt32(r["cropid"].ToString());
                    crop_stp_regioncd.soil_test_phosphorous_region_cd = Convert.ToInt32(r["soil_test_phosphorous_region_cd"].ToString());
                    crop_stp_regioncd.phosphorous_crop_group_region_cd = r["phosphorous_crop_group_region_cd"].ToString() == "null" ? (int?)null : Convert.ToInt32(r["phosphorous_crop_group_region_cd"].ToString());
                }
            }

            return crop_stp_regioncd;
        }

        public Models.StaticData.CropSTKRegionCd GetCropSTKRegionCd(int cropid, int soil_test_potassium_region_cd)
        {

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray array = (JArray)rss["agri"]["nmp"]["crop_stk_regioncds"]["crop_stk_regioncd"];
            Models.StaticData.CropSTKRegionCd crop_stk_regioncd = new Models.StaticData.CropSTKRegionCd();

            foreach (var r in array)
            {
                if (Convert.ToInt32(r["cropid"].ToString()) == cropid &&
                    Convert.ToInt32(r["soil_test_potassium_region_cd"].ToString()) == soil_test_potassium_region_cd)
                {
                    crop_stk_regioncd.cropid = Convert.ToInt32(r["cropid"].ToString());
                    crop_stk_regioncd.soil_test_potassium_region_cd = Convert.ToInt32(r["soil_test_potassium_region_cd"].ToString());
                    crop_stk_regioncd.potassium_crop_group_region_cd = r["potassium_crop_group_region_cd"].ToString() == "null" ? (int?)null : Convert.ToInt32(r["potassium_crop_group_region_cd"].ToString());
                }
            }

            return crop_stk_regioncd;
        }

        public Models.StaticData.DM GetDM(int ID)
        {

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray array = (JArray)rss["agri"]["nmp"]["dms"]["dm"];
            Models.StaticData.DM dm = new Models.StaticData.DM();

            foreach (var r in array)
            {
                if (Convert.ToInt32(r["ID"].ToString()) == ID)
                {
                    dm.ID = Convert.ToInt32(r["ID"].ToString());
                    dm.name = r["name"].ToString();
                }
            }

            return dm;
        }

        public Models.StaticData.AmmoniaRetention GetAmmoniaRetention(int seasonApplicatonId, int dm)
        {

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray array = (JArray)rss["agri"]["nmp"]["ammoniaretentions"]["ammoniaretention"];
            Models.StaticData.AmmoniaRetention ammoniaretention = new Models.StaticData.AmmoniaRetention();

            foreach (var r in array)
            {
                if (Convert.ToInt32(r["seasonapplicatonid"].ToString()) == seasonApplicatonId &&
                    Convert.ToInt32(r["dm"].ToString()) == dm)
                {
                    ammoniaretention.seasonapplicatonid = Convert.ToInt32(r["seasonapplicatonid"].ToString());
                    ammoniaretention.dm = Convert.ToInt32(r["dm"].ToString());
                    ammoniaretention.value = r["value"].ToString() == "null" ? (decimal?)null : Convert.ToDecimal(r["value"].ToString());
                }
            }

            return ammoniaretention;
        }

        public Models.StaticData.NMineralization GetNMineralization(int id, int locationid)
        {

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray array = (JArray)rss["agri"]["nmp"]["nmineralizations"]["nmineralization"];
            Models.StaticData.NMineralization nmineralization = new Models.StaticData.NMineralization();

            foreach (var r in array)
            {
                if (Convert.ToInt32(r["id"].ToString()) == id &&
                    Convert.ToInt32(r["locationid"].ToString()) == locationid)
                {
                    nmineralization.id = Convert.ToInt32(r["id"].ToString());
                    nmineralization.name = r["name"].ToString();
                    nmineralization.locationid = Convert.ToInt32(r["locationid"].ToString());
                    nmineralization.firstyearvalue = Convert.ToDecimal(r["firstyearvalue"].ToString());
                    nmineralization.longtermvalue = Convert.ToDecimal(r["longtermvalue"].ToString());
                }
            }

            return nmineralization;
        }
        public string GetSoilTestMethod(string id)
        {
            string method = id.ToString();
            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray array = (JArray)rss["agri"]["nmp"]["soiltestmethods"]["soiltestmethod"];
            JObject rec = array.Children<JObject>().FirstOrDefault(o => o["id"] != null && o["id"].ToString() == id);

            method = rec["name"].ToString();

            return method;
        }
        public Models.StaticData.SoilTestMethods GetSoilTestMethods()
        {
            Models.StaticData.SoilTestMethods meths = new Models.StaticData.SoilTestMethods();
            meths.methods = new List<Models.StaticData.SoilTestMethod>();

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray items = (JArray)rss["agri"]["nmp"]["soiltestmethods"]["soiltestmethod"];

            foreach (var r in items)
            {
                Models.StaticData.SoilTestMethod rec = new Models.StaticData.SoilTestMethod();

                rec.id = Convert.ToInt32(r["id"].ToString());
                rec.name = r["name"].ToString();
                rec.ConvertToKelownaPlt72 = Convert.ToDecimal(r["ConvertToKelownaPlt72"].ToString());
                rec.ConvertToKelownaPge72 = Convert.ToDecimal(r["ConvertToKelownaPge72"].ToString());
                rec.ConvertToKelownaK = Convert.ToDecimal(r["ConvertToKelownaK"].ToString());

                meths.methods.Add(rec);
            }

            return meths;
        }

        public List<Models.StaticData.SelectListItem> GetSoilTestMethodsDll()
        {
            Models.StaticData.SoilTestMethods meths = GetSoilTestMethods();

            List<Models.StaticData.SelectListItem> mthOptions = new List<Models.StaticData.SelectListItem>();

            foreach (var r in meths.methods)
            {
                Models.StaticData.SelectListItem li = new Models.StaticData.SelectListItem() { Id = r.id, Value = r.name };
                mthOptions.Add(li);
            }

            return mthOptions;
        }
        
        public Models.StaticData.Region GetRegion(int id)
        {

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray array = (JArray)rss["agri"]["nmp"]["regions"]["region"];
            Models.StaticData.Region region = new Models.StaticData.Region();

            foreach (var r in array)
            {
                if (Convert.ToInt32(r["id"].ToString()) == id)
                {
                    region.id = Convert.ToInt32(r["id"].ToString());
                    region.name = r["name"].ToString();
                    region.soil_test_phospherous_region_cd = Convert.ToInt32(r["soil_test_phospherous_region_cd"].ToString());
                    region.soil_test_potassium_region_cd = Convert.ToInt32(r["soil_test_potassium_region_cd"].ToString());
                    region.locationid = Convert.ToInt32(r["locationid"].ToString());
                }
            }

            return region;
        }
      
        public Models.StaticData.PrevCropType GetPrevCropType(int id)
        {
            Models.StaticData.PrevCropType type = new Models.StaticData.PrevCropType();

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray array = (JArray)rss["agri"]["nmp"]["prevcroptypes"]["prevcroptype"];
            JObject rec = array.Children<JObject>().FirstOrDefault(o => o["id"] != null && o["id"].ToString() == id.ToString());

            type.id = Convert.ToInt32(rec["id"].ToString());
            type.prevcropcd = Convert.ToInt32(rec["prevcropcd"].ToString());
            type.name = rec["name"].ToString();
            type.nCreditMetric = Convert.ToInt32(rec["ncreditmetric"].ToString());
            type.nCreditImperial = Convert.ToInt32(rec["ncreditimperial"].ToString());

            return type;
        }

        public Models.StaticData.PrevCropTypes GetPrevCropTypes()
        {
            Models.StaticData.PrevCropTypes types = new Models.StaticData.PrevCropTypes();
            types.prevCropTypes = new List<Models.StaticData.PrevCropType>();

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray array = (JArray)rss["agri"]["nmp"]["prevcroptypes"]["prevcroptype"];

            foreach (var r in array)
            {
                Models.StaticData.PrevCropType type = new Models.StaticData.PrevCropType();
                type.id = Convert.ToInt32(r["id"].ToString());
                type.prevcropcd = Convert.ToInt32(r["prevcropcd"].ToString());
                type.name = r["name"].ToString();
                type.nCreditMetric = Convert.ToInt32(r["ncreditmetric"].ToString());
                type.nCreditImperial = Convert.ToInt32(r["ncreditimperial"].ToString());
                types.prevCropTypes.Add(type);
            }

            return types;
        }

        public List<Models.StaticData.SelectListItem> GetPrevCropTypesDll(string prevCropCd)
        {
            Models.StaticData.PrevCropTypes types = GetPrevCropTypes();

            List<Models.StaticData.SelectListItem> typesOptions = new List<Models.StaticData.SelectListItem>();

            foreach (var r in types.prevCropTypes)
            {
                if (r.prevcropcd.ToString() == prevCropCd)
                {
                    Models.StaticData.SelectListItem li = new Models.StaticData.SelectListItem() { Id = r.id, Value = r.name };
                    typesOptions.Add(li);
                }
            }

            return typesOptions;
        }

        public Models.StaticData.CropYield GetCropYield(int cropid, int locationid)
        {

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray array = (JArray)rss["agri"]["nmp"]["cropyields"]["cropyield"];
            Models.StaticData.CropYield cropYield = new Models.StaticData.CropYield();

            foreach (var r in array)
            {
                if (Convert.ToInt32(r["cropid"].ToString()) == cropid && 
                    Convert.ToInt32(r["locationid"].ToString()) == locationid)
                {
                    cropYield.cropid = Convert.ToInt32(r["cropid"].ToString());
                    cropYield.locationid = Convert.ToInt32(r["locationid"].ToString());
                    cropYield.amt = r["amt"].ToString() == "null" ? (decimal?)null : Convert.ToDecimal(r["amt"].ToString());
                }
            }

            return cropYield;
        }

        public Models.StaticData.STPRecommend GetSTPRecommend(int stp_kelowna_rangeid, int soil_test_phosphorous_region_cd, int phosphorous_crop_group_region_cd)
        {

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray array = (JArray)rss["agri"]["nmp"]["stp_recommends"]["stp_recommend"];
            Models.StaticData.STPRecommend sTPRecommend = new Models.StaticData.STPRecommend();

            foreach (var r in array)
            {
                if (Convert.ToInt32(r["stp_kelowna_rangeid"].ToString()) == stp_kelowna_rangeid &&
                    Convert.ToInt32(r["soil_test_phosphorous_region_cd"].ToString()) == soil_test_phosphorous_region_cd &&
                    Convert.ToInt32(r["phosphorous_crop_group_region_cd"].ToString()) == phosphorous_crop_group_region_cd)
                {
                    sTPRecommend.stp_kelowna_rangeid = Convert.ToInt32(r["stp_kelowna_rangeid"].ToString());
                    sTPRecommend.soil_test_phosphorous_region_cd = Convert.ToInt32(r["soil_test_phosphorous_region_cd"].ToString());
                    sTPRecommend.phosphorous_crop_group_region_cd = Convert.ToInt32(r["phosphorous_crop_group_region_cd"].ToString());
                    sTPRecommend.p2o5_recommend_kgperha = Convert.ToInt32(r["p2o5_recommend_kgperha"].ToString());
                }
            }

            return sTPRecommend;
        }

        public Models.StaticData.STPKelownaRange GetSTPKelownaRangeByPpm(int ppm)
        {

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray array = (JArray)rss["agri"]["nmp"]["stp_kelowna_ranges"]["stp_kelowna_range"];
            Models.StaticData.STPKelownaRange sTPKelownaRange = new Models.StaticData.STPKelownaRange();

            foreach (var r in array)
            {
                if (ppm >= Convert.ToInt32(r["range_low"].ToString()) &&
                    ppm <= Convert.ToInt32(r["range_high"].ToString()))
                {
                    sTPKelownaRange.id = Convert.ToInt32(r["id"].ToString());
                    sTPKelownaRange.range = r["range"].ToString();
                    sTPKelownaRange.range_low = Convert.ToInt32(r["range_low"].ToString());
                    sTPKelownaRange.range_high = Convert.ToInt32(r["range_high"].ToString());
                }
            }

            return sTPKelownaRange;
        }

        public Models.StaticData.STKRecommend GetSTKRecommend(int stk_kelowna_rangeid, int soil_test_potassium_region_cd, int potassium_crop_group_region_cd)
        {

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray array = (JArray)rss["agri"]["nmp"]["stk_recommends"]["stk_recommend"];
            Models.StaticData.STKRecommend sTKRecommend = new Models.StaticData.STKRecommend();

            foreach (var r in array)
            {
                if (Convert.ToInt32(r["stk_kelowna_rangeid"].ToString()) == stk_kelowna_rangeid &&
                    Convert.ToInt32(r["soil_test_potassium_region_cd"].ToString()) == soil_test_potassium_region_cd &&
                    Convert.ToInt32(r["potassium_crop_group_region_cd"].ToString()) == potassium_crop_group_region_cd)
                {
                    sTKRecommend.stk_kelowna_rangeid = Convert.ToInt32(r["stk_kelowna_rangeid"].ToString());
                    sTKRecommend.soil_test_potassium_region_cd = Convert.ToInt32(r["soil_test_potassium_region_cd"].ToString());
                    sTKRecommend.potassium_crop_group_region_cd = Convert.ToInt32(r["potassium_crop_group_region_cd"].ToString());
                    sTKRecommend.k2o_recommend_kgperha = Convert.ToInt32(r["k2o_recommend_kgperha"].ToString());
                }
            }

            return sTKRecommend;
        }

        public Models.StaticData.STKKelownaRange GetSTKKelownaRangeByPpm(int ppm)
        {

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray array = (JArray)rss["agri"]["nmp"]["stk_kelowna_ranges"]["stk_kelowna_range"];
            Models.StaticData.STKKelownaRange sTKKelownaRange = new Models.StaticData.STKKelownaRange();

            foreach (var r in array)
            {
                if (ppm >= Convert.ToInt32(r["range_low"].ToString()) &&
                    ppm <= Convert.ToInt32(r["range_high"].ToString()))
                {
                    sTKKelownaRange.id = Convert.ToInt32(r["id"].ToString());
                    sTKKelownaRange.range = r["range"].ToString();
                    sTKKelownaRange.range_low = Convert.ToInt32(r["range_low"].ToString());
                    sTKKelownaRange.range_high = Convert.ToInt32(r["range_high"].ToString());
                }
            }

            return sTKKelownaRange;
        }

        public Models.StaticData.ConversionFactor GetConversionFactor()
        {
            Models.StaticData.ConversionFactor cf = new Models.StaticData.ConversionFactor();

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            
            cf.n_protein_conversion = Convert.ToDecimal((string)rss["agri"]["nmp"]["conversions"]["n_protein_conversion"]);
            cf.unit_conversion = Convert.ToDecimal((string)rss["agri"]["nmp"]["conversions"]["unit_conversion"]);
            cf.defaultSoilTestKelownaP = Convert.ToInt16((string)rss["agri"]["nmp"]["conversions"]["defaultSoilTestKelownaP"]); 
            cf.defaultSoilTestKelownaK = Convert.ToInt16((string)rss["agri"]["nmp"]["conversions"]["defaultSoilTestKelownaK"]);
            cf.kgperha_lbperac_conversion = Convert.ToDecimal((string)rss["agri"]["nmp"]["conversions"]["kgperha_lbperac_conversion"]);
            cf.potassiumAvailabilityFirstYear = Convert.ToDecimal((string)rss["agri"]["nmp"]["conversions"]["potassiumAvailabilityFirstYear"]);
            cf.potassiumAvailabilityLongTerm = Convert.ToDecimal((string)rss["agri"]["nmp"]["conversions"]["potassiumAvailabilityLongTerm"]);
            cf.potassiumKtoK2Oconversion = Convert.ToDecimal((string)rss["agri"]["nmp"]["conversions"]["potassiumKtoK2Oconversion"]);
            cf.phosphorousAvailabilityFirstYear = Convert.ToDecimal((string)rss["agri"]["nmp"]["conversions"]["phosphorousAvailabilityFirstYear"]);
            cf.phosphorousAvailabilityLongTerm = Convert.ToDecimal((string)rss["agri"]["nmp"]["conversions"]["phosphorousAvailabilityLongTerm"]);
            cf.phosphorousPtoP2O5Kconversion = Convert.ToDecimal((string)rss["agri"]["nmp"]["conversions"]["phosphorousPtoP2O5Kconversion"]);
            cf.lbPerTonConversion = Convert.ToDecimal((string)rss["agri"]["nmp"]["conversions"]["lbPerTonConversion"]);

            return cf;
        }

        public Utility.BalanceMessages GetMessageByChemicalBalance(string balanceType, int balance, bool legume)
        {            
            JArray array = (JArray)rss["agri"]["nmp"]["messages"]["message"];            
            Utility.BalanceMessages bm = new Utility.BalanceMessages();

            foreach (var r in array)
            {
                if (balanceType == r["balanceType"].ToString() && 
                    balance >= Convert.ToInt32(r["balance_low"].ToString()) &&
                    balance <= Convert.ToInt32(r["balance_high"].ToString()))
                {
                    bm.Chemical = balanceType;
                    if (r["displayMessage"].ToString() == "Yes")
                        bm.Message = string.Format(r["text"].ToString(), Math.Abs(balance).ToString());
                    bm.Icon = r["icon"].ToString();
                
                    //Message is removed for AgrN Legumes
                    if (balanceType == "AgrN" &&
                        legume &&
                        Convert.ToInt32(r["balance_low"].ToString()) == -99999)
                    {
                        bm = null;                        
                    }

                    return bm;
                }
            }

            return bm;
        }

        public string GetMessageByChemicalBalance(string balanceType, int balance, bool legume, decimal soilTest)
        {            
            JArray array = (JArray)rss["agri"]["nmp"]["messages"]["message"];
            string message = null;

            foreach (var r in array)
            {
                if (balanceType == r["balanceType"].ToString() &&
                    balance >= Convert.ToInt32(r["balance_low"].ToString()) &&
                    balance <= Convert.ToInt32(r["balance_high"].ToString()) &&
                    soilTest >= Convert.ToDecimal(r["soiltest_low"].ToString()) &&
                    soilTest <= Convert.ToDecimal(r["soiltest_high"].ToString()))
                {
                    message = string.Format(r["text"].ToString(), Math.Abs(balance).ToString());
                }

                //If legume crop in field never display that more N is required
                if (balanceType == "AgrN" &&
                    legume &&
                    Convert.ToInt32(r["balance_high"].ToString()) == 9999)
                {
                    message = string.Empty;
                }
            }

            return message;
        }

        public Utility.BalanceMessages GetMessageByChemicalBalance(string balanceType, int balance1, int balance2, string assignedChemical)
        {            
            JArray array = (JArray)rss["agri"]["nmp"]["messages"]["message"];            
            Utility.BalanceMessages bm = new Utility.BalanceMessages();

            foreach (var r in array)
            {
                if (balanceType == r["balanceType"].ToString() &&
                    balance1 >= Convert.ToInt32(r["balance_low"].ToString()) &&
                    balance1 <= Convert.ToInt32(r["balance_high"].ToString()) &&
                    balance2 >= Convert.ToInt32(r["balance1_low"].ToString()) &&
                    balance2 <= Convert.ToInt32(r["balance1_high"].ToString()))
                {                    
                    bm.Chemical = assignedChemical;
                    if (r["displayMessage"].ToString() == "Yes")
                        bm.Message = r["text"].ToString();
                    bm.Icon = r["icon"].ToString();
                }
            }
            
            return bm;
        }

        public Models.StaticData.FertilizerType GetFertilizerType(string id)
        {
            string x = id.ToString();
            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray array = (JArray)rss["agri"]["nmp"]["fertilizertypes"]["fertilizertype"];
            JObject rec = array.Children<JObject>().FirstOrDefault(o => o["id"] != null && o["id"].ToString() == id);

            Models.StaticData.FertilizerType type = new Models.StaticData.FertilizerType();
            type.id = Convert.ToInt32(rec["id"].ToString());
            type.name = rec["name"].ToString();
            type.dry_liquid = rec["dry_liquid"].ToString();
            type.custom = rec["customfertilizer"].ToString() == "true" ? true : false;

            return type;
        }

        public Models.StaticData.FertilizerTypes GetFertilizerTypes()
        {
            Models.StaticData.FertilizerTypes types = new Models.StaticData.FertilizerTypes();
            types.fertilizerTypes = new List<Models.StaticData.FertilizerType>();

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray array = (JArray)rss["agri"]["nmp"]["fertilizertypes"]["fertilizertype"];

            foreach (var r in array)
            {
                Models.StaticData.FertilizerType type = new Models.StaticData.FertilizerType();
                type.id = Convert.ToInt32(r["id"].ToString());
                type.name = r["name"].ToString();
                type.dry_liquid = r["dry_liquid"].ToString();
                type.custom = r["customfertilizer"].ToString() == "true" ? true : false;
                types.fertilizerTypes.Add(type);
            }

            return types;
        }

        public List<Models.StaticData.SelectListItem> GetFertilizerTypesDll()
        {
            Models.StaticData.FertilizerTypes types = GetFertilizerTypes();

            List<Models.StaticData.SelectListItem> typesOptions = new List<Models.StaticData.SelectListItem>();

            foreach (var r in types.fertilizerTypes)
            {
                Models.StaticData.SelectListItem li = new Models.StaticData.SelectListItem() { Id = r.id, Value = r.name };
                typesOptions.Add(li);
            }

            return typesOptions;
        }

        public Models.StaticData.Fertilizer GetFertilizer(string id)
        {
            string x = id.ToString();
            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray array = (JArray)rss["agri"]["nmp"]["fertilizers"]["fertilizer"];
            JObject rec = array.Children<JObject>().FirstOrDefault(o => o["id"] != null && o["id"].ToString() == id);

            Models.StaticData.Fertilizer fertilizer = new Models.StaticData.Fertilizer();
            fertilizer.id = Convert.ToInt32(rec["id"].ToString());
            fertilizer.name = rec["name"].ToString();
            fertilizer.dry_liquid = rec["dry_liquid"].ToString();
            fertilizer.nitrogen = Convert.ToDecimal(rec["nitrogen"].ToString());
            fertilizer.phosphorous = Convert.ToDecimal(rec["phosphorous"].ToString());
            fertilizer.potassium = Convert.ToDecimal(rec["potassium"].ToString());

            return fertilizer;
        }

        public Models.StaticData.Fertilizers GetFertilizers()
        {
            Models.StaticData.Fertilizers fertilizers = new Models.StaticData.Fertilizers();
            fertilizers.fertilizers = new List<Models.StaticData.Fertilizer>();

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray array = (JArray)rss["agri"]["nmp"]["fertilizers"]["fertilizer"];

            foreach (var r in array)
            {

                Models.StaticData.Fertilizer fertilizer = new Models.StaticData.Fertilizer();
                fertilizer.id = Convert.ToInt32(r["id"].ToString());
                fertilizer.name = r["name"].ToString();
                fertilizer.dry_liquid = r["dry_liquid"].ToString();
                fertilizer.nitrogen = Convert.ToDecimal(r["nitrogen"].ToString());
                fertilizer.phosphorous = Convert.ToDecimal(r["phosphorous"].ToString());
                fertilizer.potassium = Convert.ToDecimal(r["potassium"].ToString());

                fertilizers.fertilizers.Add(fertilizer);
            }

            return fertilizers;
        }

        public List<Models.StaticData.SelectListItem> GetFertilizersDll(string fertilizerType)
        {
            Models.StaticData.Fertilizers types = GetFertilizers();

            List<Models.StaticData.SelectListItem> typesOptions = new List<Models.StaticData.SelectListItem>();

            foreach (var r in types.fertilizers)
            {
                if (r.dry_liquid.ToString() == fertilizerType)
                {
                    Models.StaticData.SelectListItem li = new Models.StaticData.SelectListItem() { Id = r.id, Value = r.name };
                    typesOptions.Add(li);
                }
            }

            return typesOptions;
        }

        public Models.StaticData.SoilTestMethod GetSoilTestMethodByMethod(string _soilTest)
        {
            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray items = (JArray)rss["agri"]["nmp"]["soiltestmethods"]["soiltestmethod"];
            Models.StaticData.SoilTestMethod soilTestMethod = new Models.StaticData.SoilTestMethod();
            
            foreach (var r in items)
            {
                if (_soilTest == r["name"].ToString())
                {
                    soilTestMethod.id = Convert.ToInt32(r["id"].ToString());
                    soilTestMethod.name = r["name"].ToString();
                    soilTestMethod.ConvertToKelownaPlt72 = Convert.ToDecimal(r["ConvertToKelownaPlt72"].ToString());
                    soilTestMethod.ConvertToKelownaPge72 = Convert.ToDecimal(r["ConvertToKelownaPge72"].ToString());
                    soilTestMethod.ConvertToKelownaK = Convert.ToDecimal(r["ConvertToKelownaK"].ToString());
                }
            }

            return soilTestMethod;
        }

        public Models.StaticData.SoilTestMethod GetSoilTestMethodById(string _id)
        {            
            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray items = (JArray)rss["agri"]["nmp"]["soiltestmethods"]["soiltestmethod"];
            Models.StaticData.SoilTestMethod soilTestMethod = new Models.StaticData.SoilTestMethod();
            int id = 0;
            int.TryParse(_id, out id);

            try
            { 
                foreach (var r in items)
                {
                    if (id == Convert.ToInt16(r["id"].ToString()))
                    {
                        soilTestMethod.id = Convert.ToInt32(r["id"].ToString());
                        soilTestMethod.name = r["name"].ToString();
                        soilTestMethod.ConvertToKelownaPlt72 = Convert.ToDecimal(r["ConvertToKelownaPlt72"].ToString());
                        soilTestMethod.ConvertToKelownaPge72 = Convert.ToDecimal(r["ConvertToKelownaPge72"].ToString());
                        soilTestMethod.ConvertToKelownaK = Convert.ToDecimal(r["ConvertToKelownaK"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle error
            }

            return soilTestMethod;
        }

        public Models.StaticData.LiquidFertilizerDensity GetLiquidFertilizerDensity(int fertilizerId, int densityId)
        {
            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray array = (JArray)rss["agri"]["nmp"]["liquidfertilizerdensitys"]["liquidfertilizerdensity"];
            JObject rec = array.Children<JObject>().FirstOrDefault(o => o["fertilizerid"] != null && o["fertilizerid"].ToString() == fertilizerId.ToString() &&  o["densityunitid"] != null && o["densityunitid"].ToString() == densityId.ToString());

            Models.StaticData.LiquidFertilizerDensity density = new Models.StaticData.LiquidFertilizerDensity();
            density.id = Convert.ToInt32(rec["id"].ToString());
            density.value = Convert.ToDecimal(rec["value"].ToString());

            return density;
        }

        public Models.StaticData.DefaultSoilTest GetDefaultSoilTest()
        {
            Models.StaticData.DefaultSoilTest dt = new Models.StaticData.DefaultSoilTest();

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));

            dt.nitrogen = Convert.ToDecimal((string)rss["agri"]["nmp"]["conversions"]["defaultSoilTestNitrogen"]);
            dt.phosphorous = Convert.ToDecimal((string)rss["agri"]["nmp"]["conversions"]["defaultSoilTestKelownaP"]);
            dt.potassium = Convert.ToDecimal((string)rss["agri"]["nmp"]["conversions"]["defaultSoilTestKelownaK"]);
            dt.pH = Convert.ToDecimal((string)rss["agri"]["nmp"]["conversions"]["defaultSoilTestpH"]);
            dt.convertedKelownaP = Convert.ToInt32((string)rss["agri"]["nmp"]["conversions"]["defaultSoilTestKelownaP"]);
            dt.convertedKelownaK = Convert.ToInt32((string)rss["agri"]["nmp"]["conversions"]["defaultSoilTestKelownaK"]);

            return dt;
        }

        public string GetDefaultSoilTestMethod()
        {
            Models.StaticData.DefaultSoilTest dt = new Models.StaticData.DefaultSoilTest();

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));

            return (string)rss["agri"]["nmp"]["conversions"]["defaultSoilTestMethodId"];
        }

        public string SoilTestRating(string chem, decimal value)
        {
            string results = "Ukn";

            Models.StaticData.Fertilizers fertilizers = new Models.StaticData.Fertilizers();
            List<SoilTestRange> ranges = new List<SoilTestRange>();

            //JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray array = (JArray)rss["agri"]["nmp"]["soiltestranges"][chem];

            foreach (var r in array)
            {
                Models.StaticData.SoilTestRange range = new Models.StaticData.SoilTestRange();
                range.upperLimit = Convert.ToInt32(r["upperlimit"].ToString());
                range.rating = r["rating"].ToString();
                ranges.Add(range);
            }
            for(int i=0; i < ranges.Count(); i++)
            {
                if(value < ranges[i].upperLimit)
                {
                    results = ranges[i].rating;
                    break;
                }
            }

            return results;
        }


        public Models.StaticData.FertilizerMethod GetFertilizerMethod(string id)
        {
            JArray array = (JArray)rss["agri"]["nmp"]["fertilizermethods"]["fertilizermethod"];
            JObject rec = array.Children<JObject>().FirstOrDefault(o => o["id"] != null && o["id"].ToString() == id);

            Models.StaticData.FertilizerMethod fertilizerMethod = new Models.StaticData.FertilizerMethod();
            fertilizerMethod.id = Convert.ToInt32((string)rec["id"]);
            fertilizerMethod.name = (string)rec["name"];

            return fertilizerMethod;
        }

        public Models.StaticData.FertilizerMethods GetFertilizerMethods()
        {
            Models.StaticData.FertilizerMethods fertilizerMethods = new Models.StaticData.FertilizerMethods();
            fertilizerMethods.fertilizerMethods = new List<Models.StaticData.FertilizerMethod>();

            JArray array = (JArray)rss["agri"]["nmp"]["fertilizermethods"]["fertilizermethod"];

            foreach (var r in array)
            {
                Models.StaticData.FertilizerMethod fertilizerMethod = new Models.StaticData.FertilizerMethod();
                fertilizerMethod.id = Convert.ToInt32(r["id"].ToString());
                fertilizerMethod.name = r["name"].ToString();
                fertilizerMethods.fertilizerMethods.Add(fertilizerMethod);
            }

            return fertilizerMethods;
        }

        public List<Models.StaticData.SelectListItem> GetFertilizerMethodsDll()
        {
            Models.StaticData.FertilizerMethods methods = GetFertilizerMethods();

            List<Models.StaticData.SelectListItem> methodsOptions = new List<Models.StaticData.SelectListItem>();

            foreach (var r in methods.fertilizerMethods)
            {
                Models.StaticData.SelectListItem li = new Models.StaticData.SelectListItem() { Id = r.id, Value = r.name };
                methodsOptions.Add(li);
            }

            return methodsOptions;
        }

        public string GetSoilTestWarning()
        {
            string template = (string)rss["agri"]["nmp"]["conversions"]["defaultSoilTestMessage"];
            decimal pH = Convert.ToDecimal((string)rss["agri"]["nmp"]["conversions"]["defaultSoilTestpH"]);
            decimal phosphorous = Convert.ToDecimal((string)rss["agri"]["nmp"]["conversions"]["defaultSoilTestKelownaP"]);
            decimal potassium = Convert.ToDecimal((string)rss["agri"]["nmp"]["conversions"]["defaultSoilTestKelownaK"]);

            string msg = string.Format(template, phosphorous, potassium, pH);

            return msg;
        }

        public string GetExternalLink(string name)
        {
            string result = string.Empty;

            JArray array = (JArray)rss["agri"]["nmp"]["externallinks"]["externallink"];
            JObject rec = array.Children<JObject>().FirstOrDefault(o => o["name"].ToString() == name);

            if(rec != null)
                result = (string)rec["url"];

            return result;
        }

        public string GetUserPrompt(string name)
        {
            string result = string.Empty;

            JArray array = (JArray)rss["agri"]["nmp"]["userprompts"]["userprompt"];
            JObject rec = array.Children<JObject>().FirstOrDefault(o => o["name"].ToString() == name);

            if (rec != null)
                result = (string)rec["text"];

            return result;
        }
    }
}

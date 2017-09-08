﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SERVERAPI.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SERVERAPI.Models.Impl
{
    public partial class StaticData
    {
        private readonly IHttpContextAccessor _ctx;

        public StaticData(IHttpContextAccessor ctx)
        {
            _ctx = ctx;
        }

        public Models.StaticData.Regions GetRegions()
        {
            Models.StaticData.Regions regs = new Models.StaticData.Regions();
            regs.regions = new List<Models.StaticData.Region>();

            JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
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

            JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
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

        public Models.StaticData.Manures GetManures()
        {
            Models.StaticData.Manures mans = new Models.StaticData.Manures();
            mans.manures = new List<Models.StaticData.Manure>();

            JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
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

            JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
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

            JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
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

            JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
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

            JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
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

        public Models.StaticData.CropTypes GetCropTypes()
        {
            Models.StaticData.CropTypes types = new Models.StaticData.CropTypes();
            types.cropTypes = new List<Models.StaticData.CropType>();

            JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray array = (JArray)rss["agri"]["nmp"]["croptypes"]["croptype"];

            foreach (var r in array)
            {
                Models.StaticData.CropType type = new Models.StaticData.CropType();
                type.id = Convert.ToInt32(r["id"].ToString());
                type.name = r["name"].ToString();
                types.cropTypes.Add(type);
            }

            return types;
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

            JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray array = (JArray)rss["agri"]["nmp"]["crops"]["crop"];

            foreach (var r in array)
            {

                Models.StaticData.Crop crop = new Models.StaticData.Crop();
                crop.cropname = r["cropname"].ToString();
                crop.cropremovalfactor = r["cropremovalfactor"].ToString() == "null" ? (decimal?)null : Convert.ToDecimal(r["cropremovalfactor"].ToString());
                crop.croptypeid = Convert.ToInt32(r["croptypeid"].ToString());
                crop.id = Convert.ToInt32(r["id"].ToString());
                crop.n_high_lbperac = r["n_high_lbperac"].ToString() == "null" ? (decimal?)null : Convert.ToDecimal(r["n_high_lbperac"].ToString());
                crop.n_recommcd = Convert.ToDecimal(r["n_recommcd"].ToString());
                crop.n_recomm_lbperac = r["n_recomm_lbperac"].ToString() == "null" ? (decimal?)null : Convert.ToDecimal(r["n_recomm_lbperac"].ToString());
                crop.prevcropcd = Convert.ToInt32(r["prevcropcd"].ToString());
                crop.value_KO5 = r["KO5"].ToString() == "null" ? (decimal?)null : Convert.ToDecimal(r["KO5"].ToString());
                crop.value_P2O5 = r["P2O5"].ToString() == "null" ? (decimal?)null : Convert.ToDecimal(r["P2O5"].ToString());
                crop.yieldcd = Convert.ToInt32(r["yieldcd"].ToString());
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

            JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JObject r = (JObject)rss["agri"]["nmp"]["crops"]["crop"].FirstOrDefault(x => x["id"].ToString() == cropId.ToString());
            Models.StaticData.Crop crop = new Models.StaticData.Crop();

            crop.cropname = r["cropname"].ToString();
            crop.cropremovalfactor = r["cropremovalfactor"].ToString() == "null" ? (decimal?)null : Convert.ToDecimal(r["cropremovalfactor"].ToString());
            crop.croptypeid = Convert.ToInt32(r["croptypeid"].ToString());
            crop.id = Convert.ToInt32(r["id"].ToString());
            crop.n_high_lbperac = r["n_high_lbperac"].ToString() == "null" ? (decimal?)null : Convert.ToDecimal(r["n_high_lbperac"].ToString());
            crop.n_recommcd = Convert.ToDecimal(r["n_recommcd"].ToString());
            crop.n_recomm_lbperac = r["n_recomm_lbperac"].ToString() == "null" ? (decimal?)null : Convert.ToDecimal(r["n_recomm_lbperac"].ToString());
            crop.prevcropcd = Convert.ToInt32(r["prevcropcd"].ToString());
            crop.value_KO5 = r["KO5"].ToString() == "null" ? (decimal?)null : Convert.ToDecimal(r["KO5"].ToString());
            crop.value_P2O5 = r["P2O5"].ToString() == "null" ? (decimal?)null : Convert.ToDecimal(r["P2O5"].ToString());

            crop.yieldcd = Convert.ToInt32(r["yieldcd"].ToString());

            return crop;
        }

        public Models.StaticData.Yield GetYield(int yieldId)
        {

            JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
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

        public Models.StaticData.Crop_STP_RegionCd GetCrop_STP_RegionCd(int cropid, int regionid)
        {

            JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray array = (JArray)rss["agri"]["nmp"]["crop_stp_regioncds"]["crop_stp_regioncd"];
            Models.StaticData.Crop_STP_RegionCd crop_stp_regioncd = new Models.StaticData.Crop_STP_RegionCd();

            foreach (var r in array)
            {
                if (Convert.ToInt32(r["cropid"].ToString()) == cropid &&
                    Convert.ToInt32(r["regionid"].ToString()) == regionid)
                {
                    crop_stp_regioncd.cropid = Convert.ToInt32(r["cropid"].ToString());
                    crop_stp_regioncd.regionid = Convert.ToInt32(r["regionid"].ToString());
                    crop_stp_regioncd.regioncd = Convert.ToInt32(r["regioncd"].ToString());
                }
            }

            return crop_stp_regioncd;
        }

        public Models.StaticData.Crop_STK_RegionCd GetCrop_STK_RegionCd(int cropid, int regionid)
        {

            JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray array = (JArray)rss["agri"]["nmp"]["crop_stk_regioncds"]["crop_stk_regioncd"];
            Models.StaticData.Crop_STK_RegionCd crop_stk_regioncd = new Models.StaticData.Crop_STK_RegionCd();

            foreach (var r in array)
            {
                if (Convert.ToInt32(r["cropid"].ToString()) == cropid &&
                    Convert.ToInt32(r["regionid"].ToString()) == regionid)
                {
                    crop_stk_regioncd.cropid = Convert.ToInt32(r["cropid"].ToString());
                    crop_stk_regioncd.regionid = Convert.ToInt32(r["regionid"].ToString());
                    crop_stk_regioncd.regioncd = Convert.ToInt32(r["regioncd"].ToString());
                }
            }

            return crop_stk_regioncd;
        }

        public Models.StaticData.DM GetDM(int ID)
        {

            JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
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

            JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray array = (JArray)rss["agri"]["nmp"]["ammoniaretentions"]["ammoniaretention"];
            Models.StaticData.AmmoniaRetention ammoniaretention = new Models.StaticData.AmmoniaRetention();

            foreach (var r in array)
            {
                if (Convert.ToInt32(r["seasonapplicatonid"].ToString()) == seasonApplicatonId &&
                    Convert.ToInt32(r["dm"].ToString()) == dm)
                {
                    ammoniaretention.seasonapplicatonid = Convert.ToInt32(r["seasonapplicatonid"].ToString());
                    ammoniaretention.dm = Convert.ToInt32(r["dm"].ToString());
                    ammoniaretention.value = Convert.ToDecimal(r["value"].ToString());
                }
            }

            return ammoniaretention;
        }

        public Models.StaticData.NMineralization GetNMineralization(int id, int locationid)
        {

            JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
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
        public Models.StaticData.SoilTestMethods GetSoilTestMethods()
        {
            Models.StaticData.SoilTestMethods meths = new Models.StaticData.SoilTestMethods();
            meths.methods = new List<Models.StaticData.SoilTestMethod>();

            JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray items = (JArray)rss["agri"]["nmp"]["soiltestmethods"]["soiltestmethod"];

            foreach (var r in items)
            {
                Models.StaticData.SoilTestMethod rec = new Models.StaticData.SoilTestMethod();

                rec.id = Convert.ToInt32(r["id"].ToString());
                rec.name = r["name"].ToString();
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

            JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
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
      
        public Models.StaticData.PrevCropTypes GetPrevCropTypes()
        {
            Models.StaticData.PrevCropTypes types = new Models.StaticData.PrevCropTypes();
            types.prevCropTypes = new List<Models.StaticData.PrevCropType>();

            JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(_ctx.HttpContext.Session.Get("Static")));
            JArray array = (JArray)rss["agri"]["nmp"]["prevcroptypes"]["prevcroptype"];

            foreach (var r in array)
            {
                Models.StaticData.PrevCropType type = new Models.StaticData.PrevCropType();
                type.id = Convert.ToInt32(r["id"].ToString());
                type.cropType = Convert.ToInt32(r["croptype"].ToString());
                type.name = r["name"].ToString();
                type.nCreditMetric = Convert.ToInt32(r["ncreditmetric"].ToString());
                type.nCreditImperial = Convert.ToInt32(r["ncreditimperial"].ToString());
                types.prevCropTypes.Add(type);
            }

            return types;
        }

        public List<Models.StaticData.SelectListItem> GetPrevCropTypesDll(string cropType)
        {
            Models.StaticData.PrevCropTypes types = GetPrevCropTypes();

            List<Models.StaticData.SelectListItem> typesOptions = new List<Models.StaticData.SelectListItem>();

            foreach (var r in types.prevCropTypes)
            {
                if (r.cropType.ToString() == cropType)
                {
                    Models.StaticData.SelectListItem li = new Models.StaticData.SelectListItem() { Id = r.id, Value = r.name };
                    typesOptions.Add(li);
                }
            }

            return typesOptions;

        }
    }
}

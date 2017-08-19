using System;
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
        public Models.StaticData.Regions GetRegions(HttpContext ctx)
        {
            Models.StaticData.Regions regs = new Models.StaticData.Regions();
            regs.regions = new List<Models.StaticData.Region>();

            JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(ctx.Session.Get("Static")));
            JArray regions = (JArray)rss["agri"]["nmp"]["regions"]["region"];

            foreach (var r in regions)
            {
                Models.StaticData.Region reg = new Models.StaticData.Region();

                reg.id = Convert.ToInt32(r["id"].ToString());
                reg.name = r["name"].ToString();
                reg.location = r["location"].ToString();
                reg.soil_test_phospherous_region_cd = Convert.ToInt32(r["soil_test_phospherous_region_cd"].ToString());
                reg.soil_test_potassium_region_cd = Convert.ToInt32(r["soil_test_potassium_region_cd"].ToString());
                regs.regions.Add(reg);
            }

            return regs;
        }

        public List<Models.StaticData.SelectListItem> GetRegionsDll(HttpContext ctx)
        {
            Models.StaticData.Regions regs = GetRegions(ctx);

            List <Models.StaticData.SelectListItem> regOptions = new List<Models.StaticData.SelectListItem>();

            foreach(var r in regs.regions)
            {
                Models.StaticData.SelectListItem li = new Models.StaticData.SelectListItem() { Id = r.id, Value = r.name } ;
                regOptions.Add(li);
            }

            return regOptions;
        }
        public Models.StaticData.Manure GetManure(HttpContext ctx, string manId )
        {
            Models.StaticData.Manure man = new Models.StaticData.Manure();

            JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(ctx.Session.Get("Static")));
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
                }
            }

            return man;
        }
        public Models.StaticData.Manures GetManures(HttpContext ctx)
        {
            Models.StaticData.Manures mans = new Models.StaticData.Manures();
            mans.manures = new List<Models.StaticData.Manure>();

            JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(ctx.Session.Get("Static")));
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
                mans.manures.Add(man);
            }

            return mans;
        }
        public List<Models.StaticData.SelectListItem> GetManuresDll(HttpContext ctx)
        {
            Models.StaticData.Manures mans = GetManures(ctx);

            List<Models.StaticData.SelectListItem> manOptions = new List<Models.StaticData.SelectListItem>();

            foreach (var r in mans.manures)
            {
                Models.StaticData.SelectListItem li = new Models.StaticData.SelectListItem() { Id = r.id, Value = r.name };
                manOptions.Add(li);
            }

            return manOptions;
        }
        public Models.StaticData.Season_Application GetApplication(HttpContext ctx, string applId)
        {
            Models.StaticData.Season_Application appl = new Models.StaticData.Season_Application();

            JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(ctx.Session.Get("Static")));
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
        public Models.StaticData.Season_Applications GetApplications(HttpContext ctx)
        {
            Models.StaticData.Season_Applications appls = new Models.StaticData.Season_Applications();
            appls.season_applications = new List<Models.StaticData.Season_Application>();

            JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(ctx.Session.Get("Static")));
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

        public List<Models.StaticData.SelectListItem> GetApplicationsDll(HttpContext ctx)
        {
            Models.StaticData.Season_Applications appls = GetApplications(ctx);

            List<Models.StaticData.SelectListItem> applsOptions = new List<Models.StaticData.SelectListItem>();

            foreach (var r in appls.season_applications)
            {
                Models.StaticData.SelectListItem li = new Models.StaticData.SelectListItem() { Id = r.id, Value = r.name };
                applsOptions.Add(li);
            }

            return applsOptions;
        }
        public Models.StaticData.Units GetUnits(HttpContext ctx)
        {
            Models.StaticData.Units units = new Models.StaticData.Units();
            units.units = new List<Models.StaticData.Unit>();

            JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(ctx.Session.Get("Static")));
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

        public List<Models.StaticData.SelectListItem> GetUnitsDll(HttpContext ctx, string unitType)
        {
            Models.StaticData.Units units = GetUnits(ctx);

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
    }
}

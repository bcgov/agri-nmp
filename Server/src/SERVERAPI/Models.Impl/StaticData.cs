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
        /// <summary>
        /// Get region values
        /// </summary>
        /// <param name="text">text for the history entry</param>
        /// <param name="smUserId">Site Minder User ID for the history entry</param>
        public Models.StaticData.Regions GetRegions(HttpContext ctx)
        {
            Models.StaticData.Regions regs = new Models.StaticData.Regions();
            regs.regions = new List<Models.StaticData.Region>();

            JObject rss = JObject.Parse(System.Text.Encoding.UTF8.GetString(ctx.Session.Get("Static")));
            JArray regions = (JArray)rss["agri"]["nmp"]["regions"]["region"];

            foreach (var r in regions)
            {
                Models.StaticData.Region reg = new Models.StaticData.Region();

                reg.id = Convert.ToInt32(r["-id"].ToString());
                reg.name = r["-name"].ToString();
                reg.location = r["-Location"].ToString();
                reg.p_regioncd = Convert.ToInt32(r["-SoilTestPhospherousRegionCd"].ToString());
                reg.k_regioncd = Convert.ToInt32(r["-SoilTestPotassiumtRegionCd"].ToString());
                regs.regions.Add(reg);
            }

            return regs;
        }

        public List<Models.StaticData.SelectListItem> GetRegionsDll(HttpContext ctx)
        {
            Models.StaticData.Regions regs = GetRegions(ctx);

            List <Models.StaticData.SelectListItem> RegOptions = new List<Models.StaticData.SelectListItem>();

            foreach(var r in regs.regions)
            {
                Models.StaticData.SelectListItem li = new Models.StaticData.SelectListItem() { Id = r.id, Value = r.name } ;
                RegOptions.Add(li);
            }

            return RegOptions;
        }
    }
}

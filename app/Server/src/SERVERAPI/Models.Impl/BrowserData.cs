using Agri.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using Version = System.Version;

namespace SERVERAPI.Models.Impl
{
    public partial class BrowserData
    {
        private readonly IHttpContextAccessor _ctx;
        private readonly IAgriConfigurationRepository _sd;
        public string BrowserName { get; }
        public string BrowserVersion { get; }
        public bool BrowserValid { get; }
        public bool OSValid { get; }
        public bool BrowserOutofdate { get; }
        public string BrowserAgent { get; }
        public string BrowserUpdate { get; }
        public string BrowserOs { get; }

        public BrowserData(IHttpContextAccessor ctx, IAgriConfigurationRepository sd)
        {
            _ctx = ctx;
            _sd = sd;

            try
            {
                UserAgent.UserAgent ua = new UserAgent.UserAgent(_ctx.HttpContext.Request.Headers["User-Agent"]);
                BrowserName = ua.Browser.Name;
                BrowserVersion = ua.Browser.Version;
                BrowserOs = ua.OS.Name;
                BrowserAgent = _ctx.HttpContext.Request.Headers["User-Agent"].ToString();

                var ab = _sd.GetAllowableBrowsers();
                var browser = ab.Where(a => a.Name.Equals(BrowserName, StringComparison.CurrentCultureIgnoreCase))
                                    .SingleOrDefault();   //known.FindIndex(r => r.name == BrowserName);
                if (BrowserOs == "iOS")
                {
                    OSValid = false;
                }
                else
                {
                    OSValid = true;
                    if (browser == null)
                    {
                        BrowserValid = false;
                        BrowserName = "Unknown";
                    }
                    else
                    {
                        BrowserValid = true;
                        var minVer = Version.Parse(browser.MinVersion);
                        var thisVer = Version.Parse(BrowserVersion);
                        if (thisVer < minVer)
                        {
                            BrowserOutofdate = true;
                        }
                        else
                        {
                            BrowserOutofdate = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BrowserName = "Unknown";
                throw new Exception("Could not retrieve browser type.!", ex);
            }
        }
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;

namespace SERVERAPI.Services.Impl
{
    public abstract class ServiceBase
    {
        private readonly ILogger<ServiceBase> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ServiceBase(ILogger<ServiceBase> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        protected HttpRequest Request
        {
            get { return _httpContextAccessor.HttpContext.Request; }
        }

        protected ClaimsPrincipal User
        {
            get { return _httpContextAccessor.HttpContext.User; }
        }

        protected OkObjectResult Ok(object value)
        {
            return new OkObjectResult(value);
        }

        // parse a string of ints into an array.
        public int?[] ParseIntArray(string source)
        {
            int?[] result = null;
            try
            {
                string[] tokens = source.Split(',');
                result = new int?[tokens.Length];
                for (int i = 0; i < tokens.Length; i++)
                {
                    result[i] = int.Parse(tokens[i]);
                }
            }
            catch (Exception e)
            {
                result = null;
                _logger.LogError(e, "ParseIntArray exception");
            }
            return result;
        }
    }
}
using ApothecaricApi.Data;
using ApothecaricApi.Models.Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ApothecaricApi.Middleware
{
    public class TenantProviderMiddleware
    {
        private readonly RequestDelegate next;

        public TenantProviderMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public Task Invoke(HttpContext httpContext, ApothecaricDbContext dbContext)
        {
            string urlHost = httpContext.Request.Host.ToString();

            if (string.IsNullOrEmpty(urlHost))
            {
                throw new ApplicationException("urlHost must be specified");
            }

            urlHost = urlHost.Remove(urlHost.IndexOf(":"), urlHost.Length - urlHost.IndexOf(":")).ToLower().Trim();

            Tenant tenant = dbContext.Tenants.FirstOrDefault(a => a.DomainName.ToLower() == urlHost);

            if (tenant == null)
            {
                tenant = dbContext.Tenants.FirstOrDefault(a => a.IsDefault);

                if (tenant == null)
                {
                    throw new ApplicationException("tenant not found based on URL, no default found");
                }
            }

            httpContext.Items.Add("TENANT", tenant);

            return next(httpContext);
        }
    }
}

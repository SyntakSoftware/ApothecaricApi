using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApothecaricApi.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApothecaricApi.Controllers
{
    public class MultiTenantController : Controller
    {
        public Tenant Tenant
        {
            get
            {
                object multiTenant;

                if (!HttpContext.Items.TryGetValue("TENANT", out multiTenant))
                    throw new ApplicationException("Could not find tenant.");

                return (Tenant)multiTenant;
            }
        }
    }
}
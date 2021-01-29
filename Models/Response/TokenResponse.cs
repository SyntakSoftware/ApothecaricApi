using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApothecaricApi.Models.Response
{
    public class TokenResponse
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string TenantCode { get; set; }

        public DateTime TokenExpiration { get; set; }
    }
}

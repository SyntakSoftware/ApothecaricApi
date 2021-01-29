using ApothecaricApi.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApothecaricApi.Data
{
    public class ApothecaricUserManager : UserManager<ApothecaricUser>
    {
        public ApothecaricUserManager(IUserStore<ApothecaricUser> store, IOptions<IdentityOptions> optionsAccessor,
                                     IPasswordHasher<ApothecaricUser> passwordHasher, IEnumerable<IUserValidator<ApothecaricUser>> userValidators,
                                     IEnumerable<IPasswordValidator<ApothecaricUser>> passwordValidators, ILookupNormalizer keyNormalizer,
                                     IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApothecaricUser>> logger)
           : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger) { }

        public virtual Task<ApothecaricUser> FindByNameAndTenantAsync(string normalizedUserName, string tenantId)
        {
            return Task.FromResult(base.Users.Where(u => u.NormalizedUserName == normalizedUserName.ToUpper().Trim() && u.TenantId == tenantId).SingleOrDefault());
        }
    }
}

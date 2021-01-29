using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ApothecaricApi.Models.Identity
{
    public class ApothecaricUser : IdentityUser
    {
        [StringLength(250)]
        public string FirstName { get; set; }

        [StringLength(250)]
        public string LastName { get; set; }

        public bool IsActive { get; set; }

        [StringLength(450)]
        public string TenantId { get; set; }

        public virtual Tenant Tenant { get; set; }

    }
}

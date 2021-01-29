using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApothecaricApi.Models.Identity
{
    [Table("AspNetTenants")]
    public class Tenant
    {
        [Key]
        [StringLength(450)]
        public string Id { get; set; }

        [StringLength(250)]
        [Required]
        public string Name { get; set; }

        [StringLength(250)]
        [Required]
        public string DomainName { get; set; }

        [StringLength(50)]
        [Required]
        public string Code { get; set; }

        public bool IsDefault { get; set; }

        public virtual IEnumerable<ApothecaricUser> Users { get; set; }
    }
}


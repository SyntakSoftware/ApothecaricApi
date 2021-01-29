using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApothecaricApi.Models.Identity
{
    [Table("AspNetRefreshTokens")]
    public class RefreshToken
    {
        [Key]
        [StringLength(450)]
        public string Id { get; set; }

        public DateTime IssuedUtc { get; set; }

        public DateTime ExpiresUtc { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(450)]
        public string Token { get; set; }

        [StringLength(450)]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApothecaricUser User { get; set; }
    }
}

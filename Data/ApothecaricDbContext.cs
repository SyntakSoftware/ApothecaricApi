using ApothecaricApi.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApothecaricApi.Data
{
    public class ApothecaricDbContext : IdentityDbContext<ApothecaricUser>
    {
        public ApothecaricDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Tenant> Tenants { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Tenant>()
                .HasIndex(u => u.Code)
                .IsUnique();

            builder.Entity<Tenant>()
                .HasIndex(u => u.DomainName)
                .IsUnique();

            builder.Entity<RefreshToken>()
               .HasAlternateKey(c => c.UserId)
               .HasName("refreshToken_UserId");

            builder.Entity<RefreshToken>()
                .HasAlternateKey(c => c.Token)
                .HasName("refreshToken_Token");

            base.OnModelCreating(builder);
        }
    }
}

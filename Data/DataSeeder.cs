using ApothecaricApi.Models.Identity;
using System.Linq;

namespace ApothecaricApi.Data
{
    public class DataSeeder
    {
        private readonly ApothecaricDbContext dbContext;

        public DataSeeder(ApothecaricDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Seed()
        {
            dbContext.Database.EnsureCreated();

            if (!dbContext.Tenants.Any())
            {
                dbContext.Add(new Tenant()
                {
                    Id = "f28ab858-5ed0-4b10-85f9-6f93ac67f5e0",
                    Code = "LH",
                    DomainName = "localhost",
                    IsDefault = true,
                    Name = "localhost",
                });
            }

            if (!dbContext.Users.Any())
            {
                dbContext.Add(new ApothecaricUser()
                {
                    AccessFailedCount = 0,
                    ConcurrencyStamp = "c9001bc5-8d72-45cf-8af9-a72a2d13876f",
                    Email = "testuser@test.com",
                    EmailConfirmed = false,
                    FirstName = "Test",
                    Id = "4fe1bbfd-6d51-49a9-a252-98b7e0f4fdef",
                    IsActive = true,
                    LastName = "User",
                    LockoutEnabled = true,
                    LockoutEnd = null,
                    NormalizedEmail = "TESTUSER@TEST.COM",
                    NormalizedUserName = "TESTUSER@TEST.COM",
                    PasswordHash = "AQAAAAEAACcQAAAAEOP6R31Cv8n1pEIAcXBdaxHuiLd++CWlrEi3lzqA7QzZGbhv/AAON1L2fJBvskIzrA==", //coolPassword#1
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    SecurityStamp = "G4SRFP7YSPJA4NO5E5VB6FCEVH3VNG65",
                    TwoFactorEnabled = false,
                    UserName = "testuser@test.com",
                    TenantId = "f28ab858-5ed0-4b10-85f9-6f93ac67f5e0"
                });
            }


            dbContext.SaveChanges();
        }
    }
}

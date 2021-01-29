using ApothecaricApi.Data;
using ApothecaricApi.Filters;
using ApothecaricApi.Models.Identity;
using ApothecaricApi.Models.Response;
using ApothecaricApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApothecaricApi.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : MultiTenantController
    {
        private readonly IConfiguration configuration;
        private readonly ApothecaricUserManager userManager;
        private readonly SignInManager<ApothecaricUser> signinManager;
        private readonly ApothecaricDbContext dbContext;

        public AccountController(IConfiguration configuration, ApothecaricUserManager userManager, 
                                 SignInManager<ApothecaricUser> signinManager, ApothecaricDbContext context)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.signinManager = signinManager;
            this.dbContext = context;
        }

        [HttpPost]
        [ApiValidationFilter]
        [Route("Register")]
        public async Task<IActionResult> TempRegister([FromBody] RegisterViewModel model)
        {
            var user = new ApothecaricUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
                return Created("", result);

            return BadRequest();
        }

        [HttpPost]
        [AllowAnonymous]
        [ApiValidationFilter]
        [Route("Token")]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel login)
        {
            var user = await userManager.FindByNameAndTenantAsync(login.Email, Tenant.Id);

            if (user != null)
            {
                var result = await signinManager.CheckPasswordSignInAsync(user, login.Password, false);

                if (result.Succeeded)
                {

                    var refreshDbToken = dbContext.RefreshTokens.SingleOrDefault(t => t.UserId == user.Id);

                    if (refreshDbToken != null)
                    {
                        dbContext.RefreshTokens.Remove(refreshDbToken);
                        await dbContext.SaveChangesAsync();
                    }

                    var newRefreshToken = new RefreshToken
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = user.Id,
                        Token = Guid.NewGuid().ToString(),
                        IssuedUtc = DateTime.Now.ToUniversalTime(),
                        ExpiresUtc = DateTime.Now.AddMinutes(3).ToUniversalTime()
                    };

                    dbContext.RefreshTokens.Add(newRefreshToken);
                    await dbContext.SaveChangesAsync();

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Tokens:Key"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var nowUtc = DateTime.Now.ToUniversalTime();
                    var expires = nowUtc.AddMinutes(double.Parse(configuration["Tokens:ExpiryMinutes"])).ToUniversalTime();

                    var token = new JwtSecurityToken(
                        configuration["Tokens:Issuer"],
                        configuration["Tokens:Audience"],
                        null,
                        expires: expires,
                        signingCredentials: creds);

                    var response = new TokenResponse
                    {
                        AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                        RefreshToken = newRefreshToken.Token,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        TenantCode = Tenant.Code,
                        TokenExpiration = token.ValidTo
                    };
                    
                    return Ok(response);
                }

                return BadRequest();
            }

            return BadRequest();
        }

        [HttpPost]
        [AllowAnonymous]
        [ApiValidationFilter]
        [Route("Token/Refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshToken refreshToken)
        {

            var refreshTokenFromDatabase = dbContext.RefreshTokens
                                               .Include(x => x.User)
                                               .SingleOrDefault(i => i.Token == refreshToken.Token);

            if (refreshTokenFromDatabase == null)
                return BadRequest("invalid_grant");

            if (refreshTokenFromDatabase.ExpiresUtc < DateTime.Now.ToUniversalTime())
                return BadRequest("invalid_grant");

            if (!await signinManager.CanSignInAsync(refreshTokenFromDatabase.User))
                return BadRequest("invalid_grant");

            if (userManager.SupportsUserLockout && await userManager.IsLockedOutAsync(refreshTokenFromDatabase.User))
                return BadRequest("invalid_grant");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var nowUtc = DateTime.Now.ToUniversalTime();
            var expires = nowUtc.AddMinutes(double.Parse(configuration["Tokens:ExpiryMinutes"])).ToUniversalTime();

            var token = new JwtSecurityToken(
                configuration["Tokens:Issuer"],
                configuration["Tokens:Audience"],
                null,
                expires: expires,
                signingCredentials: creds);

            var response = new TokenResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshTokenFromDatabase.Token,
                FirstName = refreshTokenFromDatabase.User.FirstName,
                LastName = refreshTokenFromDatabase.User.LastName,
                TenantCode = Tenant.Code,
                TokenExpiration = token.ValidTo
            };

            return Ok(response);
        }
    }
}
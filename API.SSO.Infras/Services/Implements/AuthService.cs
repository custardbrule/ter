using API.SSO.Domain;
using API.SSO.Infras.Shared;
using API.SSO.Infras.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace API.SSO.Infras.Services.Implements
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
        }

        public async Task<(string AccessToken, string RefreshToken)> GenerateJwt(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) throw new AppException(string.Format(ValidationConstant.NOTFOUND, email), HttpStatusCode.NotFound);

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!signInResult.Succeeded) throw new AppException(string.Format(ValidationConstant.INVALID, nameof(password)), HttpStatusCode.BadRequest);

            var principal = await _signInManager.CreateUserPrincipalAsync(user);

            var accessToken = GenerateJwt(principal.Claims);
            var refreshToken = GenerateJwtRefresh(user.Id.ToString());


            return (accessToken, refreshToken);
        }

        public async Task<(string AccessToken, string RefreshToken)> RefreshJwt(string token, string refresh, CancellationToken cancellationToken = default)
        {
            _ = GetPrincipalFromRefresh(refresh);
            var oldPrincipal = GetPrincipalFromExpiredToken(token) ?? throw new AppException("", HttpStatusCode.BadRequest);
            var id = oldPrincipal.Claims.First(v => v.Type is Claims.Subject).Value;

            var user = await _userManager.FindByIdAsync(id) ?? throw new AppException(string.Format(ValidationConstant.NOTFOUND, id), HttpStatusCode.NotFound);
            var newPrincipal = await _signInManager.CreateUserPrincipalAsync(user);

            var accessToken = GenerateJwt(newPrincipal.Claims);
            var refreshToken = GenerateJwtRefresh(user.Id.ToString());


            return (accessToken, refreshToken);
        }

        #region validate
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken) throw new SecurityTokenException("Invalid token");

            return principal;
        }

        private ClaimsPrincipal GetPrincipalFromRefresh(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:RefreshSecret"]!)),
                ValidateLifetime = true
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken) throw new SecurityTokenException("Invalid token");

            return principal;
        }
        #endregion

        #region generate
        private string GenerateJwt(IEnumerable<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(_config.GetRequiredSection("Jwt:ExpInMinute").Get<int>()),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateJwtRefresh(string id)
        {

            // generate access token
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:RefreshSecret"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new Claim[] { new Claim(Claims.Subject, id) };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion
    }
}

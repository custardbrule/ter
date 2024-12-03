using API.SSO.Domain;
using API.SSO.Infras.Shared;
using API.SSO.Infras.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
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

        public async Task<(string AccessToken, string RefreshToken, int ExpiredTime)> GenerateJwt(string email, string password, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) throw new AppException(string.Format(ValidationConstant.NOTFOUND, email), HttpStatusCode.NotFound);

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!signInResult.Succeeded) throw new AppException(string.Format(ValidationConstant.INVALID, nameof(password)), HttpStatusCode.BadRequest);

            // generate access token
            var principal = await _signInManager.CreateUserPrincipalAsync(user);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expiredTime = _config.GetRequiredSection("Jwt:ExpInMinute").Get<int>();

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                principal.Claims,
                expires: DateTime.Now.AddMinutes(expiredTime),
              signingCredentials: credentials);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            // generate refresh token
            var refreshToken = Convert.ToBase64String(user.Id.ToByteArray());


            return (accessToken, refreshToken, expiredTime);
        }

        public async Task<(string AccessToken, string RefreshToken, int ExpiredTime)> GenerateJwt(string id, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) throw new AppException(string.Format(ValidationConstant.NOTFOUND, id), HttpStatusCode.NotFound);

            // generate access token
            var principal = await _signInManager.CreateUserPrincipalAsync(user);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expiredTime = _config.GetRequiredSection("Jwt:ExpInMinute").Get<int>();

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                principal.Claims,
                expires: DateTime.Now.AddMinutes(expiredTime),
              signingCredentials: credentials);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            // generate refresh token
            var refreshToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(id));


            return (accessToken, refreshToken, expiredTime);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
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
    }
}

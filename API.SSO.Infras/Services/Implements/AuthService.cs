using API.SSO.Domain;
using API.SSO.Infras.Shared;
using API.SSO.Infras.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

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
            var identity = await GetClaimsIdentity(token) ?? throw new AppException("", HttpStatusCode.BadRequest);
            var id = identity.Claims.First(v => v.Type is ClaimTypes.NameIdentifier).Value;

            var user = await _userManager.FindByIdAsync(id) ?? throw new AppException(string.Format(ValidationConstant.NOTFOUND, id), HttpStatusCode.NotFound);
            var principal = await _signInManager.CreateUserPrincipalAsync(user);

            var accessToken = GenerateJwt(principal.Claims);
            var refreshToken = GenerateJwtRefresh(user.Id.ToString());


            return (accessToken, refreshToken);
        }

        #region validate
        private async Task<ClaimsIdentity> GetClaimsIdentity(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!)),
                ValidateLifetime = false
            };

            var jsonWebTokenHandler = new JsonWebTokenHandler();
            var tokenValidationResult = await jsonWebTokenHandler.ValidateTokenAsync(token, tokenValidationParameters);
            if (!tokenValidationResult.IsValid) throw new SecurityTokenException("Invalid token");

            return tokenValidationResult.ClaimsIdentity;
        }

        private async Task<ClaimsIdentity> GetPrincipalFromRefresh(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:RefreshSecret"]!)),
                ValidateLifetime = true,
            };

            var jsonWebTokenHandler = new JsonWebTokenHandler();
            var tokenValidationResult = await jsonWebTokenHandler.ValidateTokenAsync(token, tokenValidationParameters);
            if (!tokenValidationResult.IsValid) throw new SecurityTokenException("Invalid token");

            return tokenValidationResult.ClaimsIdentity;

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
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, id) };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion
    }
}

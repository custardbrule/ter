using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;
using System.Security.Claims;
using OpenIddict.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using OpenIddict.Validation.AspNetCore;
using MediatR;
using API.SSO.Infras.Features.UserManagement.Commands;
using API.SSO.Domain;

namespace API.SSO.Controllers
{
    [Route("[controller]/[action]")]
    public class AuthenticateController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthenticateController(IConfiguration config, IMediator mediator, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _config = config;
            _mediator = mediator;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login()
        {
            var user = await _userManager.FindByEmailAsync("torinoutacnh@gmail.com");
            var claims = await _userManager.GetClaimsAsync(user);
            claims.Add(new Claim(Claims.Subject, user.Id.ToString()));
            var identity = new ClaimsIdentity(claims, TokenValidationParameters.DefaultAuthenticationType, Claims.Name, Claims.Role);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              identity.Claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            var res = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(res);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
        public string Get()
        {
            return "Wala";
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest req)
        {
            await _mediator.Send(req);
            return NoContent();
        }
    }
}

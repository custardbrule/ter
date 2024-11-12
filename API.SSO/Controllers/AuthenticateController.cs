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
using API.SSO.Infras.Features.UserManagement.Queries;
using Azure.Core;

namespace API.SSO.Controllers
{
    [Route("[controller]/[action]")]
    [AllowAnonymous]
    public class AuthenticateController : Controller
    {
        private readonly IMediator _mediator;

        public AuthenticateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginRequest request) => Ok(await _mediator.Send(request));

        [HttpGet]
        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
        public string Get()
        {
            return "Wala";
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request) => Ok(await _mediator.Send(request));
    }
}

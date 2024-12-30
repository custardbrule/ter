using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using API.SSO.Infras.Features.UserManagement.Commands;
using API.SSO.Infras.Features.UserManagement.Queries;

namespace API.SSO.Controllers
{
    [Route("[controller]/[action]")]
    public class AuthenticateController : Controller
    {
        private readonly IMediator _mediator;

        public AuthenticateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request) => Ok(await _mediator.Send(request));

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request) => Ok(await _mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request) => Ok(await _mediator.Send(request));
    }
}

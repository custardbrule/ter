using API.SSO.Domain;
using API.SSO.Infras.Shared;
using API.SSO.Infras.Shared.Exceptions;
using FluentValidation;
using MediatR;
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

namespace API.SSO.Infras.Features.UserManagement.Queries
{
    public record LoginResponse(string AccessToken);
    public record LoginRequest(string Email, string Password) : IRequest<LoginResponse>;

    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(r => r.Email)
                .NotEmpty().WithMessage(string.Format(ValidationConstant.NOTNULL, nameof(LoginRequest.Email)))
                .EmailAddress().WithMessage(string.Format(ValidationConstant.INVALIDFORMAT, nameof(LoginRequest.Email)));
        }
    }

    public class LoginRequestHandler : IRequestHandler<LoginRequest, LoginResponse>
    {
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginRequestHandler(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
        }

        public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) throw new AppException(string.Format(ValidationConstant.NOTFOUND, request.Email), HttpStatusCode.NotFound);

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!signInResult.Succeeded) throw new AppException(string.Format(ValidationConstant.INVALID, nameof(request.Password)), HttpStatusCode.BadRequest);

            var principle = await _signInManager.CreateUserPrincipalAsync(user);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                principle.Claims,
                expires: DateTime.Now.AddMinutes(_config.GetRequiredSection("Jwt:ExpInMinute").Get<int>()),
              signingCredentials: credentials);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new LoginResponse(accessToken);
        }
    }
}

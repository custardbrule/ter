using API.SSO.Domain;
using API.SSO.Infras.Services;
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
    public record LoginResponse(string AccessToken, string RefreshToken);
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
        private readonly IAuthService _authService;

        public LoginRequestHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var (AccessToken, RefreshToken) = await _authService.GenerateJwt(request.Email, request.Password, cancellationToken);
            return new LoginResponse(AccessToken, RefreshToken);
        }
    }
}

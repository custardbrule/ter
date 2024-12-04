using API.SSO.Infras.Services;
using API.SSO.Infras.Shared;
using API.SSO.Infras.Shared.Exceptions;
using Azure.Core;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.SSO.Infras.Features.UserManagement.Queries
{
    public record RefreshResponse(string AccessToken, string RefreshToken);
    public record RefreshRequest(string Access, string Refresh) : IRequest<RefreshResponse>;

    public class RefreshRequestValidator : AbstractValidator<RefreshRequest>
    {
        public RefreshRequestValidator()
        {
            RuleFor(r => r.Access)
                .NotEmpty().WithMessage(string.Format(ValidationConstant.NOTNULL, nameof(RefreshRequest.Access)));

            RuleFor(r => r.Refresh)
                .NotEmpty().WithMessage(string.Format(ValidationConstant.NOTNULL, nameof(RefreshRequest.Refresh)));
        }
    }

    public class RefreshRequestHandler : IRequestHandler<RefreshRequest, RefreshResponse>
    {
        private readonly IAuthService _authService;

        public RefreshRequestHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<RefreshResponse> Handle(RefreshRequest request, CancellationToken cancellationToken)
        {
            var (AccessToken, RefreshToken) = await _authService.RefreshJwt(request.Access, request.Refresh, cancellationToken);
            return new RefreshResponse(AccessToken, RefreshToken);
        }
    }
}

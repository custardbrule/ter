﻿using API.SSO.Domain;
using API.SSO.Infras.Features.UserManagement;
using API.SSO.Infras.Services;
using API.SSO.Infras.Shared;
using API.SSO.Infras.Shared.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.SSO.Infras.Features.UserManagement
{
    public record RegisterRequest(string FirstName, string LastName, string Email, string PhoneNumber, string Password) : IRequest;

    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(r => r.FirstName)
                .NotEmpty().WithMessage(string.Format(ValidationConstant.NOTNULL, nameof(RegisterRequest.FirstName)));
            RuleFor(r => r.LastName)
                .NotEmpty().WithMessage(string.Format(ValidationConstant.NOTNULL, nameof(RegisterRequest.LastName)));
            RuleFor(r => r.Email)
                .NotEmpty().WithMessage(string.Format(ValidationConstant.NOTNULL, nameof(RegisterRequest.Email)))
                .EmailAddress().WithMessage(string.Format(ValidationConstant.INVALIDFORMAT, nameof(RegisterRequest.Email)));
            RuleFor(r => r.PhoneNumber)
                .NotEmpty().WithMessage(string.Format(ValidationConstant.NOTNULL, nameof(RegisterRequest.PhoneNumber)))
                .Matches(RegexConstant.EMAIL).WithMessage(string.Format(ValidationConstant.INVALIDFORMAT, nameof(RegisterRequest.PhoneNumber)));
            RuleFor(r => r.Password)
                .NotEmpty().WithMessage(string.Format(ValidationConstant.NOTNULL, nameof(RegisterRequest.Password)))
                .Matches(RegexConstant.PASSWORD).WithMessage(string.Format(ValidationConstant.INVALID, nameof(RegisterRequest.Password)));
        }
    }

    public class RegisterHandler : IRequestHandler<RegisterRequest>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMailService _mailService;
        private readonly IScheduler _scheduler;

        public RegisterHandler(UserManager<ApplicationUser> userManager, IMailService mailService, IScheduler scheduler)
        {
            _userManager = userManager;
            _mailService = mailService;
            _scheduler = scheduler;
        }

        public async Task Handle(RegisterRequest request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser(request.FirstName, request.LastName, request.Email, request.PhoneNumber);
            var res = await _userManager.CreateAsync(user, request.Password);

            if (!res.Succeeded) throw new AppException(nameof(RegisterRequest), HttpStatusCode.BadRequest, res.Errors.ToDictionary(v => v.Code, v => new string[] { v.Description }));

            // TODO throw to job
            // _mailService.SendMail([user.Email], "Register Success", EMailTempalte.Register, new { });
        }
    }
}

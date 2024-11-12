
using API.SSO.Infras.Shared.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.SSO.Settings
{
    public class AppExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public AppExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AppException ex)
            {
                var res = context.Response;

                res.StatusCode = (int)ex.StatusCode;
                await res.WriteAsJsonAsync(ex.Data);
            }
            catch (ValidationException ex) {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Type = "ValidationFailure",
                    Title = "Validation error",
                    Detail = "One or more validation errors has occurred"
                };

                if (ex.Errors is not null)
                {
                    problemDetails.Extensions["errors"] = ex.Errors;
                }

                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }
    }
}

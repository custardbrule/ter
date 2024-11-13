
using API.SSO.Infras.Shared.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mime;

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
                var problemDetails = new ProblemDetails
                {
                    Status = (int)ex.StatusCode,
                    Type = "BussinessException",
                    Title = "Bussiness error",
                    Detail = ex.Message,
                };
                if (ex.Errors is not null)
                {
                    problemDetails.Extensions["errors"] = ex.Errors;
                }

                context.Response.StatusCode = (int)ex.StatusCode;
                context.Response.ContentType = MediaTypeNames.Application.Json;
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
            catch (ValidationException ex)
            {
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
                context.Response.ContentType = MediaTypeNames.Application.Json;
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
            catch (Exception ex)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Type = "InternalServerError",
                    Title = "Something went wrong!",
                    Detail = ex.Message,
                };

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = MediaTypeNames.Application.Json;
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }
    }
}

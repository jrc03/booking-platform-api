using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        private static Task HandleExceptionAsync(HttpContext httpContext, HttpStatusCode statusCode, string message)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)statusCode;

            var result = JsonSerializer.Serialize(new { error = message });
            return httpContext.Response.WriteAsync(result);
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (FluentValidation.ValidationException ex)
            {
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;


                var errors = ex.Errors.Select(e => new { Field = e.PropertyName, Message = e.ErrorMessage });
                var result = JsonSerializer.Serialize(new { error = "Validation failed", details = errors });
                await httpContext.Response.WriteAsync(result);
            }
            catch (DbUpdateConcurrencyException ex) // EF Core concurrency exception
            {
                _logger.LogWarning(ex, "Concurrency conflict while updating the database.");
                await HandleExceptionAsync(httpContext, HttpStatusCode.Conflict, "The property is no longer available for the selected dates. Someone else just booked it.");
            }
            catch (ArgumentException ex) // E.g., "Price must be greater than zero"
            {
                await HandleExceptionAsync(httpContext, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (InvalidOperationException ex) // E.g., "Cannot book your own property"
            {
                await HandleExceptionAsync(httpContext, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (UnauthorizedAccessException ex) // E.g., User doesn't own the property or email not confirmed
            {
                await HandleExceptionAsync(httpContext, HttpStatusCode.Unauthorized, ex.Message);
            }
            catch (Exception ex) // Any other unhandled exception
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(httpContext, HttpStatusCode.InternalServerError, "An internal server error occurred.");
            }
        }
    }
}
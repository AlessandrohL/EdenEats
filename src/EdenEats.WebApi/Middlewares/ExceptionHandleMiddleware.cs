using EdenEats.Domain.Exceptions.BaseExceptions;
using EdenEats.WebApi.Common;
using EdenEats.WebApi.Helpers;

namespace EdenEats.WebApi.Middlewares
{
    public class ExceptionHandleMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandleMiddleware(
            RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var errors = GetErrorsFromException(ex);
                var statusCode = GetStatusCodeToException(ex);
                var title = HttpStatusHelper.GetTitleByStatusCode(statusCode);
                var errorResponse = new ErrorResponse(title, errors, statusCode);

                context.Response.StatusCode = statusCode;
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }

        private static Dictionary<string, IEnumerable<string>> GetErrorsFromException(Exception ex)
        {
            return ex switch
            {
                BadRequestException e => e.Errors,
                NotFoundException e => e.Errors,
                ValidationException e => e.Errors,
                UnauthorizedException e => e.Errors,
                UnprocessableException e => e.Errors,
                _ => new()
                {
                    { "Server", new string[1] { "Internal Server Error" } }
                }
            };
        }

        private static int GetStatusCodeToException(Exception ex)
        {
            return ex switch
            {
                BadRequestException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                ValidationException => StatusCodes.Status400BadRequest,
                UnauthorizedException => StatusCodes.Status401Unauthorized,
                UnprocessableException => StatusCodes.Status422UnprocessableEntity,
                _ => StatusCodes.Status500InternalServerError
            };
        }
    }
}
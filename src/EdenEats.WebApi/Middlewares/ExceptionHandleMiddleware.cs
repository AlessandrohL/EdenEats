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
                var errorResponse = CreateErrorResponseFromException(ex);

                context.Response.StatusCode = errorResponse.Status;
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }

        private static ErrorResponse CreateErrorResponseFromException(Exception ex)
        {
            return ex switch
            {
                BadRequestException e => ErrorResponse.BadRequest(e.TypeError, e.Message),
                NotFoundException e => ErrorResponse.NotFound(e.TypeError, e.Message),
                ValidationException e => ErrorResponse.BadRequest(e.TypeError, e.Message),
                UnauthorizedException e => ErrorResponse.Unauthorized(e.TypeError, e.Message),
                UnprocessableException e => ErrorResponse.UnprocessableEntity(e.TypeError, e.Message),
                ConflictException e => ErrorResponse.Conflict(e.TypeError, e.Message),
                _ => ErrorResponse.ServerError()
            };
        }
    }
}
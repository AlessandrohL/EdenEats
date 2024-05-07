using EdenEats.WebApi.Helpers;

namespace EdenEats.WebApi.Common
{
    public sealed class ErrorResponse : Response
    {
        public string Type { get; init; } = null!;
        public string Error { get; init; } = null!;

        public ErrorResponse(
            string title,
            string type,
            string error,
            int statusCode)
        {
            Status = statusCode;
            Title = title;
            Type = type;
            Error = error;
            IsSuccess = false;
        }

        public static ErrorResponse NotFound(string type, string error)
        {
            return new ErrorResponse(
                title: HttpStatusHelper.GetTitleByStatusCode(StatusCodes.Status404NotFound),
                type: type,
                error: error,
                statusCode: StatusCodes.Status404NotFound);
        }

        public static ErrorResponse BadRequest(string type, string error)
        {
            return new ErrorResponse(
                title: HttpStatusHelper.GetTitleByStatusCode(StatusCodes.Status400BadRequest),
                type: type,
                error: error,
                statusCode: StatusCodes.Status400BadRequest);
        }

        public static ErrorResponse Unauthorized(string type, string error)
        {
            return new ErrorResponse(
                title: HttpStatusHelper.GetTitleByStatusCode(StatusCodes.Status401Unauthorized),
                type: type,
                error: error,
                statusCode: StatusCodes.Status401Unauthorized);
        }

        public static ErrorResponse Forbidden(string type, string error)
        {
            return new ErrorResponse(
                title: HttpStatusHelper.GetTitleByStatusCode(StatusCodes.Status403Forbidden)
                type: type,
                error: error,
                statusCode: StatusCodes.Status403Forbidden
            );

        }

        public static ErrorResponse UnprocessableEntity(string type, string error)
        {
            return new ErrorResponse(
                title: HttpStatusHelper.GetTitleByStatusCode(StatusCodes.Status422UnprocessableEntity),
                type: type,
                error: error,
                statusCode: StatusCodes.Status422UnprocessableEntity);
        }

        public static ErrorResponse Conflict(string type, string error)
        {
            return new ErrorResponse(
                title: HttpStatusHelper.GetTitleByStatusCode(StatusCodes.Status409Conflict),
                type: type,
                error: error,
                statusCode: StatusCodes.Status409Conflict);
        }

        public static ErrorResponse ServerError(string type, string error)
        {
            return new ErrorResponse(
               title: HttpStatusHelper.GetTitleByStatusCode(StatusCodes.Status500InternalServerError),
               type: type,
               error: error,
               statusCode: StatusCodes.Status500InternalServerError);
        }

        public static ErrorResponse ServerError()
        {
            return new ErrorResponse(
               title: HttpStatusHelper.GetTitleByStatusCode(StatusCodes.Status500InternalServerError),
               type: "Server",
               error: "Internal Server Error",
               statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}

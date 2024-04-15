using Application.Common;
using EdenEats.WebApi.Helpers;

namespace EdenEats.WebApi.Common
{
    public class SuccessResponse<T> : Response
    {
        public T? Result { get; init; }

        private SuccessResponse()
        {
            Status = StatusCodes.Status200OK;
            Title = string.Empty;
            IsSuccess = true;
            Result = default;
        }

        public SuccessResponse(int statusCode, string title, T? result)
        {
            Status = statusCode;
            Title = title;
            IsSuccess = true;
            Result = result;
        }

        public static SuccessResponse<T> Created(T? result)
            => new SuccessResponse<T>(
                statusCode: StatusCodes.Status201Created,
                title: HttpStatusHelper.GetTitleByStatusCode(StatusCodes.Status201Created),
                result: result);

        public static SuccessResponse<T> Ok(T? result)
            => new SuccessResponse<T>(
                statusCode: StatusCodes.Status200OK,
                title: HttpStatusHelper.GetTitleByStatusCode(StatusCodes.Status200OK),
                result: result);

    }

    public class SuccessResponse : Response
    {
        private SuccessResponse()
        {
            Status = StatusCodes.Status200OK;
            Title = string.Empty;
            IsSuccess = true;
        }

        private SuccessResponse(int statusCode, string title)
        {
            Status = statusCode;
            Title = title;
            IsSuccess = true;
        }

        public static SuccessResponse Created()
            => new SuccessResponse(
                statusCode: StatusCodes.Status201Created,
                title: HttpStatusHelper.GetTitleByStatusCode(StatusCodes.Status201Created));

        public static SuccessResponse NoContent()
            => new SuccessResponse(
                statusCode: StatusCodes.Status204NoContent,
                title: HttpStatusHelper.GetTitleByStatusCode(StatusCodes.Status204NoContent));

    }
}

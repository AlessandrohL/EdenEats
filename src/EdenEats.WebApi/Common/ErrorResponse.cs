namespace EdenEats.WebApi.Common
{
    public sealed class ErrorResponse : Response
    {
        public Dictionary<string, IEnumerable<string>> Errors { get; init; }

        public ErrorResponse(
            string title,
            Dictionary<string, IEnumerable<string>> errors,
            int statusCode)
        {
            Status = statusCode;
            Title = title;
            IsSuccess = false;
            Errors = errors;
        }
    }
}

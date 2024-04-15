namespace EdenEats.WebApi.Common
{
    public abstract class Response
    {
        public int Status { get; set; }
        public string Title { get; set; } = null!;
        public bool IsSuccess { get; set; }
    }
}

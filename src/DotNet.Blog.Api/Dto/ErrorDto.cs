namespace DotNet.Blog.Api
{
    public class ErrorDto
    {
        public int Status { get; set; }

        public Dictionary<string, List<string>> Errors { get; } = new();
    }
}

namespace DotNet.Blog.Api
{
    public class ErrorDto
    {
        public string? Code { get; set; }

        public Dictionary<string, List<string>> Errors { get; } = new();
    }
}

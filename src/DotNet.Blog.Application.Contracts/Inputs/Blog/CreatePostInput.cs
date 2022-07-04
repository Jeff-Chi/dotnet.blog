namespace DotNet.Blog.Application.Contracts
{
    public class CreatePostInput
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; } = string.Empty;
        public bool IsPublished { get; set; }
        public string Path { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}

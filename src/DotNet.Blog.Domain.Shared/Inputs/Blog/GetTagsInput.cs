namespace DotNet.Blog.Domain.Shared
{
    public class GetTagsInput:PageInput
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }
    }
}

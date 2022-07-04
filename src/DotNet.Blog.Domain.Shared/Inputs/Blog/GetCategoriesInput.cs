namespace DotNet.Blog.Domain.Shared
{
    public class GetCategoriesInput : PageInput
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }
    }
}

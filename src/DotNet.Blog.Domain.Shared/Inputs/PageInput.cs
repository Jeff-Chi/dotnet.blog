using System.ComponentModel.DataAnnotations;

namespace DotNet.Blog.Domain.Shared
{
    public class PageInput
    {
        /// <summary>
        /// 分页，从1开始
        /// </summary>
        [Range(0,10)]
        public int Page { get; set; } = 1;

        /// <summary>
        /// 分页大小，默认10
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// 排序
        /// </summary>
        public string? Sorting { get; set; }
    }
}

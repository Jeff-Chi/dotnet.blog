using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Application.Contracts
{
    /// <summary>
    /// 分页dto
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedResultDto<T>
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 列表
        /// </summary>
        public IReadOnlyList<T> Items { get; set; } = new List<T>();
    }
}

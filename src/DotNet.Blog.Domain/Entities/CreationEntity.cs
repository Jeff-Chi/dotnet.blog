using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Domain
{
    /// <summary>
    /// modified
    /// </summary>
    public class CreationEntity<TKey> : Entity<TKey>, ICreator, ICreationTime
    {
        public CreationEntity(TKey id) : base(id)
        {
        }
        /// <summary>
        /// 创建人ID
        /// </summary>
        public Guid? CreatorId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; } = DateTime.Now;
    }
}

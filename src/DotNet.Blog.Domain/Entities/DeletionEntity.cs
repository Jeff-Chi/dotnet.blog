namespace DotNet.Blog.Domain
{
    /// <summary>
    /// modified
    /// </summary>
    public class DeletionEntity<TKey> : ModificationEntity<TKey>, IDeleter, IDeletionTime, ISoftDelete
    {
        public DeletionEntity(TKey id):base(id)
        {
        }

        /// <summary>
        /// 删除人Id
        /// </summary>
        public Guid? DeleterId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeletionTime { get; set; }

        /// <summary>
        /// 是否已删除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}

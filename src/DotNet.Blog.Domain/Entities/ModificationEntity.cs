namespace DotNet.Blog.Domain
{
    /// <summary>
    /// modified
    /// </summary>
    public class ModificationEntity<TKey> : CreationEntity<TKey>, IModifier, IModificationTime
    {
        public ModificationEntity(TKey id) : base(id)
        {
        }
        /// <summary>
        /// 修改人ID
        /// </summary>
        public Guid? ModifierId { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModificationTime { get; set; }
    }
}

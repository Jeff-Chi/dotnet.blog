namespace DotNet.Blog.Domain
{
    public interface ISoftDelete
    {
        /// <summary>
        /// 是否已删除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}

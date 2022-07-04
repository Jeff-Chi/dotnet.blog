namespace DotNet.Blog.Domain
{
    public interface IDeletionTime
    {
        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeletionTime { get; set; }
    }
}

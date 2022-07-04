namespace DotNet.Blog.Domain
{
    public interface IDeleter
    {
        /// <summary>
        /// 删除人Id
        /// </summary>
        public Guid? DeleterId { get; set; }
    }
}

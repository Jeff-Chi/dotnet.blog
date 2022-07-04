namespace DotNet.Blog.Domain
{
    public interface IModificationTime
    {
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModificationTime { get; set; }
    }
}

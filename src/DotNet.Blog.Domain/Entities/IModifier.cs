namespace DotNet.Blog.Domain
{
    public interface IModifier
    {
        /// <summary>
        /// 修改人Id
        /// </summary>
        public Guid? ModifierId { get; set; }
    }
}

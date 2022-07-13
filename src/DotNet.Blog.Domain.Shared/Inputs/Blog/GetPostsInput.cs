namespace DotNet.Blog.Domain.Shared
{
    public class GetPostsInput: PageInput
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 分类Id
        /// </summary>
        public Guid? CategoryId { get; set; }

        /// <summary>
        /// 是否发布
        /// </summary>
        public bool? IsPublished { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public Guid? UserId { get; set; }

        #region navigation properties

        public bool IncludeCategory { get; set; }

        public bool IncludeTag { get; set; }

        public bool IncludeUser { get; set; }
        #endregion

    }
}

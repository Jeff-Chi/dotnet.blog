namespace DotNet.Blog.Domain
{
    /// <summary>
    /// 文章
    /// </summary>
    public class Post : DeletionEntity<Guid>
    {
        public Post(Guid id) : base(id)
        {

        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; } = string.Empty;
        public bool IsPublished { get; set; }
        public string? Path { get; set; }
        public string Content { get; set; } = string.Empty;
        public Guid UserId { get; set; }

        #region navigation properties

        public User? User { get; set; }

        public List<PostCategory> PostCategories { get; set; } = new List<PostCategory>();
        public List<PostTag> PostTags { get; set; } = new List<PostTag>();
        #endregion
    }
}

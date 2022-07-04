namespace DotNet.Blog.Domain
{
    public class PostCategory
    {
        public Guid PostId { get; set; }
        public Guid CategoryId { get; set; }

        #region navigation properties

        public Post? Post { get; set; }
        public Category? Category { get; set; }

        #endregion
    }
}

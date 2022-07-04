namespace DotNet.Blog.Domain
{
    public class Category : DeletionEntity<Guid>
    {
        public Category(Guid id) : base(id)
        {

        }
        public string Name { get; set; } = string.Empty;
    }
}

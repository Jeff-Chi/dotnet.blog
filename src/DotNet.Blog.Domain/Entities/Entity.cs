namespace DotNet.Blog.Domain
{
    /// <summary>
    /// base entity
    /// </summary>
    /// <typeparam name="Tkey"></typeparam>
    public class Entity<Tkey>
    {
        public Entity(Tkey id)
        {
            Id = id;
        }

        /// <summary>
        /// Id
        /// </summary>
        public Tkey Id { get; set; }
    }
}

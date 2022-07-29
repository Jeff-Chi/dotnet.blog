namespace DotNet.Blog.Domain
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User : DeletionEntity<Guid>
    {
        public User(Guid id):base(id)
        {
        }
        public string UserName { get; set; } = string.Empty;
        public string? NickName { get; set; }
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsEnabled { get; set; }

        #region navigation properties

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        #endregion
    }
}

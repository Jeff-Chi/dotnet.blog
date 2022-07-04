namespace DotNet.Blog.Api.Dto
{
    public class CurrentUserDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string? NickName { get; set; }

        public string? Email { get; set; }

        public HashSet<string>? Permissions { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace DotNet.Blog.Api
{
    public class LoginDto
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        [Required]
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}

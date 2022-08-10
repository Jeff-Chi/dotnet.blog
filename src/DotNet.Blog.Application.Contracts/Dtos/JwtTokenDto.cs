namespace DotNet.Blog.Api
{
    public class JwtTokenDto
    {
        /// <summary>
        /// token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 有效时长
        /// </summary>
        public int Expires { get; set; }

        /// <summary>
        /// 刷新 token
        /// </summary>
        public string? RefreshToken { get; set; }

        public JwtTokenDto(string token, int expires, string? refreshToken)
        {
            Token = token;
            Expires = expires;
            RefreshToken = refreshToken;
        }
    }
}

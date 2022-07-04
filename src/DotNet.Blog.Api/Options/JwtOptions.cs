using Microsoft.IdentityModel.Tokens;

namespace DotNet.Blog.Api
{
    public class JwtOptions
    {
        public string SecurityKey { get; set; } = string.Empty;

        public string? Issuer { get; set; }

        public string? Audience { get; set; }

        //HmacSha256Signature
        public string Algorithms { get; set; } = SecurityAlgorithms.HmacSha256Signature; //SecurityAlgorithms.HmacSha256;

        public int Expires { get; set; } = 300;
    }
}

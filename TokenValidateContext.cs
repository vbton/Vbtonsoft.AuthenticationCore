using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Vbtonsoft.AuthenticationCore.Core;

namespace Vbtonsoft.AuthenticationCore
{
    public class TokenValidateContext : YepResultContext<YepAuthenticationSchemeOptions>
    {
        public TokenValidateContext(HttpContext context, AuthenticationScheme scheme, YepAuthenticationSchemeOptions options) : base(context, scheme, options)
        {
        }
    }
}

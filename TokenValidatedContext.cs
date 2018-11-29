using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Vbtonsoft.AuthenticationCore.Core;

namespace Vbtonsoft.AuthenticationCore
{
    public class TokenValidatedContext : YepResultContext<YepAuthenticationSchemeOptions>
    {
        public TokenValidatedContext(HttpContext context, AuthenticationScheme scheme, YepAuthenticationSchemeOptions options) : base(context, scheme, options)
        {

        }
    }
}

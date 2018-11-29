using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Vbtonsoft.AuthenticationCore.Core;

namespace Vbtonsoft.AuthenticationCore
{
    public class AuthenticationContext : YepResultContext<YepAuthenticationSchemeOptions>
    {
        public AuthenticationContext(HttpContext context, AuthenticationScheme scheme, YepAuthenticationSchemeOptions options) : base(context, scheme, options) { }
    }
}

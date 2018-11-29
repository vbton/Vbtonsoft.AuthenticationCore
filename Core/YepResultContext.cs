using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;

namespace Vbtonsoft.AuthenticationCore.Core
{
    public class YepResultContext<TOptions> : ResultContext<TOptions> where TOptions : AuthenticationSchemeOptions
    {
        public YepResultContext(HttpContext context, AuthenticationScheme scheme, TOptions options) : base(context, scheme, options)
        {
        }
        
        private ClaimsIdentity _InternalUser = null;
        internal ClaimsIdentity InternalUser
        {
            get
            {
                return _InternalUser ?? (_InternalUser = (ClaimsIdentity)User);
            }
            set
            {
                if (Principal == null)
                {
                    Principal = HttpContext.User = new ClaimsPrincipal();
                }
                Principal.AddIdentity(_InternalUser = value);
            }
        }
        /// <summary>
        /// 认证
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 调用方：协议、域名、和端口号部分；如:http://www.youepiao.com
        /// </summary>
        public string Origin => Request.Headers[HeaderNames.Origin];
        /// <summary>
        /// 调用方：地址；如:http://www.youepiao.com/auth?debug=true
        /// </summary>
        public string Url => Request.Headers[HeaderNames.Referer];

        /// <summary>
        /// 用户认证实体
        /// </summary>
        public IIdentity User
        {
            get
            {
                if (Principal == null)
                {
                    Principal = HttpContext.User ?? (HttpContext.User = new ClaimsPrincipal());
                }
                return Principal.Identity;
            }
        }
    }
}

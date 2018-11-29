using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;

namespace Vbtonsoft.AuthenticationCore.Core
{
    public class YepChallengeContext : PropertiesContext<YepAuthenticationSchemeOptions>
    {
        public YepChallengeContext(HttpContext context, AuthenticationScheme scheme, YepAuthenticationSchemeOptions options, AuthenticationProperties properties) : base(context, scheme, options, properties)
        {
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error
        {
            get;
            set;
        }

        /// <summary>
        /// 错误介绍
        /// </summary>
        public string ErrorDescription
        {
            get;
            set;
        }

        /// <summary>
        /// 错误地址
        /// </summary>
        public string ErrorUri
        {
            get;
            set;
        }

        /// <summary>
        /// 如果为真，将跳过任何相关验证
        /// </summary>
        public bool Handled
        {
            get;
            private set;
        }

        /// <summary>
        /// 事件请求
        /// </summary>
        public void HandleResponse()
        {
            Handled = true;
        }
    }
}

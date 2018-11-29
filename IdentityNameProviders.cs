using System;
using Vbtonsoft.AuthenticationCore.Security;

namespace Vbtonsoft.AuthenticationCore
{
    public static class IdentityNameProviders
    {
        /// <summary>
        /// 将字符串直接作为认证名称
        /// </summary>
        /// <returns></returns>
        public static IIdentityNameProvider FromString(string identityName)
        {
            return new DefaultIdentityNameProvider(identityName);
        }
        /// <summary>
        /// 将token的IdentityName作为认证信息。
        /// </summary>
        /// <returns></returns>
        public static IIdentityNameProvider FromToken(SecurityToken token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }
            return FromString(token.IdentityName);
        }
        /// <summary>
        /// 将字符串转为 Token 类型，再获取IdentityName作为认证信息。
        /// </summary>
        /// <returns></returns>
        public static IIdentityNameProvider FromTokenString<Token>(string token) where Token : SecurityToken
        {
            return FromToken(Newtonsoft.Json.JsonConvert.DeserializeObject<Token>(token));
        }
    }
}

using System;
using Vbtonsoft.AuthenticationCore.Security;

namespace Vbtonsoft.AuthenticationCore
{
    /// <summary>
    /// Token 认证
    /// </summary>
    public sealed class SecurityTokenValidate
    {
        private SecurityTokenValidate()
        {
        }

        private SecurityTokenValidate(IIdentityNameProvider userProvider)
        {
            UserProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
            TokenSuccess = true;
        }

        public bool TokenSuccess { private set; get; }

        public IIdentityNameProvider UserProvider { private set; get; }

        public static SecurityTokenValidate Fail()
        {
            return new SecurityTokenValidate();
        }
        public static SecurityTokenValidate Success(IIdentityNameProvider userProvider)
        {
            return new SecurityTokenValidate(userProvider);
        }
        public static SecurityTokenValidate Success(SecurityToken token)
        {
            return Success(IdentityNameProviders.FromToken(token));
        }
        public static SecurityTokenValidate Success<Token>(string token) where Token : SecurityToken
        {
            return Success(IdentityNameProviders.FromTokenString<Token>(token));
        }
        public static SecurityTokenValidate Success(string identityName)
        {
            return Success(IdentityNameProviders.FromString(identityName));
        }
    }
}

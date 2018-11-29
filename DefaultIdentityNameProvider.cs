using System;

namespace Vbtonsoft.AuthenticationCore
{
    /// <summary>
    /// 用户名称实体
    /// </summary>
    public sealed class DefaultIdentityNameProvider : IIdentityNameProvider
    {
        public DefaultIdentityNameProvider(string identityName)
        {
            if (string.IsNullOrEmpty(identityName))
            {
                throw new ArgumentException("identityName cannot be null or empty.", nameof(identityName));
            }
            IdentityName = identityName;
        }
        public string IdentityName { private set; get; }
    }
}

using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Vbtonsoft.AuthenticationCore.Core
{
    public class YepAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public YepAuthenticationSchemeOptions() : base() { }

        private readonly List<Claim> _Claims = new List<Claim>();

        /// <summary>
        /// 获取或设置用于"WWW-Authenticate"头的消息.
        /// </summary>
        public string Challenge
        {
            get;
            set;
        }

        /// <summary>
        /// 用户认证
        /// </summary>
        public IEnumerable<Claim> Claims { get { return _Claims; } }

        public new YepAuthenticationEvents Events
        {
            get
            {
                return (YepAuthenticationEvents)base.Events;
            }
            set
            {
                base.Events = value;
            }
        }
        /// <summary>
        /// 添加用户认证
        /// </summary>
        /// <param name="claims"></param>
        public void AddClaim(params Claim[] claims)
        {
            if (claims == null) throw new ArgumentException("claims");
            if (claims.Length > 0)
            {
                _Claims.AddRange(claims);
            }
        }
        /// <summary>
        /// 添加用户角色认证(多个用逗号分隔)
        /// </summary>
        /// <param name="roles"></param>
        public void AddClaim(string roles)
        {
            if (string.IsNullOrEmpty(roles)) return;
            foreach (string role in roles.Split(','))
            {
                _Claims.Add(new Claim(ClaimTypes.Role, role));
            }
        }
    }
}

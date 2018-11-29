namespace Vbtonsoft.AuthenticationCore.Security
{
    /// <summary>
    /// token 认证
    /// </summary>
    public class SecurityToken
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }
        /// <summary> 认证实体名称 </summary>
        public virtual string IdentityName => this.Name;
    }
}

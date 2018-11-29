namespace Vbtonsoft.AuthenticationCore
{
    /// <summary>
    /// 获取用户名称
    /// </summary>
    public interface IIdentityNameProvider
    {
        string IdentityName { get; }
    }
}

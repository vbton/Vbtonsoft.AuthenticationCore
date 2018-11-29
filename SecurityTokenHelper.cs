using Vbtonsoft.AuthenticationCore.Security;

namespace Vbtonsoft.AuthenticationCore
{
    public class SecurityTokenHelper
    {
        public virtual SecurityToken GetToken(string token)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<SecurityToken>(token);
        }
    }
}

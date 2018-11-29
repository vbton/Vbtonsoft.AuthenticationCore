using System;
using System.Threading.Tasks;
using Vbtonsoft.AuthenticationCore.Core;

namespace Vbtonsoft.AuthenticationCore
{
    public class YepAuthenticationEvents
    {
        /// <summary>
        /// 接收到请求连接的消息
        /// </summary>
        public Action<MessageReceivedContext> OnMessageReceived { get; set; }
        /// <summary>
        /// 消息验证
        /// </summary>
        public Func<TokenValidateContext, SecurityTokenValidate> OnTokenValidate { get; set; }
        /// <summary>
        /// 验证成功
        /// </summary>
        public Func<TokenValidatedContext, Task> OnTokenValidated { get; set; }

        /// <summary>
        /// 在challenge之前执行
        /// </summary>
        public Func<YepChallengeContext, Task> OnChallenge { get; set; }

        public virtual Task MessageReceived(MessageReceivedContext context)
        {
            return Task.Run(() =>
           {
               OnMessageReceived?.Invoke(context);
           });
        }
        public virtual Task<SecurityTokenValidate> TokenValidate(TokenValidateContext context)
        {
            return Task.Run(() =>
           {
               return OnTokenValidate?.Invoke(context) ?? SecurityTokenValidate.Fail();
           });
        }

        public virtual Task TokenValidated(TokenValidatedContext context)
        {
            return OnTokenValidated?.Invoke(context);
        }

        public virtual Task Challenge(YepChallengeContext context)
        {
            return OnChallenge?.Invoke(context);
        }
    }
}

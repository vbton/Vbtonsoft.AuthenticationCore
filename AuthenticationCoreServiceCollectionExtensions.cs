using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;
using Vbtonsoft.AuthenticationCore.Core;
using Vbtonsoft.AuthenticationCore.Core.Infrastructure;

namespace Vbtonsoft.AuthenticationCore
{
    public static class AuthenticationCoreServiceCollectionExtensions
    {
        /// <summary>
        /// 中间件
        /// </summary>
        /// <param name="configure">token认证</param>
        /// <returns></returns>
        public static IServiceCollection AddAuthenticationCore(this IServiceCollection services, string policyName, Action<YepAuthenticationSchemeOptions> configure)
        {
            return services.AddAuthenticationCore(policyName, null, configure);
        }
        /// <summary>
        /// 中间件(复杂设计：拦截每个控制器的每个方法)
        /// </summary>
        /// <param name="authorizationReq">对每个控制器和方法进行拦截</param>
        /// <param name="configure">token认证</param>
        /// <returns></returns>
        public static IServiceCollection AddAuthenticationCore(this IServiceCollection services, string policyName, Action<YepAuthorizationRequirementContext> authorizationReq, Action<YepAuthenticationSchemeOptions> configure)
        {
            if (string.IsNullOrEmpty(policyName))
            {
                throw new ArgumentException("message", nameof(policyName));
            }

            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            services.AddAuthorization(options =>
            {
                options.AddPolicy(policyName, policy =>
                {
                    policy.RequireClaim(ClaimTypes.Name);
                    policy.AddAuthenticationSchemes(policyName);
                    if (authorizationReq is null) return;
                    policy.Requirements.Add(new YepAuthorizationRequirement(authorizationReq));
                });
            })
            .AddAuthentication(policyName)
            .AddScheme<YepAuthenticationSchemeOptions, YepAuthenticationHandler>(policyName, null, options =>
            {
                configure(options);
            });
            return services;
        }
    }
}

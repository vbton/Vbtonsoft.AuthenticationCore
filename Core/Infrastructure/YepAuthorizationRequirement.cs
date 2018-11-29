using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace Vbtonsoft.AuthenticationCore.Core.Infrastructure
{
    public class YepAuthorizationRequirement : AuthorizationHandler<YepAuthorizationRequirement, AuthorizationFilterContext>, IAuthorizationRequirement
    {
        private readonly Action<YepAuthorizationRequirementContext> AuthorizationReq = null;
        public YepAuthorizationRequirement(Action<YepAuthorizationRequirementContext> authorizationReq)
        {
            AuthorizationReq = authorizationReq ?? throw new ArgumentNullException("authorization");
        }
        public override async Task HandleAsync(AuthorizationHandlerContext context)
        {
            if (!context.User.Identity.IsAuthenticated) return;
            await base.HandleAsync(context);
            if (!context.HasSucceeded) return;
            var req_context = new YepAuthorizationRequirementContext(context, (AuthorizationFilterContext)context.Resource);
            AuthorizationReq(req_context);
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, YepAuthorizationRequirement requirement, AuthorizationFilterContext resource)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}

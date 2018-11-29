using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Security.Claims;

namespace Vbtonsoft.AuthenticationCore
{
    public class YepAuthorizationRequirementContext
    {
        public YepAuthorizationRequirementContext(AuthorizationHandlerContext context, AuthorizationFilterContext resource)
        {
            this.Context = context ?? throw new System.ArgumentNullException(nameof(context));
            this.Resource = resource ?? throw new System.ArgumentNullException(nameof(resource));
        }
        //
        // 摘要:
        //     Flag indicating whether the current authorization processing has succeeded.
        public bool Success { get { return Context.HasSucceeded; } }

        public AuthorizationHandlerContext Context { private set; get; }
        public AuthorizationFilterContext Resource { private set; get; }

        public IDictionary<string, object> RouteValues
        {
            get
            {
                return Resource.RouteData.Values;
            }
        }
        /// <summary>
        /// 当前访问的控制器名称
        /// </summary>
        public string ControllerName
        {
            get
            {
                return RouteValues.TryGetValue("controller", out object value) ? value.ToString() : string.Empty;
            }
        }

        /// <summary>
        /// 当前访问的方法名称
        /// </summary>
        public string ActionName
        {
            get
            {
                return RouteValues.TryGetValue("action", out object value) ? value.ToString() : string.Empty;
            }
        }
        /// <summary>
        /// 当前认证用户名称
        /// </summary>
        public string Name => User.Identity.Name;

        /// <summary>
        /// 请求信息
        /// </summary>
        public HttpRequest Request => Resource.HttpContext.Request;
        /// <summary>
        /// 请求方式
        /// </summary>
        public string Method => Request.Method;

        /// <summary>
        /// 请求地址
        /// </summary>
        public string Path => Request.Path.Value;

        /// <summary>
        /// 当前用户认证实体
        /// </summary>
        public ClaimsPrincipal User { get { return Context.User; } }
        //
        // 摘要:
        //     Called to indicate Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext.HasSucceeded
        //     will never return true, even if all requirements are met.
        public void Fail()
        {
            Context.Fail();
        }
    }
}

# AspNetCore Mvc 自定义中间件认证
> #### 实现控制器访问验证和拦截.

1.注册政策。
--
> 
>> 例如：
>> ``` C# 
>> services.AddAuthorization(options =>
>> {
>>     options.AddPolicy(policyName /* 1.1 政策名称 */ , policy =>
>>     {
>>         policy.RequireClaim(ClaimTypes.Name /* 1.2 政策要求 */);
>>         policy.AddAuthenticationSchemes(policyName /* 1.3 政策计划 */);
>>         policy.Requirements.Add(new YepAuthorizationRequirement(authorizationReq) /* 1.4 政策需求 */);
>>     });
>> })
>> ```
>
> 说明：
> * 1.1 定义政策名称，政策名称用于控制器或者控制器方法的【` AuthorizeAttribute `】属性。
> * 1.2 政策要求，建议使用 ` ClaimTypes.Name ` 类型，其他类型部分测试后程序通不过，原因还在努力理解源码。
> * 1.3 政策计划，不必须。作用：请阅读政策计划。
> * 1.4 政策需求，不必须。作用：请阅读政策需求。

2.注册计划
--
> 例如：
>> ``` C# 
>> services.AddAuthentication(policyName /* 2.1 计划名称 */)
>> .AddScheme<YepAuthenticationSchemeOptions /* 2.2 计划处理器配置项 */, YepAuthenticationHandler /* 2.3 计划处理器 */>(policyName /* 2.4 计划认证名称 */, null /* 2.5 展示名称 */, options => /* ⑦ 配置项自定义 */
>> {
>> 	   configure(options);
>> });
>> ```
>
> 说明：
> * 2.1 计划名称，与 _1.3_ 政策计划强关联，定义了该计划的控制器或方法，将按照计划执行验证。
> * 2.2 计划处理器配置项（必须继承`AuthenticationSchemeOptions`），为 _2.3_ (计划处理器) 提供计划执行中需要的参数或属性方法。
> * 2.3 计划处理器(必须继承`AuthenticationHandler<T>`并实现其抽象方法，其中泛型`T`是 _2.2_ 中配置项的类)，根据 _2.2_ 提供的信息，进行接口访问信息验证。
> * 2.4 定义当前计划处理器中的计划名称。
> * 2.5 定义当前计划处理器中的计划显示名称。
> * 2.6 配置计划处理器配置项的项。

3.政策需求
--
> 例如：
>> ``` C# 
>> public class YepAuthorizationRequirement : AuthorizationHandler<YepAuthorizationRequirement/* 3.1 政策需求 */>, /* 3.2 政策需求基类 */ IAuthorizationRequirement /* 3.3 政策需求接口 */
>> {
>> 	public YepAuthorizationRequirement()
>> 	{
>> 	}
>> 	public override async Task HandleAsync(AuthorizationHandlerContext context)
>> 	{
>> 	   await base.HandleAsync(context);
>>     /* 3.4 政策需求检测结果 */
>> 	}
>> 	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, YepAuthorizationRequirement requirement)
>> 	{
>> 	  if (context.User.Identity.IsAuthenticated/* 3.5 检测是否满足政策需求 */)
>> 	  {
>> 	    context.Succeed(requirement); /* 3.6 设置当前政策需求为认证通过 */
>> 	  }
>> 	  return Task.CompletedTask;
>> 	}
>> }
>> ``` 
>
> 说明：
> * 3.1 政策需求，当前定义的政策需求类。
> * 3.2 政策需求基类（可继承 `AuthorizationHandler<TRequirement>` 或 `AuthorizationHandler<YepAuthorizationRequirement, AuthorizationFilterContext>`类，并实现抽象方法），验证入口（核心：`IAuthorizationHandler`）。
> * 3.3 政策需求接口，必须（框架要求）。
> * 3.4 政策需求检测结果（核心：`context.HasSucceeded`，为 true 时，表示***整个需求验证通过***）。
> * 3.5 检测是否满足政策需求，对当前接口调用方进行详细的验证（如：控制器名称、方法名称等）。
> * 3.6 设置当前政策需求为认证通过。调用后表示当前用户满足请求要求，否则***整个需求验证失败***


> 参考代码：[Vbtonsoft.AuthenticationCore](https://github.com/vbton/Vbtonsoft.AuthenticationCore)

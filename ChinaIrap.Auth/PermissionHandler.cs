/*----------------------------------------------------------------
// Copyright © 2019 Chinairap.All rights reserved. 
// CLR版本：	4.0.30319.42000
// 类 名 称：    PermissionHandler
// 文 件 名：    PermissionHandler
// 创建者：      DUWENINK
// 创建日期：	2019/7/22 10:23:43
// 版本	日期					修改人	
// v0.1	2019/7/22 10:23:43	DUWENINK
//----------------------------------------------------------------*/
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChinaIrap.Auth
{
    /// <summary>
    /// 命名空间： ChinaIrap.Auth
    /// 创建者：   DUWENINK
    /// 创建日期： 2019/7/22 10:23:43
    /// 类名：     PermissionHandler(权限处理逻辑)
    /// </summary>
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        /// <summary>
        /// 验证方案提供对象
        /// </summary>
        public IAuthenticationSchemeProvider Schemes { get; set; }
        /// <summary>
        /// 用户权限集合
        /// </summary>
        List<Permission> _permissions;
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="schemes"></param>
        public PermissionHandler(IAuthenticationSchemeProvider schemes, List<Permission> permissions = null)
        {
            Schemes = schemes;
            _permissions = permissions;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            //从AuthorizationHandlerContext转成HttpContext，以便取出表单信息
            var httpContext = (context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext).HttpContext;
            //请求Url
            var questUrl = httpContext.Request.Path.Value.ToLower();
            Console.WriteLine(questUrl);
            //判断请求是否停止
            var handlers = httpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
            foreach (var scheme in await Schemes.GetRequestHandlerSchemesAsync())
            {
                var handler = await handlers.GetHandlerAsync(httpContext, scheme.Name) as IAuthenticationRequestHandler;
                if (handler != null && await handler.HandleRequestAsync())
                {
                    context.Fail();
                    return;
                }
            }
            //判断请求是否拥有凭据，即有没有登录
            var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
            if (defaultAuthenticate != null)
            {
                var result = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
                //result?.Principal不为空即登录成功
                if (result?.Principal != null)
                {
                    httpContext.User = result.Principal;
                    //权限中是否存在请求的url
                    if (_permissions != null && _permissions.GroupBy(g => g.Url).Where(w => w.Key.ToLower() == questUrl).Count() > 0)
                    {
                        var name = httpContext.User.Claims.SingleOrDefault(s => s.Type == requirement.ClaimType).Value;
                        //验证权限
                        if (_permissions.Where(w => w.Name == name && w.Url.ToLower() == questUrl).Count() == 0)
                        {
                            //无权限跳转到拒绝页面   
                            httpContext.Response.Redirect(requirement.DeniedAction);
                            context.Succeed(requirement);
                            return;
                        }
                    }
                    //判断过期时间
                    if (DateTime.Parse(httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration).Value) >= DateTime.Now)
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        context.Fail();
                    }
                    return;
                }
            }
            //判断没有登录时，是否访问登录的url,并且是Post请求，并且是form表单提交类型，否则为失败
            if (!questUrl.Equals(requirement.LoginPath.ToLower(), StringComparison.Ordinal) && (!httpContext.Request.Method.Equals("POST")
               || !httpContext.Request.HasFormContentType))
            {
                context.Fail();
                return;
            }
            context.Succeed(requirement);
        }
    }
}

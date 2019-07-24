/*----------------------------------------------------------------
// Copyright © 2019 Chinairap.All rights reserved. 
// CLR版本：	4.0.30319.42000
// 类 名 称：    ExceptionLessExtension
// 文 件 名：    ExceptionLessExtension
// 创建者：      DUWENINK
// 创建日期：	2019/7/24 15:56:00
// 版本	日期					修改人	
// v0.1	2019/7/24 15:56:00	DUWENINK
//----------------------------------------------------------------*/
using ChinaIrap.Extentions;
using ChinaIrap.Extentions.Interfaces;
using ChinaIrap.Extentions.Services;
using Exceptionless;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaIrap.Extentions
{
    /// <summary>
    /// 命名空间： ChinaIrap.Extentions
    /// 创建者：   DUWENINK
    /// 创建日期： 2019/7/24 15:56:00
    /// 类名：     ExceptionLess
    /// </summary>
    public static class ExceptionLessExtension
    {


        /// <summary>
        /// 添加ChinaIrapExceptionLess
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddChinaIrapExceptionLess(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ExceptionOptions>(configuration.GetSection("ChinaIrapExceptionLess"));
            var provider = services.BuildServiceProvider().GetRequiredService<IOptions<ExceptionOptions>>().Value;
            //注入ExceptionlessLogger服务到容器,单例模式
            services.AddSingleton<ILoggerHelper, ExceptionlessLogger>();
            return services;
        }


        /// <summary>
        /// 使用ExceptionLess
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseChinaIrapExceptionLess(this IApplicationBuilder app)
        {
            IOptions<ExceptionOptions> serviceOptions = app.ApplicationServices.GetRequiredService<IOptions<ExceptionOptions>>();
           

            if (!serviceOptions.Value.IsActive) { return app; }
            ExceptionlessClient.Default.Configuration.ApiKey = serviceOptions.Value.ApiKey;
            if (serviceOptions.Value.ServerUrl.IsNotBlank())
            {
                ExceptionlessClient.Default.Configuration.ServerUrl = serviceOptions.Value.ServerUrl;
            }
            ExceptionlessClient.Default.SubmittingEvent += OnSubmittingEvent;
            app.UseExceptionless(serviceOptions.Value.ApiKey);
            return app;
        }

        /// <summary>
        /// 全局配置Exceptionless
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnSubmittingEvent(object sender, EventSubmittingEventArgs e)
        {
            // 只处理未处理的异常
            if (!e.IsUnhandledError)
                return;

            // 忽略404错误
            if (e.Event.IsNotFound())
            {
                e.Cancel = true;
                return;
            }

            // 忽略没有错误体的错误
            var error = e.Event.GetError();
            if (error == null)
                return;
            // 忽略 401 (Unauthorized) 和 请求验证的错误.
            if (error.Code == "401" || error.Type == "System.Web.HttpRequestValidationException")
            {
                e.Cancel = true;
                return;
            }
            // Ignore any exceptions that were not thrown by our code.
            var handledNamespaces = new List<string> { "Exceptionless" };
            if (!error.StackTrace.Select(s => s.DeclaringNamespace).Distinct().Any(ns => handledNamespaces.Any(ns.Contains)))
            {
                e.Cancel = true;
                return;
            }
            // 添加附加信息.
            //e.Event.AddObject(order, "Order", excludedPropertyNames: new[] { "CreditCardNumber" }, maxDepth: 2);
            e.Event.Tags.Add("MunicipalPublicCenter.BusinessApi");
            e.Event.MarkAsCritical();
            //e.Event.SetUserIdentity();

        }
    }
}

/*----------------------------------------------------------------
// Copyright © 2019 Chinairap.All rights reserved. 
// CLR版本：	4.0.30319.42000
// 类 名 称：    SkyWalkingExtension
// 文 件 名：    SkyWalkingExtension
// 创建者：      DUWENINK
// 创建日期：	2019/7/23 15:41:06
// 版本	日期					修改人	
// v0.1	2019/7/23 15:41:06	DUWENINK
//----------------------------------------------------------------*/
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChinaIrap.Extentions.SkyWalking
{
    /// <summary>
    /// 命名空间： ChinaIrap.Extentions.SkyWalking
    /// 创建者：   DUWENINK
    /// 创建日期： 2019/7/23 15:41:06
    /// 类名：     SkyWalkingExtension
    /// </summary>
    public static class SkyWalkingExtension
    {

        /// <summary>
        /// 添加服务跟踪功能
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddChinaIrapServiceTracking(this IServiceCollection services, IConfiguration configuration)
        {
            //配置跟踪地址
            services.Configure<ServiceDiscoveryOptions>(configuration.GetSection("ServiceTracking"));

          
            return services;
        }


    }
}

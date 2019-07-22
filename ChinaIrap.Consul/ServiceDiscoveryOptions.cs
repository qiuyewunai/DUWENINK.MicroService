/*----------------------------------------------------------------
// Copyright © 2019 Chinairap.All rights reserved. 
// CLR版本：	4.0.30319.42000
// 类 名 称：    ServiceDiscoveryOptions
// 文 件 名：    ServiceDiscoveryOptions
// 创建者：      DUWENINK
// 创建日期：	2019/7/22 11:54:53
// 版本	日期					修改人	
// v0.1	2019/7/22 11:54:53	DUWENINK
//----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;

namespace ChinaIrap.Consul
{
    /// <summary>
    /// 命名空间： ChinaIrap.Consul
    /// 创建者：   DUWENINK
    /// 创建日期： 2019/7/22 11:54:53
    /// 类名：     ServiceDiscoveryOptions(服务治理第三方组件Consul相关配置参数)
    /// </summary>
    public class ServiceDiscoveryOptions
    {
        public string ServiceName { get; set; }

        public ConsulOptions Consul { get; set; }
    }

    public class ConsulOptions
    {
        public string HttpEndPoint { get; set; }
    }

}

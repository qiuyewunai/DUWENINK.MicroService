/*----------------------------------------------------------------
// Copyright © 2019 Chinairap.All rights reserved. 
// CLR版本：	4.0.30319.42000
// 类 名 称：    Permission
// 文 件 名：    Permission
// 创建者：      DUWENINK
// 创建日期：	2019/7/22 10:22:19
// 版本	日期					修改人	
// v0.1	2019/7/22 10:22:19	DUWENINK
//----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;

namespace ChinaIrap.Auth
{
    /// <summary>
    /// 命名空间： ChinaIrap.Auth
    /// 创建者：   DUWENINK
    /// 创建日期： 2019/7/22 10:22:19
    /// 类名：     Permission(用户或角色或其他的凭据实体)
    /// </summary>
    public class Permission
    {
        /// <summary>
        /// 用户或角色或其他凭据名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 请求Url
        /// </summary>
        public virtual string Url { get; set; }
    }
}

/*----------------------------------------------------------------
// Copyright © 2019 Chinairap.All rights reserved. 
// CLR版本：	4.0.30319.42000
// 类 名 称：    ExceptionOptions
// 文 件 名：    ExceptionOptions
// 创建者：      DUWENINK
// 创建日期：	2019/7/24 15:48:23
// 版本	日期					修改人	
// v0.1	2019/7/24 15:48:23	DUWENINK
//----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;

namespace ChinaIrap.Extentions
{
    /// <summary>
    /// 命名空间： ChinaIrap.Extentions.ExceptionLess
    /// 创建者：   DUWENINK
    /// 创建日期： 2019/7/24 15:48:23
    /// 类名：     ExceptionOptions
    /// </summary>
    public class ExceptionOptions
    {
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// apikey
        /// </summary>
        public string ApiKey { get; set; }
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string ServerUrl { get; set; }
    }
}

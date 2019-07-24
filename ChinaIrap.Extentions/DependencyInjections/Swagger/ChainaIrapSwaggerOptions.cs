/*----------------------------------------------------------------
// Copyright © 2019 Chinairap.All rights reserved. 
// CLR版本：	4.0.30319.42000
// 类 名 称：    ChainaIrapSwaggerOptions
// 文 件 名：    ChainaIrapSwaggerOptions
// 创建者：      DUWENINK
// 创建日期：	2019/7/22 17:18:44
// 版本	日期					修改人	
// v0.1	2019/7/22 17:18:44	DUWENINK
//----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;

namespace ChinaIrap.Extentions
{
    /// <summary>
    /// 命名空间： ChinaIrap.Extentions.Swagger
    /// 创建者：   DUWENINK
    /// 创建日期： 2019/7/22 17:18:44
    /// 类名：     ChainaIrapSwaggerOptions
    /// </summary>
    public class ChainaIrapSwaggerOptions
    {
        /// <summary>
        /// 文档的名字
        /// </summary>
        public string DocName { get; set; }


        /// <summary>
        /// 标题信息
        /// </summary>
        public string TitleInfo { get; set; }

        /// <summary>
        /// 版本信息
        /// </summary>
        public string VersionInfo { get; set; }
        /// <summary>
        /// Xml文档路径
        /// </summary>
        public string XmlName { get ; set ; }
    }
}

/*----------------------------------------------------------------
// Copyright © 2019 Chinairap.All rights reserved. 
// CLR版本：	4.0.30319.42000
// 类 名 称：    ILoggerHelper
// 文 件 名：    ILoggerHelper
// 创建者：      DUWENINK
// 创建日期：	2019/7/24 16:19:29
// 版本	日期					修改人	
// v0.1	2019/7/24 16:19:29	DUWENINK
//----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;

namespace ChinaIrap.Extentions.Interfaces
{
    /// <summary>
    /// 命名空间： ChinaIrap.Extentions.Interfaces
    /// 创建者：   DUWENINK
    /// 创建日期： 2019/7/24 16:19:29
    /// 类名：     ILoggerHelper
    /// </summary>
    public interface ILoggerHelper
    {
        /// <summary>
        /// 记录trace日志
        /// </summary>
        /// <param name="source">信息来源</param>
        /// <param name="message">日志内容</param>
        /// <param name="args">标记</param>
        void Trace(string source, string message, params string[] args);
        /// <summary>
        /// 记录debug信息
        /// </summary>
        /// <param name="source">信息来源</param>
        /// <param name="message">日志内容</param>
        /// <param name="args">标记</param>
        void Debug(string source, string message, params string[] args);
        /// <summary>
        /// 记录信息
        /// </summary>
        /// <param name="source">信息来源</param>
        /// <param name="message">日志内容</param>
        /// <param name="args">标记</param>
        void Info(string source, string message, params string[] args);
        /// <summary>
        /// 记录警告日志
        /// </summary>
        /// <param name="source">信息来源</param>
        /// <param name="message">日志内容</param>
        /// <param name="args">标记</param>
        void Warn(string source, string message, params string[] args);
        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="source">信息来源</param>
        /// <param name="message">日志内容</param>
        /// <param name="args">标记</param>
        void Error(string source, string message, params string[] args);
    }
   

}

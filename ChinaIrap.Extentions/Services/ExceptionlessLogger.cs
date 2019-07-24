/*----------------------------------------------------------------
// Copyright © 2019 Chinairap.All rights reserved. 
// CLR版本：	4.0.30319.42000
// 类 名 称：    ExceptionlessLogger
// 文 件 名：    ExceptionlessLogger
// 创建者：      DUWENINK
// 创建日期：	2019/7/24 16:20:56
// 版本	日期					修改人	
// v0.1	2019/7/24 16:20:56	DUWENINK
//----------------------------------------------------------------*/
using ChinaIrap.Extentions.Interfaces;
using Exceptionless;
using Exceptionless.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChinaIrap.Extentions.Services
{
    /// <summary>
    /// 命名空间： ChinaIrap.Extentions.Services
    /// 创建者：   DUWENINK
    /// 创建日期： 2019/7/24 16:20:56
    /// 类名：     ExceptionlessLogger
    /// </summary>
    public class ExceptionlessLogger : ILoggerHelper
    { /// <summary>
      /// 记录trace日志
      /// </summary>
      /// <param name="source">信息来源</param>
      /// <param name="message">日志内容</param>
      /// <param name="args">添加标记</param>
        public void Trace(string source, string message, params string[] args)
        {
            if (args != null && args.Length > 0)
            {
                ExceptionlessClient.Default.CreateLog(source, message, LogLevel.Trace).AddTags(args).Submit();

            }
            else
            {
                ExceptionlessClient.Default.SubmitLog(source, message, LogLevel.Trace);
            }
        }
        /// <summary>
        /// 记录debug信息
        /// </summary>
        /// <param name="source">信息来源</param>
        /// <param name="message">日志内容</param>
        /// <param name="args">标记</param>
        public void Debug(string source, string message, params string[] args)
        {
            if (args != null && args.Length > 0)
            {
                ExceptionlessClient.Default.CreateLog(source, message, LogLevel.Debug).AddTags(args).Submit();
            }
            else
            {
                ExceptionlessClient.Default.SubmitLog(source, message, LogLevel.Debug);
            }
        }
        /// <summary>
        /// 记录信息
        /// </summary>
        /// <param name="source">信息来源</param>
        /// <param name="message">日志内容</param>
        /// <param name="args">标记</param>
        public void Info(string source, string message, params string[] args)
        {
            if (args != null && args.Length > 0)
            {
                ExceptionlessClient.Default.CreateLog(source, message, LogLevel.Info).AddTags(args).Submit();
            }
            else
            {
                ExceptionlessClient.Default.SubmitLog(source, message, LogLevel.Info);
            }
        }
        /// <summary>
        /// 记录警告日志
        /// </summary>
        /// <param name="source">信息来源</param>
        /// <param name="message">日志内容</param>
        /// <param name="args">标记</param>
        public void Warn(string source, string message, params string[] args)
        {
            if (args != null && args.Length > 0)
            {
                ExceptionlessClient.Default.CreateLog(source, message, LogLevel.Warn).AddTags(args).Submit();
            }
            else
            {
                ExceptionlessClient.Default.SubmitLog(source, message, LogLevel.Warn);
            }
        }
        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="source">信息来源</param>
        /// <param name="message">日志内容</param>
        /// <param name="args">标记</param>
        public void Error(string source, string message, params string[] args)
        {
            if (args != null && args.Length > 0)
            {
                ExceptionlessClient.Default.CreateLog(source, message, LogLevel.Error).AddTags(args).Submit();
            }
            else
            {
                ExceptionlessClient.Default.SubmitLog(source, message, LogLevel.Error);
            }
        }

    }
}

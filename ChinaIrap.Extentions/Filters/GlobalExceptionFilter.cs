/*----------------------------------------------------------------
// Copyright © 2019 Chinairap.All rights reserved. 
// CLR版本：	4.0.30319.42000
// 类 名 称：    GlobalExceptionFilter
// 文 件 名：    GlobalExceptionFilter
// 创建者：      DUWENINK
// 创建日期：	2019/7/24 16:32:49
// 版本	日期					修改人	
// v0.1	2019/7/24 16:32:49	DUWENINK
//----------------------------------------------------------------*/
using ChinaIrap.Extentions.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ChinaIrap.Extentions.Filters
{
    /// <summary>
    /// 命名空间： ChinaIrap.Extentions.Filters
    /// 创建者：   DUWENINK
    /// 创建日期： 2019/7/24 16:32:49
    /// 类名：     GlobalExceptionFilter(全局异常捕获)
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILoggerHelper _loggerHelper;
        //构造函数注入ILoggerHelper
        public GlobalExceptionFilter(ILoggerHelper loggerHelper)
        {
            _loggerHelper = loggerHelper;
        }

        public void OnException(ExceptionContext filterContext)
        {
            _loggerHelper.Error(filterContext.Exception.TargetSite.GetType().FullName, filterContext.Exception.ToString(),  filterContext.Exception.GetType().FullName);
            var result = new NormalResult()
            {
               Successful=false,
               Code=-1,
               Message=$"系统异常:{filterContext?.Exception?.Message??"错误类型未知"}"
            };
            filterContext.Result = new ObjectResult(result);
            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            filterContext.ExceptionHandled = true;
        }
    }
}

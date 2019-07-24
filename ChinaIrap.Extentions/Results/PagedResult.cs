/*----------------------------------------------------------------
// Copyright © 2019 Chinairap.All rights reserved. 
// CLR版本：	4.0.30319.42000
// 类 名 称：    PagedResult
// 文 件 名：    PagedResult
// 创建者：      DUWENINK
// 创建日期：	2019/7/24 16:41:39
// 版本	日期					修改人	
// v0.1	2019/7/24 16:41:39	DUWENINK
//----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;

namespace ChinaIrap.Extentions
{
    /// <summary>
    /// 命名空间： ChinaIrap.Extentions.Results
    /// 创建者：   DUWENINK
    /// 创建日期： 2019/7/24 16:41:39
    /// 类名：     PagedResult(分页结果)
    /// </summary>
    public class PagedResult<T>
    {
        /// <summary>
        /// ctor
        /// </summary>
        public PagedResult()
        {
            rows = new List<T>();
        }

        /// <summary>
        /// ctor with params
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页显示数量</param>
        public PagedResult(int pageIndex, int pageSize)
        {
            page = pageIndex;
            pagesize = pageSize;
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int records { set; get; }
        /// <summary>
        /// 当前页的所有项
        /// </summary>
        public IList<T> rows { set; get; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int page { set; get; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int pagesize { set; get; }
        /// <summary>
        /// 页总数
        /// </summary>
        public int total { get { return records.CeilingDivide(pagesize); } }
    }
}

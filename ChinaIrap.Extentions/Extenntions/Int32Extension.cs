/*----------------------------------------------------------------
// Copyright © 2019 Chinairap.All rights reserved. 
// CLR版本：	4.0.30319.42000
// 类 名 称：    Int32Extension
// 文 件 名：    Int32Extension
// 创建者：      DUWENINK
// 创建日期：	2019/7/24 16:42:45
// 版本	日期					修改人	
// v0.1	2019/7/24 16:42:45	DUWENINK
//----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;

namespace ChinaIrap.Extentions
{
    /// <summary>
    /// 命名空间： ChinaIrap.Extentions.Extenntions
    /// 创建者：   DUWENINK
    /// 创建日期： 2019/7/24 16:42:45
    /// 类名：     Int32Extension
    /// </summary>
    public static class Int32Extension
    {
        /// <summary>
        /// 向上整除
        /// 1.当num能被divideBy整除时,结果即为num/divideBy;
        /// 2.当num不能被divideBy整除时,结果为num/divideBy + 1;
        /// </summary>
        /// <param name="num">被除数,大于或者等于0</param>
        /// <param name="divideBy">除数,大于0</param>
        /// <returns>向上整除结果</returns>
        public static int CeilingDivide(this int num, int divideBy)
        {
            if (num < 0) throw new ArgumentException("num");
            if (divideBy <= 0) throw new ArgumentException("divideBy");

            return (num + divideBy - 1) / divideBy;
        }
    }
}

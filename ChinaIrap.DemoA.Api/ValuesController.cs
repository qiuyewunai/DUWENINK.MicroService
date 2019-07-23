using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChinaIrap.DemoA.Api
{
    [Authorize("permission")]
    [Route("[controller]/[action]")]
    public class ValuesController : Controller
    {
        /// <summary>
        /// 获取请求
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "DemoA服务", "请求" };
        }
        /// <summary>
        /// 没有权限时提示
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Denied()
        {
            return new JsonResult(new
            {
                Status = false,
                Message = "DemoA服务你无权限访问"
            });
        }
        /// <summary>
        /// 唱歌
        /// </summary>
        /// <param name="songName">歌曲名</param>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<string> Sing(string songName)
        {
            return new string[] { songName, "清明雨上" };
        }


        /// <summary>
        /// 获取脑袋中的书籍
        /// </summary>
        /// <param name="bookDto"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetAll(BookDto bookDto)
        {
            return null;//空空如也
        }
        /// <summary>
        /// 书本传输对象
        /// </summary>
        public class BookDto
        {
            /// <summary>
            /// 书本名
            /// </summary>
            public string BookName { get; set; }
            /// <summary>
            /// 书本Id
            /// </summary>
            public string BookId { get; set; }
        }

    }
}

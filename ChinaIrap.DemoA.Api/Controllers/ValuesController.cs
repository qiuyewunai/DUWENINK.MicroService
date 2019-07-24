using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChinaIrap.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChinaIrap.DemoA.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize("permission")]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        /// <summary>
        /// 获取请求// GET api/values
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public NormalResult<string> Get()
        {
            var i = 0;
            var k = 5 / i;

            return new NormalResult<string>(){ Message="发送成功" };
        }

        /// <summary>
        ///  // GET api/values/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return $"Id为{id}";
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        /// <summary>
        /// 没有权限时提示
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("/api/denied")]
        public IActionResult Denied()
        {
            return new JsonResult(new
            {
                Status = false,
                Message = "DemoA服务你无权限访问"
            });
        }
        /// <summary>
        ///  // POST api/values
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        /// <summary>
        /// 唱歌
        /// </summary>
        /// <param name="songName">歌曲名</param>
        /// <returns></returns>
        [HttpPost("{songName}")]
        public IEnumerable<string> Sing(string songName)
        {
            return new string[] { songName, "清明雨上" };
        }
    

        ///// <summary>
        ///// 获取脑袋中的书籍
        ///// </summary>
        ///// <param name="bookDto"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public IActionResult GetAll(BookDto bookDto)
        //{
        //    return null;//空空如也
        //}
        ///// <summary>
        ///// 书本传输对象
        ///// </summary>
        //public class BookDto
        //{
        //    /// <summary>
        //    /// 书本名
        //    /// </summary>
        //    public string BookName { get; set; }
        //    /// <summary>
        //    /// 书本Id
        //    /// </summary>
        //    public string BookId { get; set; }
        //}

    }
}

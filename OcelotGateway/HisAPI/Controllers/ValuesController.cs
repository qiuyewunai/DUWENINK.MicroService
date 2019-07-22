using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoAAPI.Controllers
{
    [Authorize("Permission")]
    [Route("demoaapi/[controller]")]
    public class ValuesController : Controller
    {       
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "DemoA服务", "请求" };
        }
        [AllowAnonymous]
        [HttpGet("/demoaapi/denied")]
        public IActionResult Denied()
        {
            return new JsonResult(new
            {
                Status = false,
                Message = "demoaapi你无权限访问"
            });
        }
    }
}

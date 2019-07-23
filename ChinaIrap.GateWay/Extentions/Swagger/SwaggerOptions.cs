using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChinaIrap.GateWay
{
    public class SwaggerOptions
    {
        public bool UseSwagger { get; set; }

        public SwaggerConfig SwaggerConfig { get; set;}


    }
    public class SwaggerConfig
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
        /// 此网关负责转发的微服务
        /// </summary>
        public List<string> Apis { get; set; }

    }
}

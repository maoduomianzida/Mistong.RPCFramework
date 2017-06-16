using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
{
    public class ServiceCheckConfig
    {
        /// <summary>
        /// 服务健康检查时间间隔，默认10秒
        /// </summary>
        public TimeSpan? Interval { get; set; }

        /// <summary>
        /// 服务健康检查超时时间，默认3秒
        /// </summary>
        public TimeSpan? Timeout { get; set; }

        /// <summary>
        /// 服务健康检查类型，默认 "tcp"
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 服务健康检查端口号，没有默认
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 服务健康检查地址，默认本机地址，不需要赋值
        /// </summary>
        public string Address { get; set; }
    }
}
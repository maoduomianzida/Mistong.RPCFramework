using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
{
    public class ThriftClientConfig : ClientConfig
    {
        /// <summary>
        /// 客户端连接池配置
        /// </summary>
        public ThriftConnectionPoolConfig ConnectionPool { get; set; }

        /// <summary>
        /// 客户端熔断器配置
        /// </summary>
        public CircuitBreakerConfig CircuitBreaker { get; set; }
    }
}
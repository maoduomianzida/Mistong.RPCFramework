using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
{
    public class ThriftServerConfig : ServerConfig
    {
        /// <summary>
        /// 等待Consul操作超时时间，默认3秒
        /// </summary>
        public TimeSpan? WaitConsulTime { get; set; }
        
        /// <summary>
        /// 服务健康检查配置信息
        /// </summary>
        public ServiceCheckConfig ServiceCheck { get; set; }
    }
}
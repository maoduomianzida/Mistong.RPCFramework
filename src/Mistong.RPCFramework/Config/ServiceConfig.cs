using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework
{
    /// <summary>
    /// 服务配置信息
    /// </summary>
    public class ServiceConfig
    {
        /// <summary>
        /// 配置中心信息
        /// </summary>
        public RegistrationCenter RegistrationCenter { get; set; }

        /// <summary>
        /// 服务端配置的服务信息
        /// </summary>
        public ServerConfig Server { get; set; }

        /// <summary>
        /// 客户端配置的服务信息
        /// </summary>
        public ClientConfig Client { get; set; }
    }
}
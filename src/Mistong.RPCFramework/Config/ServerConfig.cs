using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework
{
    public class ServerConfig
    {
        public ServerConfig()
        {
            Services = new List<Service>();
        }

        /// <summary>
        /// 服务端配置的服务信息
        /// </summary>
        public ICollection<Service> Services { get; set; }
    }
}

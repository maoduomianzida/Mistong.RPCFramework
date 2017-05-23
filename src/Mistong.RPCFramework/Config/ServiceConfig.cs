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
        public ConfigCenter ConfigCenter { get; set; }

        /// <summary>
        /// 服务信息
        /// </summary>
        public ICollection<Service> Services { get; set; }
    }
}
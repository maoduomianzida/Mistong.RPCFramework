using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework
{
    /// <summary>
    /// 服务注册接口
    /// </summary>
    public interface IServiceRegistry
    {
        /// <summary>
        /// 配置中心信息
        /// </summary>
        ConfigCenter ConfigCenter { get; set; }

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="services"></param>
        void Register(ICollection<Service> services);
    }
}
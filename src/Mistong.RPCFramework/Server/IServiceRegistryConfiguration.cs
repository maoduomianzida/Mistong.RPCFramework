using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework
{
    /// <summary>
    /// 服务注册配置接口
    /// </summary>
    public interface IServiceRegistryConfiguration
    {
        /// <summary>
        /// 获取服务注册配置
        /// </summary>
        /// <returns></returns>
        ServiceConfig GetServiceConfig();
    }
}
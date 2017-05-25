using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework
{
    /// <summary>
    /// 服务发现配置信息接口
    /// </summary>
    public interface IServiceDiscovererConfiguration
    {
        /// <summary>
        /// 获取服务发现的配置
        /// </summary>
        /// <returns></returns>
        ServiceConfig GetServiceConfig();
    }
}
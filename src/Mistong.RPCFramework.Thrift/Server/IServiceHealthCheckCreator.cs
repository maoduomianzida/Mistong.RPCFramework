using Consul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
{
    /// <summary>
    /// 服务健康检查接口
    /// </summary>
    public interface IServiceHealthCheckCreator
    {
        /// <summary>
        /// 创建Consul健康检查信息
        /// </summary>
        /// <returns></returns>
        AgentCheckRegistration CreateCheck();

        /// <summary>
        /// 开启服务健康检查接口
        /// </summary>
        void EnableHealthCheckInterface();
    }
}
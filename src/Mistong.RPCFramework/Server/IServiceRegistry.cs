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
        /// 注册服务
        /// </summary>
        /// <param name="services"></param>
        void Register(IEnumerable<Service> services);
    }
}
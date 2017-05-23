using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework
{
    /// <summary>
    /// 服务启动接口
    /// </summary>
    public interface IServiceStarter
    {
        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="services"></param>
        void Start(ICollection<Service> services);
    }
}
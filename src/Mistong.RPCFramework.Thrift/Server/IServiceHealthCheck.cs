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
    public interface IServiceHealthCheck
    {
        void AddHeadlthCheck(ICollection<Service> services);
    }
}
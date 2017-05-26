using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
{
    /// <summary>
    /// thrift client 创建接口
    /// </summary>
    public interface IThriftClientActivator
    {
        void RegisterServices(IEnumerable<Service> services);

        object Create(Type thriftType);
    }
}
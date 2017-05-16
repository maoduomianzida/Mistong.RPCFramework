using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.ThriftClient
{
    public class ServiceAssembliesResolver : IServiceAssembliesResolver
    {
        public IEnumerable<Assembly> GetAssemblies()
        {
            return new Assembly[] { Assembly.Load("Mistong.RPCFramework.ThriftClient.Thrift") };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;

namespace Mistong.RPCFramework.Thrift
{
    public class ThriftServiceRegistry : IServiceRegistry
    {
        private IConsulClient _client;

        public ThriftServiceRegistry()
        {

        }

        public void Register(IEnumerable<Service> services)
        {
            
        }
    }
}
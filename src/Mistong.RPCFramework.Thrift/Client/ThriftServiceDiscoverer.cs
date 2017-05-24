using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
{
    public class ThriftServiceDiscoverer : IServiceDiscoverer
    {
        public IEnumerable<Service> Discover()
        {
            return new ThriftService[]
            {
                new ThriftService { Address = "192.168.1.1", Name = "Mistong.Services.UserService", Port = 100 , ServiceType = null },
                new ThriftService { Address = "192.168.1.2", Name = "Mistong.Services.OrderService", Port = 100, ServiceType = null }
            };
        }
    }
}
using Mistong.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift.Protocol;

namespace Mistong.RPCFramework.Thrift
{
    public class ThriftServiceStarter : IServiceStarter
    {
        private ILogger _logger;

        public ThriftServiceStarter(ILogger logger)
        {
            _logger = logger;
        }

        public void Start(ICollection<Service> services)
        {
            IEnumerable<ThriftService> thriftServices = services.Cast<ThriftService>();
            TMultiplexedProcessor multiplexedProcessor = new TMultiplexedProcessor();

        }
    }
}

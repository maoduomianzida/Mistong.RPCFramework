using Consul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
{
    internal static class AgentServiceHelper
    {

        public static AgentServiceRegistration CreateAgentService(ThriftService service)
        {
            return new AgentServiceRegistration
            {
                Name = service.Name,
                Address = service.Address,
                Port = service.Port,
                Tags = new string[] { service.Type, ThriftServiceHelper.ExtractThriftInterface(service.ServiceType)?.FullName, service.Transport?.GetType().FullName }
            };
        }
    }
}
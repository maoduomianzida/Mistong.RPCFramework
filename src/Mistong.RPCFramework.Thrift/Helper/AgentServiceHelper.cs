using Consul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                Tags = SerializeServiceFalg(service)
            };
        }

        public static string[] SerializeServiceFalg(ThriftService service)
        {
            return new string[] { service.Type, service.ServiceInterfaceType?.FullName };
        }

        public static Tuple<string,Type> DeserializeServiceFalg(string[] serviceFlag)
        {
            if(serviceFlag != null && serviceFlag.Length >= 2)
            {
                string serviceType = serviceFlag[0];
                string serviceInterfaceStr = serviceFlag[1];
                Type interfaceType = GetTypeFromAssemblies(serviceInterfaceStr);
                if (interfaceType != null)
                    return new Tuple<string, Type>(serviceType, interfaceType);
            }

            return null;
        }

        public static Type GetTypeFromAssemblies(string serviceInterfaceStr)
        {
            IServiceAssembliesResolver assembliesResolver = GlobalSetting.GetService<IServiceAssembliesResolver>();
            if(assembliesResolver == null)
            {
                throw new NullReferenceException("IServiceAssembliesResolver接口不能为空");
            }
            IEnumerable<Assembly> assemblies = assembliesResolver.GetAssemblies();
            foreach(Assembly assembly in assemblies.Where(tmp => tmp != null))
            {
                Type interfaceType = assembly.GetType(serviceInterfaceStr);
                if(interfaceType != null)
                {
                    return interfaceType;
                }
            }

            return null;
        }
    }
}
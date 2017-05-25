using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Thrift;

namespace Mistong.RPCFramework.Thrift
{
    public class ThriftServiceFinder : IServiceFinder
    {
        public virtual IEnumerable<ServiceMap> Find(IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(Find);
        }

        protected virtual IEnumerable<ServiceMap> Find(Assembly assembly)
        {
            foreach (Type processorType in assembly.GetTypes().Where(IsThriftProcessor))
            {
                ServiceMap serviceMap = GetServiceMap(processorType);
                if (serviceMap != null)
                    yield return serviceMap;
            }
        }

        protected virtual bool IsThriftProcessor(Type type)
        {
            return type != null && !type.IsAbstract && !type.IsGenericType && typeof(TProcessor).IsAssignableFrom(type);
        }

        protected virtual ServiceMap GetServiceMap(Type processorType)
        {
            string serviceName = processorType.DeclaringType.FullName;
            string assemblyName = processorType.Assembly.FullName;
            Type serviceType = Type.GetType(serviceName + "+Iface," + assemblyName);
            Type implementType = Type.GetType(serviceName + "+Client," + assemblyName);
            if (serviceType != null && implementType != null)
                return new ServiceMap(serviceType, implementType);

            return null;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift;
using Thrift.Protocol;
using Thrift.Transport;

namespace Mistong.RPCFramework.Thrift
{
    public class ThriftClientActivator : IThriftClientActivator
    {
        private HashSet<ThriftService> _services;

        public ThriftClientActivator()
        {
            _services = new HashSet<ThriftService>();
        }

        public object Create(Type thriftType)
        {
            if (thriftType == null)
            {
                throw new ArgumentNullException(nameof(thriftType));
            }
            IThriftConnectionPool connectionPool = GlobalSetting.Container.GetService<IThriftConnectionPool>();
            if (connectionPool == null)
            {
                throw new NullReferenceException("IThriftConnectionPool接口不能为空");
            }
            Service service = FindService(thriftType);
            if (service == null)
            {
                throw new NullReferenceException($"未找到类型{thriftType.FullName}的服务");
            }
            TTransport transport = connectionPool.GetTransport(service);
            if (transport == null) throw new ThriftConnectionFullException();

            return CreateInstance(service, transport);
        }

        protected virtual object CreateInstance(Service service, TTransport transport)
        {
            ThriftService thriftService = service as ThriftService;
            if (thriftService == null)
                throw new InvalidCastException("无法转换成ThriftService");
            if (transport == null)
                throw new ArgumentNullException(nameof(transport));
            if (!transport.IsOpen)
                transport.Open();
            TProtocol protocol = new TBinaryProtocol(transport);
            TMultiplexedProtocol multiplexedProtocol = new TMultiplexedProtocol(protocol, thriftService.Name);
            object instance = Activator.CreateInstance(thriftService.ServiceType, multiplexedProtocol);
            IDynamicProxyBuilder proxyBuilder = GlobalSetting.GetService<IDynamicProxyBuilder>();
            if (proxyBuilder == null)
            {
                return instance;
            }
            Type proxyType = proxyBuilder.CreateProxy(thriftService.ServiceInterfaceType);

            return Activator.CreateInstance(proxyType, instance);
        }

        protected virtual Service FindService(Type thriftType)
        {
            return _services.Where(service => thriftType.IsAssignableFrom(service.ServiceType)).SingleOrDefault();
        }

        public virtual void RegisterServices(IEnumerable<Service> services)
        {
            _services = new HashSet<ThriftService>(services.Cast<ThriftService>());
            ThriftServiceContainer thriftContainer = GlobalSetting.Container as ThriftServiceContainer;
            if (thriftContainer == null)
                throw new NullReferenceException("IServiceContainer接口无法转换成ThriftServiceContainer");
            foreach (ThriftService servic in _services)
            {
                thriftContainer.Add(servic.ServiceInterfaceType, type => Create(type));
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
{
    public class ThriftServiceContainer : IServiceContainer
    {
        private Dictionary<Type, object> _cache;
        private Dictionary<Type, Func<Type, object>> _funcCache;
        public FilterCollection Filters { get; private set; }

        public ThriftServiceContainer()
        {
            _cache = new Dictionary<Type, object>();
            _funcCache = new Dictionary<Type, Func<Type, object>>();
            Filters = new FilterCollection();
            Init();
        }

        protected virtual void Init()
        {
            ThriftServiceConfiguration configuration = new ThriftServiceConfiguration();
            ServiceConfig serviceConfig = configuration.GetServiceConfig();
            ThriftClientConfig clientConfig = serviceConfig.Client as ThriftClientConfig;
            if (clientConfig == null) throw new Exception("ClientConfig必须为ThriftClientConfig类型");
            _cache.Add(typeof(IServiceConfiguration), configuration);
            _cache.Add(typeof(IServiceRegistry), new ThriftServiceRegistry());
            _cache.Add(typeof(IServerController), new ThriftServerController());
            _cache.Add(typeof(IServiceActivator), new ThriftServiceActivator());

            _cache.Add(typeof(IServiceAssembliesResolver), new ThriftServiceAssembliesResolver());
            _cache.Add(typeof(IServiceFinder), new ThriftServiceFinder());
            _cache.Add(typeof(IServiceDiscoverer), new ThriftServiceDiscoverer());
            _cache.Add(typeof(IThriftClientActivator), new ThriftClientActivator());
            _cache.Add(typeof(IThriftConnectionPool), new FreshConnectionPool());
            //_cache.Add(typeof(IThriftConnectionPool), 
            //    new ThriftConnectionPool(clientConfig.ConnectionLimit.Value, 
            //                                             clientConfig.WaitFreeMillisecond.Value,
            //                                             clientConfig.WaitFreeTimes.Value,
            //                                             clientConfig.ConnectionOverdueInterval.Value));
            _cache.Add(typeof(IClientController), new ThriftClientController());
            _cache.Add(typeof(IDynamicProxyBuilder), new ThriftDynamicProxy("Mistong.RPCFramework.Thrift"));
            this.AddExceptionFilter(new MissingResultExceptionFilter());
        }

        public virtual void Add(Type type, Func<Type, object> func)
        {
            if (_funcCache.ContainsKey(type))
            {
                throw new Exception($"{type.FullName}已经存在，无法添加");
            }
            _funcCache.Add(type, func);
        }

        public virtual void Add(Type type, object instance)
        {
            if (_cache.ContainsKey(type))
            {
                throw new Exception($"{type.FullName}已经存在，无法添加");
            }
            _cache.Add(type, instance);
        }

        public virtual void Reaplce(Type type, object instance)
        {
            if (_cache.ContainsKey(type))
            {
                _cache[type] = instance;
            }
            else
            {
                _cache.Add(type, instance);
            }
        }

        public virtual void Remove(Type type)
        {
            if (_cache.ContainsKey(type))
            {
                _cache.Remove(type);
            }
        }

        public virtual object GetService(Type serviceType)
        {
            if (_cache.ContainsKey(serviceType))
            {
                return _cache[serviceType];
            }
            else if (_funcCache.ContainsKey(serviceType))
            {
                return _funcCache[serviceType]?.Invoke(serviceType);
            }

            return null;
        }
    }
}
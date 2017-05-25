using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
{
    public class ThriftServiceContainer : IServiceContainer
    {
        private Dictionary<Type, object> _singleCache;
        private Dictionary<Type, IEnumerable<object>> _multipleCache;

        public ThriftServiceContainer()
        {
            _singleCache = new Dictionary<Type, object>();
            _multipleCache = new Dictionary<Type, IEnumerable<object>>();
            Init();
        }

        protected virtual void Init()
        {
            ThriftServiceConfiguration configuration = new ThriftServiceConfiguration();
            _singleCache.Add(typeof(IServiceRegistryConfiguration), configuration);
            _singleCache.Add(typeof(IServiceRegistry), new ThriftServiceRegistry());
            _singleCache.Add(typeof(IServiceController), new ThriftServiceController());
            _singleCache.Add(typeof(IServiceActivator), new ThriftServiceActivator());

            _singleCache.Add(typeof(IServiceDiscovererConfiguration), configuration);
            _singleCache.Add(typeof(IServiceAssembliesResolver),new ThriftServiceAssembliesResolver());
            _singleCache.Add(typeof(IServiceFinder),new ThriftServiceFinder());
            _singleCache.Add(typeof(IServiceDiscoverer),new ThriftServiceDiscoverer());
            _singleCache.Add(typeof(IServiceMatcher),new ThriftServiceMatcher());
        }

        public virtual void Add(Type type,object instance)
        {
            if(_singleCache.ContainsKey(type))
            {
                throw new Exception($"{type.FullName}已经存在，无法添加");
            }
            _singleCache.Add(type,instance);
        }

        public virtual void Add(Type type, IEnumerable<object> instances)
        {
            if (_multipleCache.ContainsKey(type))
            {
                throw new Exception($"{type.FullName}已经存在，无法添加");
            }
            _multipleCache.Add(type, instances);
        }

        public virtual void Reaplce(Type type,object instance)
        {
            if(_singleCache.ContainsKey(type))
            {
                _singleCache[type] = instance;
            }
            else
            {
                _singleCache.Add(type,instance);
            }
        }

        public virtual void Reaplce(Type type,IEnumerable<object> instances)
        {
            if (_multipleCache.ContainsKey(type))
            {
                _multipleCache[type] = instances;
            }
            else
            {
                _multipleCache.Add(type, instances);
            }
        }

        public virtual void Remove(Type type)
        {
            if(_singleCache.ContainsKey(type))
            {
                _singleCache.Remove(type);
            }
            else if(_multipleCache.ContainsKey(type))
            {
                _multipleCache.Remove(type);
            }
        }
        
        public virtual object GetService(Type serviceType)
        {
            if(_singleCache.ContainsKey(serviceType))
            {
                return _singleCache[serviceType];
            }

            return null;
        }

        public virtual IEnumerable<object> GetServices(Type serviceType)
        {
            if (_multipleCache.ContainsKey(serviceType))
            {
                return _multipleCache[serviceType];
            }

            return null;
        }
    }
}

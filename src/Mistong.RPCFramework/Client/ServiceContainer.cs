using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework
{
    /*public sealed class ServiceContainer
    {
        private ICollection<IServiceFinder> _finderCollections;
        private IServiceAssembliesResolver _assembliesResolver;
        private HashSet<ServiceMap> _serviceMap;

        public ServiceContainer()
        {
            _finderCollections = new Collection<IServiceFinder>();
            _serviceMap = new HashSet<ServiceMap>();
        }

        public void Add(IServiceFinder finder)
        {
            if (finder == null) throw new ArgumentNullException(nameof(finder));

            _finderCollections.Add(finder);
        }

        public void SetAssemblies(IServiceAssembliesResolver assembliesResolver)
        {
            if(assembliesResolver == null) throw new ArgumentNullException(nameof(assembliesResolver));

            _assembliesResolver = assembliesResolver;
        }

        public void AutoFind()
        {
            _serviceMap = new HashSet<ServiceMap>(_finderCollections.SelectMany(item => item.Find(_assembliesResolver)));
        }

        public T GetService<T>() where T: class
        {
            Type implement = _serviceMap.Where(map => map.InheritInterface(typeof(T))).Select(map => map.Implement).SingleOrDefault();
            if(implement != null)
            {
                return Activator.CreateInstance(implement) as T;
            }

            return null;
        }
    }*/
}
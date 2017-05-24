using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework
{
    public static class ServiceContainerExtension
    {
        public static T GetService<T>(this IServiceContainer container) where T :class
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            return container.GetService(typeof(T)) as T;
        }

        public static IEnumerable<T> GetServices<T>(this IServiceContainer container) where T : class
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            IEnumerable<object> services = container.GetServices(typeof(T));

            return services.Cast<T>();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift.Extension
{
    public static class IServiceContainerExtension
    {
        private static ThriftServiceContainer MustThriftServiceContainer(IServiceContainer container)
        {
            ThriftServiceContainer realContainer = container as ThriftServiceContainer;
            if (realContainer == null)
                throw new NullReferenceException("非ThriftServiceContainer继承类无法使用该扩展方法");

            return realContainer;
        }

        public static void AddExceptionFilter(this IServiceContainer container,IExceptionFilter filter)
        {
            container.AddFilter(new FilterInfo { Instance = filter, Order = int.MaxValue });
        }

        public static void AddActionFilter(this IServiceContainer container,IActionFilter filter)
        {
            container.AddFilter(new FilterInfo { Instance = filter, Order = int.MaxValue });
        }

        public static void AddFilter(this IServiceContainer container,FilterInfo filter)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            var realContainer = MustThriftServiceContainer(container);

            realContainer.Filters.Add(filter);
        }
    }
}

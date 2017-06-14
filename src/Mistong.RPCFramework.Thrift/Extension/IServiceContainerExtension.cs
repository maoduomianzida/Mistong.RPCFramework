using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
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

        public static T HandException<T>(this IServiceContainer container,ExceptionContext context)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            var realContainer = MustThriftServiceContainer(container);
            realContainer.Filters.Build();
            foreach(IExceptionFilter filter in realContainer.Filters.ExceptionFilters)
            {
                T result = filter.HandException<T>(context);
                if(context.HandException)
                {
                    return result;
                }
            }
            ExceptionDispatchInfo dispatch = ExceptionDispatchInfo.Capture(context.Exception);
            dispatch.Throw();

            return default(T);
        }

        public static void ActionExecuteBefore(this IServiceContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            var realContainer = MustThriftServiceContainer(container);
            realContainer.Filters.Build();
            foreach(IActionFilter filter in realContainer.Filters.ActionFilters)
            {
                filter.ExecuteBefore();
            }
        }

        public static void ActionExecuteAfter(this IServiceContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            var realContainer = MustThriftServiceContainer(container);
            realContainer.Filters.Build();
            foreach (IActionFilter filter in realContainer.Filters.ActionFilters.Reverse())
            {
                filter.ExecuteAfter();
            }
        }
    }
}
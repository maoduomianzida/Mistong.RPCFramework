using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;
using Mistong.RPCFramework.CircuitBreaker;

namespace Mistong.RPCFramework.Thrift
{
    public class CircuitBreakerFilter : IActionFilter, IExceptionFilter
    {
        private CircuitBreakerSetting _setting;
        private ConcurrentDictionary<ActionDescriptor, Lazy<CircuitBreaker.CircuitBreaker>> _circuitBreakerCache;

        public CircuitBreakerFilter(CircuitBreakerSetting setting)
        {
            _setting = setting ?? throw new ArgumentNullException(nameof(setting));
            _circuitBreakerCache = new ConcurrentDictionary<ActionDescriptor, Lazy<CircuitBreaker.CircuitBreaker>>();
        }

        public void ExecuteAfter(ActionResult result)
        {
            Lazy<CircuitBreaker.CircuitBreaker> circuitBreaker = _circuitBreakerCache.GetOrAdd(result.ActionDescriptor,
                action => new Lazy<CircuitBreaker.CircuitBreaker>(() => new CircuitBreaker.CircuitBreaker(_setting), LazyThreadSafetyMode.PublicationOnly));
            circuitBreaker.Value.ExecuteAfter();
        }

        public void ExecuteBefore(ActionContext context)
        {
            Lazy<CircuitBreaker.CircuitBreaker> circuitBreaker = _circuitBreakerCache.GetOrAdd(context.ActionDescriptor,
                action => new Lazy<CircuitBreaker.CircuitBreaker>(() => new CircuitBreaker.CircuitBreaker(_setting), LazyThreadSafetyMode.PublicationOnly));
            circuitBreaker.Value.ExecuteBefore();
        }

        public T HandException<T>(ExceptionContext context)
        {
            if(context.Exception is RemoteResourceException exception)
            {
                Lazy<CircuitBreaker.CircuitBreaker> circuitBreaker = _circuitBreakerCache.GetOrAdd(context.ActionDescriptor,
                    action => new Lazy<CircuitBreaker.CircuitBreaker>(() => new CircuitBreaker.CircuitBreaker(_setting), LazyThreadSafetyMode.PublicationOnly));
                circuitBreaker.Value.ExecuteFail(exception);
                context.HandException = true;

                return TypeHelper.GetDefalutValue<T>();
            }

            return default(T);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using Thrift;

namespace Mistong.RPCFramework.Thrift
{
    public class MissingResultExceptionFilter : IExceptionFilter
    {
        public T HandException<T>(ExceptionContext context)
        {
            if (typeof(TApplicationException).IsAssignableFrom(context.Exception.GetType()))
            {
                context.HandException = true;
                return TypeHelper.GetDefalutValue<T>();
            }
            ExceptionDispatchInfo dispatch = ExceptionDispatchInfo.Capture(context.Exception);
            dispatch.Throw();

            return default(T);
        }
    }
}
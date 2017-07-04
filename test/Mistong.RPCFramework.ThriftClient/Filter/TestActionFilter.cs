using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mistong.RPCFramework.Thrift;

namespace Mistong.RPCFramework.ThriftClient
{
    public class TestActionFilter : IActionFilter ,IExceptionFilter
    {
        public void ExecuteAfter(ActionResult result)
        {
            Console.WriteLine("执行后");
        }

        public void ExecuteBefore(ActionContext context)
        {
            Console.WriteLine("执行前");
        }

        public T HandException<T>(ExceptionContext context)
        {
            context.HandException = true;
            return default(T);
        }
    }
}
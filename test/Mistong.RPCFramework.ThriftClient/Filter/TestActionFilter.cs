using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mistong.RPCFramework.Thrift;

namespace Mistong.RPCFramework.ThriftClient
{
    public class TestActionFilter : IActionFilter
    {
        public void ExecuteAfter(ActionResult result)
        {
            Console.WriteLine("执行后");
        }

        public void ExecuteBefore(ActionContext context)
        {
            Console.WriteLine("执行前");
        }
    }
}
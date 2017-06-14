using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.ThriftClient
{
    public class TestActionFilter : IActionFilter
    {
        public void ExecuteAfter()
        {
            lock(this)
            {
                Console.WriteLine("执行后");
            }
        }

        public void ExecuteBefore()
        {
            lock (this)
            {
                Console.WriteLine("执行前");
            }
        }
    }
}

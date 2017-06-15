using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
{
    public class ThriftConnectionFullException : Exception
    {
        public ThriftConnectionFullException():base("连接池已满，无法分配更多的连接")
        {
        }
    }
}

using Mistong.RPCFramework.Thrift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.ThriftServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ThriftServiceContainer cotainer = new ThriftServiceContainer();
            GlobalSetting.Start(cotainer);
        }
    }
}
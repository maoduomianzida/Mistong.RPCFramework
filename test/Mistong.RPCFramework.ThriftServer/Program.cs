using Consul;
using Mistong.RPCFramework.Thrift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.ThriftServer
{

    class Program
    {
        static void Main(string[] args)
        {
            GlobalSetting.Start(new ThriftServiceContainer());
        }
    }
}
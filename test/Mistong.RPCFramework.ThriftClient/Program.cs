using Mistong.RPCFramework.Thrift;
using Mistong.Services.UserService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thrift.Transport;

namespace Mistong.RPCFramework.ThriftClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string name = AppDomain.CurrentDomain.FriendlyName;
            int index = name.LastIndexOf(".");
            name = name.Substring(0, index);
            Console.WriteLine(name);
            //GlobalSetting.Start(new ThriftServiceContainer());
        }
    }
}
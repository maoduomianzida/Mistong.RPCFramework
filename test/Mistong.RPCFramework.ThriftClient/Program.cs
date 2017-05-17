using Mistong.RPCFramework.Thrift;
using Mistong.Services.UserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.ThriftClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ThriftServiceFinder finder = new ThriftServiceFinder();
            List<ServiceMap> list = finder.Find(new ServiceAssembliesResolver()).ToList();
        }
    }
}
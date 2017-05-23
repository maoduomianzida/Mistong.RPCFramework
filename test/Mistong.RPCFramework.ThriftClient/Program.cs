using Mistong.RPCFramework.Thrift;
using Mistong.Services.UserService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift.Transport;

namespace Mistong.RPCFramework.ThriftClient
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            ConfigCenter center = new ConsulServiceConfig();
            center.Type = "consul";
            center.Clusters = new List<string> { "127.0.0.1:3000" };
            List<ThriftService> services = new List<ThriftService>();
            services.Add(new ThriftService { Address = "", Name = "", Port = 100, ServiceType = typeof(UserService), Transport = new TServerSocket(100) });
            ConsulServiceConfig config = new ConsulServiceConfig(center, services);

            Console.WriteLine(JsonConvert.SerializeObject(config));
            return;

            ThriftServiceFinder finder = new ThriftServiceFinder();
            List<ServiceMap> list = finder.Find(new ServiceAssembliesResolver()).ToList();*/

            Console.WriteLine(string.Join(",",typeof(object).GetInterfaces().Select(tmp => tmp.FullName)));

        }
    }
}
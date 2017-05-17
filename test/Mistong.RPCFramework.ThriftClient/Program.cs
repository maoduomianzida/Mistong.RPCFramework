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
            ConfigCenter center = new ConfigCenter();
            center.Type = "consul";
            center.Clusters = new List<string> { "127.0.0.1:3000" };
            List<ThriftService> services = new List<ThriftService>();
            services.Add(new ThriftService { Address = "", Name = "", Port = 100, ServiceType = typeof(UserService), Transport = new TServerSocket(100) });
            ThriftServiceConfig config = new ThriftServiceConfig(center, services);

            Console.WriteLine(JsonConvert.SerializeObject(config));
            return;

            ThriftServiceFinder finder = new ThriftServiceFinder();
            List<ServiceMap> list = finder.Find(new ServiceAssembliesResolver()).ToList();
        }
    }
}
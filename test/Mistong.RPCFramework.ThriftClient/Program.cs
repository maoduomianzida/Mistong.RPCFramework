using Mistong.RPCFramework.Thrift;
using Mistong.Services.UserService;
using Newtonsoft.Json;
using System;
using System.CodeDom.Compiler;
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
            ThriftServiceContainer container = new ThriftServiceContainer();
            //SelfServiceAssembliesResolver resolver = new SelfServiceAssembliesResolver();
            //container.Reaplce(typeof(IServiceAssembliesResolver), resolver);
            GlobalSetting.Start(container);

            UserService.Iface userService = GlobalSetting.GetService<UserService.Iface>();
            bool result = userService.Add(new UserInfo { UserID = 10, UserName = "wg.king", Sex = true });

            Console.WriteLine(result ? "添加成功" : "添加失败");
            //UserInfo user = userService.GetUser(10);
            //Console.WriteLine(JsonConvert.SerializeObject(user));

            //Console.ReadLine();
        }
    }
}
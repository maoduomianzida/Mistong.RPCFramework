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
            GlobalSetting.Start(new ThriftServiceContainer());

            UserService.Iface userService = GlobalSetting.Container.GetService<UserService.Iface>();
            //userService.Add(new UserInfo { UserID = 10, UserName = "wg.king", Sex = true });

            UserInfo user = userService.GetUser(10);
            Console.WriteLine(JsonConvert.SerializeObject(user));
        }
    }
}
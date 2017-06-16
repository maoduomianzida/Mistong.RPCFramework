using Mistong.RPCFramework.Thrift;
using Mistong.Services.UserService;
using Newtonsoft.Json;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
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
            //container.Reaplce(typeof(IThriftConnectionPool),new FreshConnectionPool());
            //container.AddActionFilter(new TestActionFilter());
            //SelfServiceAssembliesResolver resolver = new SelfServiceAssembliesResolver();
            //container.Reaplce(typeof(IServiceAssembliesResolver), resolver);
            GlobalSetting.Start(container);

            UserService.Iface tmpService = GlobalSetting.GetService<UserService.Iface>();
            tmpService.Add(new UserInfo { UserID = 10, UserName = "wg.king", Sex = true });
            int threadCount = 2000;
            int count = 0;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < threadCount; i++)
            {
                Thread thread = new Thread(() =>
                {
                    IDisposable dis = null;
                    try
                    {
                        UserService.Iface userService = GlobalSetting.GetService<UserService.Iface>();

                        for (int j = 0; j < 10; j++)
                        {
                            UserInfo user = userService.GetUser(10);
                        }

                        Thread.Sleep(100);
                        dis = userService as IDisposable;
                        Console.WriteLine("OK");
                    }
                    catch(Exception err)
                    {
                        Console.WriteLine(err.Message);
                    }
                    finally
                    {
                        dis?.Dispose();
                        Interlocked.Increment(ref count);
                    }
                });
                thread.Start();
            }
            bool timeout = false;
            //Timer timer = new Timer((state) => timeout = true,null,10000,-1);
            while (count < threadCount && !timeout)
            {
            }
            if (timeout)
            {
                Console.WriteLine($"超时:执行成功{count}次");
            }
            else
            {
                Console.WriteLine($"正常结束:执行成功{count}次");
            }
            watch.Stop();
            Console.WriteLine("耗时" + watch.ElapsedMilliseconds);
            ThriftConnectionPool pool = GlobalSetting.GetService<IThriftConnectionPool>() as ThriftConnectionPool;
            foreach (var item in pool.ConnectionStore.ConnectionPool)
            {
                Console.WriteLine("连接池内的TTransport：" + item.Value.Count);
                int index = 0;
                foreach(var tmp in item.Value)
                {
                    index++;
                    Console.WriteLine(index + " " + (tmp.IsFree ? "空闲":"忙碌"));
                }
            }
            Thread.Sleep(9 * 1000);
            UserService.Iface face = GlobalSetting.GetService<UserService.Iface>();
            Console.WriteLine("剩余连接：" + pool.ConnectionStore.ConnectionPool.First().Value.Count);

            Console.ReadKey();
        }
    }
}
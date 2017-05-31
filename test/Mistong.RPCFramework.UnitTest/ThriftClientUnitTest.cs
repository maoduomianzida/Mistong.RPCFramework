using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mistong.RPCFramework.Thrift;
using Mistong.Services.UserService;
using Mistong.RPCFramework.ThriftClient;
using System.Threading;

namespace Mistong.RPCFramework.UnitTest
{
    /// <summary>
    /// ThriftClientUnitTest 的摘要说明
    /// </summary>
    [TestClass]
    public class ThriftClientUnitTest
    {
        public ThriftClientUnitTest()
        {
            //
            //TODO:  在此处添加构造函数逻辑
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        private ThriftServiceContainer _container = new ThriftServiceContainer();

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性: 
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        [TestInitialize()]
        public void MyTestInitialize()
        {
            _container.Reaplce(typeof(IServiceAssembliesResolver), new ServiceAssembliesResolver());
            GlobalSetting.Start(_container);
        }

        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void 客户端新增一条数据()
        {
            UserService.Iface userService = GlobalSetting.GetService<UserService.Iface>();
            Assert.AreEqual(true, userService.Add(new UserInfo { UserID = 10, UserName = "wg.king", Sex = true }));
        }

        [TestMethod]
        public void 客户端单个请求()
        {
            UserService.Iface userService = GlobalSetting.GetService<UserService.Iface>();
            for (int j = 0; j < 1000; j++)
            {
                UserInfo user = userService.GetUser(10);
                Assert.IsNotNull(user);
                Assert.AreEqual(10, user.UserID);
            }
        }

        private void Test()
        {
            int threadCount = 400;
            int count = 0;
            for (int i = 0; i < threadCount; i++)
            {
                Thread thread = new Thread(() =>
                {
                    UserService.Iface userService = GlobalSetting.GetService<UserService.Iface>();
                    for (int j = 0; j < 10; j++)
                    {
                        UserInfo user = userService.GetUser(10);
                    }
                    IDisposable dispose = userService as IDisposable;
                    dispose.Dispose();
                    Interlocked.Increment(ref count);
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
        }

        /// <summary>
        /// 测试结果：TTransport 不是线程安全的，无法被多个线程共用（服务端抛出各种异常）
        ///                   thrift 客户端在每次执行完，必须显示调用Dispose()方法
        ///                   （忘记Dispose造成与服务端的连接无法被断开，导致服务端连接池被占满，其它客户端无法调用）
        /// </summary>
        [TestMethod]
        public void 单例TTransport多线程发送请求()
        {
            _container.Reaplce(typeof(IThriftConnectionPool), new SingletonConnectionPool());
            Test();
        }

        [TestMethod]
        public void 每次都NewTTransport多线程发送请求()
        {
            _container.Reaplce(typeof(IThriftConnectionPool), new FreshConnectionPool());
            Test();
        }

        [TestMethod]
        public void 加锁限制每次只有一个线程使用TTransport进行发送()
        {

        }

        [TestMethod]
        public void 使用连接池内缓存的TTransport进行请求()
        {
            Test();
        }
    }
}
using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mistong.RPCFramework.Thrift;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Mistong.RPCFramework.UnitTest
{
    [TestClass]
    public class ThriftServiceRegistryConfigurationUnitTest
    {
        private ThriftServiceConfiguration _configuration;

        [TestInitialize]
        public void Init()
        {
            _configuration = new ThriftServiceConfiguration();
            GlobalSetting.SetContainer(new ThriftServiceContainer());
        }

        [TestMethod]
        public void 测试是否能反序列化()
        {
            ServiceConfig config = _configuration.GetServiceConfig();

            Assert.AreEqual(1, config.Server.Services.Count());
        }

        [TestMethod]
        public void 测试ThriftServiceConfig成功序列化()
        {
        }

        [TestMethod]
        public void 测试ConsulServiceRegistry注册()
        {
            ServiceConfig config = _configuration.GetServiceConfig();
            ThriftServiceRegistry registry = new ThriftServiceRegistry(config.RegistrationCenter,TimeSpan.FromSeconds(3));
            registry.Register(config.Server.Services);
        }

        [TestMethod]
        public void 测试ServiceController()
        {
            GlobalSetting.Start();
        }
    }
}
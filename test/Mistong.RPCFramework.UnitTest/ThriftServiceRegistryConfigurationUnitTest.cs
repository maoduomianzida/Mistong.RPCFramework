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
        private ConsulServiceRegistryConfiguration _configuration;

        [TestInitialize]
        public void Init()
        {
            _configuration = new ConsulServiceRegistryConfiguration();
            GlobalSetting.SetContainer(new ThriftServiceContainer());
        }

        [TestMethod]
        public void 测试是否能反序列化()
        {
            ServiceConfig config = _configuration.GetServiceConfig();

            Assert.AreEqual(1, config.Services.Count());
        }

        [TestMethod]
        public void 测试ThriftServiceConfig成功序列化()
        {
            ConfigCenter center = new ConsulConfigCenter() { Clusters = new List<string> { "172.16.211.146" } };
            List<Service> services = new List<Service> { new ThriftService { Address = "172.16.211.146", Name = "userservice", Port = 9000, ServiceType = typeof(UserServiceImplement)} };
            ServiceConfig config = new ServiceConfig() { Services = services, ConfigCenter = center };
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.ContractResolver = new CamelCasePropertyNamesContractResolver();
            setting.Converters.Add(new TypeConverter());
            //setting.Converters.Add(new ServiceCreationConverter());
            //setting.Converters.Add(new ConfigCenterCreationConverter());

            string json = JsonConvert.SerializeObject(config, Formatting.Indented, setting);

            Console.WriteLine(json);
        }

        [TestMethod]
        public void 测试ConsulServiceRegistry注册()
        {
            ServiceConfig config = _configuration.GetServiceConfig();
            ConsulServiceRegistry registry = new ConsulServiceRegistry();
            registry.ConfigCenter = config.ConfigCenter;
            registry.Register(config.Services);
        }

        [TestMethod]
        public void 测试ServiceController()
        {
            GlobalSetting.ServiceController.Start();
        }
    }
}
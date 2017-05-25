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

            Assert.AreEqual(1, config.Server.Count());
        }

        [TestMethod]
        public void 测试ThriftServiceConfig成功序列化()
        {
            RegistrationCenter center = new ConsulRegistrationCenter() { Clusters = new List<string> { "172.16.211.146" } };
            List<Service> services = new List<Service> { new ThriftService { Address = "172.16.211.146", Name = "userservice", Port = 9000, ServiceType = typeof(UserServiceImplement)} };
            ServiceConfig config = new ServiceConfig() { Server = services, RegistrationCenter = center };
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
            ThriftServiceRegistry registry = new ThriftServiceRegistry();
            registry.RegistrationCenter = config.RegistrationCenter;
            registry.Register(config.Server);
        }

        [TestMethod]
        public void 测试ServiceController()
        {
            GlobalSetting.ServiceController.Start();
        }
    }
}
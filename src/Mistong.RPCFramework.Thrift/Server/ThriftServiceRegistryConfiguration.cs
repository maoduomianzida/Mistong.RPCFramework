using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.Net.Sockets;

namespace Mistong.RPCFramework.Thrift
{
    public class ThriftServiceConfiguration : IServiceRegistryConfiguration, IServiceDiscovererConfiguration
    {
        private string _configPath;
        private IContractResolver _contractResolver;
        private IList<JsonConverter> _converts;

        public ThriftServiceConfiguration() :this(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"services.json"))
        { }

        public ThriftServiceConfiguration(string configPath)
        {
            if (string.IsNullOrWhiteSpace(configPath)) throw new ArgumentNullException(nameof(configPath));
            if (!File.Exists(configPath)) throw new FileNotFoundException("服务配置文件不存在",configPath);

            _configPath = configPath;
            InitJsonSetting();
        }

        private void InitJsonSetting()
        {
            _contractResolver = new CamelCasePropertyNamesContractResolver();
            _converts = new List<JsonConverter>();
            _converts.Add(new TypeConverter());
            _converts.Add(new ServiceCreationConverter());
            _converts.Add(new ConfigCenterCreationConverter());
        }

        private JsonSerializer CreateJsonSerializer()
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.ContractResolver = _contractResolver;
            foreach(JsonConverter conveter in _converts)
            {
                serializer.Converters.Add(conveter);
            }

            return serializer;
        }

        public virtual ServiceConfig GetServiceConfig()
        {
            using (Stream steam = File.Open(_configPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (StreamReader sr = new StreamReader(steam))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                JsonSerializer serializer = CreateJsonSerializer();
                ServiceConfig serviceConfig = serializer.Deserialize<ServiceConfig>(reader);
                FillAddress(serviceConfig.Server);
                FillAddress(serviceConfig.Client);

                return serviceConfig;
            }
        }

        protected virtual void FillAddress(IEnumerable<Service> services)
        {
            foreach(Service service in services.Where(tmp => string.IsNullOrWhiteSpace(tmp.Address)))
            {
                service.Address = Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork).ToString();
            }
        }
    }
}
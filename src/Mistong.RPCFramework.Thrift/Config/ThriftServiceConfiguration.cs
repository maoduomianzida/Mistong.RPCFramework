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
using System.Threading;

namespace Mistong.RPCFramework.Thrift
{
    public class ThriftServiceConfiguration : IServiceConfiguration
    {
        private string _configPath;
        private IContractResolver _contractResolver;
        private IList<JsonConverter> _converts;
        private ServiceConfig _result;

        public ThriftServiceConfiguration() : this(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "services.json"))
        { }

        public ThriftServiceConfiguration(string configPath)
        {
            if (string.IsNullOrWhiteSpace(configPath)) throw new ArgumentNullException(nameof(configPath));
            if (!File.Exists(configPath)) throw new FileNotFoundException("服务配置文件不存在", configPath);

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
            _converts.Add(new ThriftServerConfigConverter());
            _converts.Add(new ThriftClientConfigConverter());
        }

        private JsonSerializer CreateJsonSerializer()
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.ContractResolver = _contractResolver;
            foreach (JsonConverter conveter in _converts)
            {
                serializer.Converters.Add(conveter);
            }

            return serializer;
        }

        public virtual ServiceConfig GetServiceConfig()
        {
            if (_result == null)
            {
                lock (this)
                {
                    if (_result == null)
                    {
                        using (Stream steam = File.Open(_configPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                        using (StreamReader sr = new StreamReader(steam))
                        using (JsonReader reader = new JsonTextReader(sr))
                        {
                            JsonSerializer serializer = CreateJsonSerializer();
                            ServiceConfig serviceConfig = serializer.Deserialize<ServiceConfig>(reader);
                            FillValue(serviceConfig);

                            _result = serviceConfig;
                        }
                    }
                }
            }

            return _result;
        }

        protected virtual void FillValue(ServiceConfig serviceConfig)
        {
            if (serviceConfig.Server == null) serviceConfig.Server = new ThriftServerConfig();
            if (serviceConfig.Client == null) serviceConfig.Client = new ThriftClientConfig();
            FillServerConfig(serviceConfig.Server);
            FillClientConfig(serviceConfig.Client);
            FillAddress(serviceConfig.Server.Services);
            FillAddress(serviceConfig.Client.Services);
        }

        protected virtual void FillServerConfig(ServerConfig config)
        {
            ThriftServerConfig thriftConfig = config as ThriftServerConfig;
            if (thriftConfig != null)
            {
                thriftConfig.WaitConsulTime = thriftConfig.WaitConsulTime ?? TimeSpan.FromSeconds(3);
                thriftConfig.ServiceCheck = thriftConfig.ServiceCheck ?? new ServiceCheckConfig();
                thriftConfig.ServiceCheck.Address = GetLocalHost();
                thriftConfig.ServiceCheck.Interval = thriftConfig.ServiceCheck.Interval ?? TimeSpan.FromSeconds(10);
                thriftConfig.ServiceCheck.Timeout = thriftConfig.ServiceCheck.Timeout ?? TimeSpan.FromSeconds(3);
                thriftConfig.ServiceCheck.Type = thriftConfig.ServiceCheck.Type ?? "tcp";
            }
        }

        protected virtual void FillClientConfig(ClientConfig config)
        {
            ThriftClientConfig thriftConfig = config as ThriftClientConfig;
            if (thriftConfig != null)
            {
                //thriftConfig.ConnectionLimit = thriftConfig.ConnectionLimit ?? 50;
                //thriftConfig.ConnectionOverdueInterval = thriftConfig.ConnectionOverdueInterval ?? TimeSpan.FromMinutes(5);
                //thriftConfig.WaitFreeMillisecond = thriftConfig.WaitFreeMillisecond ?? 1000;
            }
        }

        private string GetLocalHost()
        {
            return Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork).ToString();
        }

        protected virtual void FillAddress(IEnumerable<Service> services)
        {
            foreach (Service service in services.Where(tmp => string.IsNullOrWhiteSpace(tmp.Address)))
            {
                service.Address = GetLocalHost();
            }
        }
    }
}
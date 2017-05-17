using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
{
    public class ConfigCenter
    {
        public string Type { get; set; }

        public List<string> Clusters { get; set; }
    }

    public class ThriftServiceConfig
    {
        public ThriftServiceConfig(ConfigCenter configCenter, ICollection<ThriftService> services)
        {
            if (configCenter == null) throw new ArgumentNullException(nameof(configCenter));

            ConfigCenter = configCenter;
            Services = services;
        }

        public ConfigCenter ConfigCenter { get; private set; }

        public ICollection<ThriftService> Services { get; private set; }
    }
}
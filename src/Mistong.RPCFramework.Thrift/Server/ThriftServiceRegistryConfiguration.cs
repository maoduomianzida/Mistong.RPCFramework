using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
{
    public class ThriftServiceRegistryConfiguration : IServiceRegistryConfiguration
    {
        private string _configPath;

        public ThriftServiceRegistryConfiguration(string configPath)
        {
            if (string.IsNullOrWhiteSpace(configPath)) throw new ArgumentNullException(nameof(configPath));

            _configPath = configPath;
        }

        public IEnumerable<Service> GetServices()
        {
            return null;
        }
    }
}
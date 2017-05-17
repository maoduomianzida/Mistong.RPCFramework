using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework
{
    public interface IServiceRegistryConfiguration
    {
        IEnumerable<Service> GetServices();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework
{
    public class ClientConfig
    {
        public ClientConfig()
        {
            Services = new List<Service>();
        }

        public ICollection<Service> Services { get; set; }
    }
}

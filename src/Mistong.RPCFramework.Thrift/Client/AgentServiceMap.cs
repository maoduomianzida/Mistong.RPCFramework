using Consul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
{
    public class AgentServiceMap
    {
        private Tuple<string, Type> _tags;
        private bool _finded;

        public AgentServiceMap(AgentService service)
        {
            Service = service;
        }

        public AgentService Service { get; set; }

        public Tuple<string, Type> Tags
        {
            get
            {
                if (!_finded)
                {
                    _tags = AgentServiceHelper.DeserializeServiceFalg(Service.Tags);
                    _finded = true;
                }

                return _tags;
            }
        }
    }
}

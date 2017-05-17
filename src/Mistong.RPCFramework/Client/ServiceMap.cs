using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework
{
    public class ServiceMap
    {
        public ServiceMap(Type _interface, Type implement)
        {
            if (_interface == null) throw new ArgumentNullException(nameof(_interface));
            if (implement == null) throw new ArgumentNullException(nameof(implement));

            Interface = _interface;
            Implement = implement;
        }

        public virtual bool InheritInterface(Type _interface)
        {
            if (_interface == null)
                return false;

            return _interface.IsAssignableFrom(Interface);
        }

        public Type Interface { get; private set; }

        public Type Implement { get; private set; }
    }
}
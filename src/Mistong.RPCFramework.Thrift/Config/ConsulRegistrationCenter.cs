using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
{
    public class ConsulRegistrationCenter : RegistrationCenter
    {
        public static readonly string RegistrationType = "consul";

        public override string Type { get { return RegistrationType; } }
    }
}
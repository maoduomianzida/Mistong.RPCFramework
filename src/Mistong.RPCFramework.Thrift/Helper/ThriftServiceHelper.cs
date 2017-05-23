using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift;

namespace Mistong.RPCFramework.Thrift
{
    internal static class ThriftServiceHelper
    {
        private static string[] _thriftClassSuffixs = new string[] { "+Iface", "+ISync", "+IAsync" };

        public static TProcessor GetProcessor(Type implementType)
        {
            Type interfaceType = ExtractThriftInterface(implementType);
        }

        public static Type ExtractThriftInterface(Type thriftType)
        {
            if (IsThriftInterface(thriftType))
            {
                return thriftType;
            }
            foreach (Type type in thriftType.GetInterfaces())
            {
                Type result = ExtractThriftInterface(type);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public static bool IsThriftInterface(Type type)
        {
            string typeName = type.Name;
            string suffixs = _thriftClassSuffixs.FirstOrDefault(tmp => type.FullName.EndsWith(tmp));

            return suffixs != null;
        }
    }
}

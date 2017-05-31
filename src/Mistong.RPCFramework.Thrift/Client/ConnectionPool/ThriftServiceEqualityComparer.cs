using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
{
    public class ThriftServiceEqualityComparer : IEqualityComparer<ThriftService>
    {
        public bool Equals(ThriftService x, ThriftService y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.Port == y.Port && x.Address == y.Address;
        }

        public int GetHashCode(ThriftService obj)
        {
            if (obj == null) return 0;

            int hashCode = 0;
            unchecked
            {
                hashCode = 3 * obj.Port.GetHashCode();
                hashCode = hashCode * 3 * obj.Address.GetHashCode();
            }

            return hashCode;
        }
    }
}
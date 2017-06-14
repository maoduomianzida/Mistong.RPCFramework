using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
{
    public class FilterComparer : IComparer<FilterInfo>
    {
        public int Compare(FilterInfo x, FilterInfo y)
        {
            if (x == null) return -1;
            if (y == null) return 1;

            return (x.Order - y.Order);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift.Transport;

namespace Mistong.RPCFramework.Thrift
{
    internal class TransportPoolItem
    {
        public TTransport Transport { get; set; }

        /// <summary>
        /// 是否空闲
        /// </summary>
        public bool IsFree { get; set; }

        /// <summary>
        /// 上次使用时间
        /// </summary>
        public DateTime LastUseTime { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
{
    public class ThriftClientConfig : ClientConfig
    {
        /// <summary>
        /// 连接池的最大连接数，默认50
        /// </summary>
        public int? ConnectionLimit { get; set; }

        /// <summary>
        /// 当连接达到最大限制时，等待其它线程释放连接的超时时间（毫秒），默认1000毫秒
        /// </summary>
        public int? WaitFreeMillisecond { get; set; }

        /// <summary>
        /// 连接池连接的超时时间，默认5分钟
        /// </summary>
        public TimeSpan? ConnectionOverdueInterval { get; set; }
    }
}
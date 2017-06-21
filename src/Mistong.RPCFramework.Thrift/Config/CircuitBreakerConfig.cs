using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework.Thrift
{
    /// <summary>
    /// 熔断设置
    /// </summary>
    public class CircuitBreakerConfig
    {
        /// <summary>
        /// 允许错误次数的时间间隔
        /// </summary>
        public TimeSpan AllowFailInterval { get; set; }

        /// <summary>
        /// 在间隔时间内允许错误的次数
        /// </summary>
        public int AllowFailTimes { get; set; }

        /// <summary>
        /// 打开状态下切换到半开状态的时间间隔
        /// </summary>
        public TimeSpan HalfOpenDuration { get; set; }

        /// <summary>
        /// 半开状态下允许的请求次数
        /// </summary>
        public int HalfOpenRequestLimit { get; set; }
    }
}

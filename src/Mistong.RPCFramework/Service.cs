using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework
{
    /// <summary>
    /// 服务信息
    /// </summary>
    public abstract class Service
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 服务类型
        /// </summary>
        public abstract string Type { get; }

        /// <summary>
        /// 端口号
        /// </summary>
        public virtual int Port { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public virtual string Address { get; set; }
    }
}
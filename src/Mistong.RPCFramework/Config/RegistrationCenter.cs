using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework
{
    /// <summary>
    /// 注册中心类
    /// </summary>
    public abstract class RegistrationCenter
    {
        /// <summary>
        /// 注册中心类型
        /// </summary>
        public abstract string Type { get; }

        /// <summary>
        /// 配置中心地址
        /// </summary>
        public virtual List<string> Clusters { get; set; }
    }
}

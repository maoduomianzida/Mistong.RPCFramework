using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework
{
    /// <summary>
    /// 客户端服务匹配接口
    /// </summary>
    public interface IServiceMatcher
    {
        /// <summary>
        /// 匹配服务
        /// </summary>
        void MatchService();
    }
}
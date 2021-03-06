﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework
{
    /// <summary>
    /// 服务发现接口
    /// </summary>
    public interface IServiceDiscoverer
    {
        /// <summary>
        /// 注册中心信息
        /// </summary>
        RegistrationCenter RegistrationCenter { get; set; }

        /// <summary>
        /// 发现服务
        /// </summary>
        /// <returns></returns>
        IEnumerable<Service> Discover(IEnumerable<ServiceMap> serviceMaps);
    }
}
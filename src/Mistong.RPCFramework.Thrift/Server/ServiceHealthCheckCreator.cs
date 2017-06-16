using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;
using System.Net.Sockets;
using System.Net;

namespace Mistong.RPCFramework.Thrift
{
    public class ServiceHealthCheckCreator : IServiceHealthCheckCreator
    {
        protected ServiceCheckConfig ServiceCheckConfig;

        public ServiceHealthCheckCreator(ServiceCheckConfig checkConfig)
        {
            ServiceCheckConfig = checkConfig ?? throw new ArgumentNullException(nameof(checkConfig));
        }

        public virtual AgentCheckRegistration CreateCheck()
        {
            string applicationName = ThriftServiceHelper.GetApplicationName();
            if(string.IsNullOrWhiteSpace(applicationName))
            {
                throw new Exception("ApplicationName不存在，无法设置健康检查ID");
            }
            applicationName = applicationName.ToLower();

            return new AgentCheckRegistration
            {
                ID = applicationName + ".check",
                Name = applicationName,
                TCP = ServiceCheckConfig.Address + ":" +ServiceCheckConfig.Port,
                Interval = ServiceCheckConfig.Interval,
                Timeout = ServiceCheckConfig.Timeout
            };
        }

        /// <summary>
        /// 开启服务健康检查接口
        /// </summary>
        public virtual void EnableHealthCheckInterface()
        {
            Task.Factory.StartNew(() =>
            {
                IPAddress ip = IPAddress.Parse(ServiceCheckConfig.Address);
                IPEndPoint endPoint = new IPEndPoint(ip, ServiceCheckConfig.Port);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(endPoint);
                socket.Listen(0);
                while (true)
                {
                    using (Socket serverSocket = socket.Accept()) { }
                }
            });
        }
    }
}
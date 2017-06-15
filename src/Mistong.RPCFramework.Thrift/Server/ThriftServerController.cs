using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thrift;
using Thrift.Protocol;
using Thrift.Server;
using Thrift.Transport;

namespace Mistong.RPCFramework.Thrift
{
    public class ThriftServerController : IServerController
    {
        private ICollection<TServer> _servers;
        private AutoResetEvent _waitHanlder;

        public ThriftServerController()
        {
            _servers = new Collection<TServer>();
            _waitHanlder = new AutoResetEvent(false);
        }

        private ILookup<int, ThriftService> TidyServices(IEnumerable<ThriftService> services)
        {
            return services.Where(tmp => tmp.Port != 0 && tmp.ServiceType != null).ToLookup(tmp => tmp.Port);
        }

        public virtual void Start(ServiceConfig serviceConfig)
        {
            ThriftService[] thriftServices = serviceConfig.Server.Services.Cast<ThriftService>().ToArray();
            IServiceRegistry registry = GlobalSetting.Container.GetService<IServiceRegistry>();
            if(registry == null)
            {
                throw new NullReferenceException("IServiceRegistry接口不能为空");
            }
            registry.RegistrationCenter = serviceConfig.RegistrationCenter;
            registry.Register(GetNeedRegisterServices(thriftServices).ToArray());
            ILookup<int, ThriftService> tidyServices = TidyServices(thriftServices);
            foreach(var group in tidyServices)
            {
                StartService(group);
            }
            if(tidyServices.Count > 0)
            {
                _waitHanlder.WaitOne();
            }
        }

        protected virtual IEnumerable<ThriftService> GetNeedRegisterServices(IEnumerable<ThriftService> services)
        {
            return from service in services where service.NeedRegister select service;
        }

        public virtual void Stop()
        {
            Parallel.ForEach(_servers, tmp => tmp.Stop());
            _waitHanlder.Set();
        }

        protected virtual void StartService(IGrouping<int,ThriftService> group)
        {
            TMultiplexedProcessor multiplexedProcessor = new TMultiplexedProcessor();
            IServiceActivator serviceActivator = GlobalSetting.Container.GetService<IServiceActivator>();
            if (serviceActivator == null)
            {
                throw new NullReferenceException("未设置IServiceActivator接口");
            }
            foreach (ThriftService service in group)
            {
                object instance = serviceActivator.Create(service.ServiceType);
                if (instance == null)
                {
                    throw new NullReferenceException($"无法创建服务{service.ServiceType.FullName}");
                }
                Type processorType = ThriftServiceHelper.GetProcessorType(service.ServiceType);
                if(processorType == null)
                {
                    throw new NullReferenceException($"无法找到服务{service.ServiceType.FullName}对应的Processor类");
                }
                TProcessor processor = ThriftServiceHelper.CreateProcessor(processorType, instance);
                if(processor == null)
                {
                    throw new NullReferenceException($"无法创建Processor{processorType.FullName}");
                }
                multiplexedProcessor.RegisterProcessor(service.Name, processor);
            }
            TServerTransport serverTransport = new TServerSocket(group.Key);
            serverTransport.Listen();
            TServer server = new TThreadPoolServer(multiplexedProcessor, serverTransport);
            _servers.Add(server);
            Task.Run(() => server.Serve());
        }
    }
}
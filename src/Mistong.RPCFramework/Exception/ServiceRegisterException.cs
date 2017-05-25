﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mistong.RPCFramework
{
    public class ServiceRegisterException : Exception
    {
        public RegistrationCenter ConfigCenter { get; private set; }

        public ServiceRegisterException(RegistrationCenter configCenter,string message = "",Exception innerException = null):base(message, innerException)
        {
            if (configCenter == null)
                throw new ArgumentNullException(nameof(configCenter));

            ConfigCenter = configCenter;
        }
    }
}
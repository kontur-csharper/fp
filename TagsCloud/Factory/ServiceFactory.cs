using System;
using System.Collections.Generic;
using TagsCloud.ResultOf;

namespace TagsCloud.Factory
{
    public abstract class ServiceFactory<TService> : IServiceFactory<TService>
    {
        protected readonly Dictionary<string, Func<TService>> services = new Dictionary<string, Func<TService>>();

        public abstract Result<TService> Create();

        public IEnumerable<string> GetServiceNames() => services.Keys;

        public IServiceFactory<TService> Register(string serviceName, Func<TService> creationFunc)
        {
            services[serviceName] = creationFunc;
            return this;
        }
    }
}

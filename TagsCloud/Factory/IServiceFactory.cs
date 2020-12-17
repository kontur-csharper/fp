using System;
using System.Collections.Generic;
using TagsCloud.ResultOf;

namespace TagsCloud.Factory
{
    public interface IServiceFactory<TService>
    {
        Result<TService> Create();
        IEnumerable<string> GetServiceNames();
        IServiceFactory<TService> Register(string serviceName, Func<TService> creationFunc);
    }
}

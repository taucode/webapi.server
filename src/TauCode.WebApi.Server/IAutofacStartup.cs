using Autofac;

namespace TauCode.WebApi.Server;

public interface IAutofacStartup
{
    ILifetimeScope AutofacContainer { get; }
}
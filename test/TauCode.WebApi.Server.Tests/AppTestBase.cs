using Autofac;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Globalization;

namespace TauCode.WebApi.Server.Tests;

[TestFixture]
public abstract class AppTestBase
{
    protected TestFactory Factory { get; private set; }
    protected HttpClient HttpClient { get; private set; }
    protected ILifetimeScope Container { get; private set; }

    protected ILifetimeScope SetupLifetimeScope { get; private set; }
    protected ILifetimeScope TestLifetimeScope { get; private set; }
    protected ILifetimeScope AssertLifetimeScope { get; private set; }

    [OneTimeSetUp]
    public void OneTimeSetUpBase()
    {
        Inflector.Inflector.SetDefaultCultureFunc = () => new CultureInfo("en-US");


        this.Factory = new TestFactory();

        this.HttpClient = this.Factory
            .WithWebHostBuilder(builder => builder.UseSolutionRelativeContentRoot(@"test\TauCode.WebApi.Server.Tests"))
            .CreateClient();

        var testServer = this.Factory.Factories.Single().Server;

        var startup = testServer.Services.GetService<IAutofacStartup>();
        this.Container = startup.AutofacContainer;
    }

    [OneTimeTearDown]
    public void OneTimeTearDownBase()
    {
        this.HttpClient.Dispose();
        this.Factory.Dispose();

        this.HttpClient = null;
        this.Factory = null;
    }

    [SetUp]
    public void SetUpBase()
    {
        // autofac stuff
        this.SetupLifetimeScope = this.Container.BeginLifetimeScope();
        this.TestLifetimeScope = this.Container.BeginLifetimeScope();
        this.AssertLifetimeScope = this.Container.BeginLifetimeScope();
    }

    [TearDown]
    public void TearDownBase()
    {
        this.SetupLifetimeScope.Dispose();
        this.TestLifetimeScope.Dispose();
        this.AssertLifetimeScope.Dispose();
    }
}
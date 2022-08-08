using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using Microsoft.OpenApi.Models;
using TauCode.Cqrs.Autofac;
using TauCode.Cqrs.Commands;
using TauCode.Cqrs.Queries;

namespace TauCode.WebApi.Server.Tests.AppHost;

public class Startup : IAutofacStartup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public ILifetimeScope AutofacContainer { get; private set; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        var coreAssembly = typeof(Startup).Assembly;

        services.AddControllers(options => options.Filters.Add(new ValidationFilterAttribute(coreAssembly)));
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title = "Demo Server RESTful Service",
                    Version = "v1"
                });
            c.CustomSchemaIds(x => x.FullName);
            c.EnableAnnotations();
        });
    }

    public void ConfigureContainer(ContainerBuilder containerBuilder)
    {
        var cqrsAssembly = typeof(Startup).Assembly;

        // command dispatching
        containerBuilder
            .RegisterType<CommandDispatcher>()
            .As<ICommandDispatcher>()
            .InstancePerLifetimeScope();

        containerBuilder
            .RegisterType<AutofacCommandHandlerFactory>()
            .As<ICommandHandlerFactory>()
            .InstancePerLifetimeScope();

        // register API ICommandHandler decorator
        containerBuilder
            .RegisterAssemblyTypes(cqrsAssembly)
            .Where(t => t.IsClosedTypeOf(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .AsSelf()
            .InstancePerLifetimeScope();

        // validators
        containerBuilder
            .RegisterAssemblyTypes(cqrsAssembly)
            .Where(t => t.IsClosedTypeOf(typeof(AbstractValidator<>)))
            .AsSelf()
            .InstancePerLifetimeScope();

        // query handling
        containerBuilder
            .RegisterType<QueryRunner>()
            .As<IQueryRunner>()
            .InstancePerLifetimeScope();

        containerBuilder
            .RegisterType<AutofacQueryHandlerFactory>()
            .As<IQueryHandlerFactory>()
            .InstancePerLifetimeScope();

        containerBuilder
            .RegisterAssemblyTypes(cqrsAssembly)
            .Where(t => t.IsClosedTypeOf(typeof(IQueryHandler<>)))
            .AsImplementedInterfaces()
            .AsSelf()
            .InstancePerLifetimeScope();

        containerBuilder
            .RegisterInstance(this)
            .As<IAutofacStartup>()
            .SingleInstance();
    }


    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

        app.UseSwagger();
        app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo Server RESTful Service"); });


        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
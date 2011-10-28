using System.Reflection;
using Autofac;
using AutoMapper;
using TestAutoMapper.Service;

namespace TestAutoMapper
{
    public static class BootStrapper
    {
        public static void Run()
        {
            var builder = new ContainerBuilder();

            // assemblies
            var assemblies = new[] { Assembly.GetExecutingAssembly() };

            // modules
            builder.RegisterAssemblyTypes(assemblies)
                   .AsClosedTypesOf(typeof(ITypeConverter<, >))
                   .AsSelf();

            IContainer container = null;
            // AutoMapper initialization
            Mapper.Initialize(x =>
            {
                x.ConstructServicesUsing(type => container.Resolve(type));
            });

            // startable which include mapper classes
            builder.RegisterAssemblyTypes(assemblies)
                   .Where(t => typeof(IStartable).IsAssignableFrom(t))
                   .As<IStartable>()
                   .SingleInstance();

            
            // services
            builder.RegisterType<OrderService>().As<IOrderService>();

            container = builder.Build();
        }
    }
}
